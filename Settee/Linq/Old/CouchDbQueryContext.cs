using System;
using System.Linq.Expressions;
using Biseth.Net.Settee.CouchDb.Api;

namespace Biseth.Net.Settee.Linq.Old
{
    public class CouchDbQueryContext
    {
        public static TResult Execute<TResult>(ICouchApi couchApi, Expression expression, bool b)
        {
            // The expression must represent a query over the data source. 
            if (!IsQueryOverDataSource(expression))
                throw new InvalidProgramException("No query over the data source was specified.");

            // Find the call to Where() and get the lambda expression predicate.
            var whereFinder = new InnermostWhereFinder();
            var whereExpression = whereFinder.GetInnermostWhere(expression);
            var type = whereExpression.Arguments[0].Type;
            var genericArgument = type.GetGenericArguments()[0];

            var lambdaExpression = (LambdaExpression)((UnaryExpression)(whereExpression.Arguments[1])).Operand;
            // Send the lambda expression through the partial evaluator.
            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            // Get the place name(s) to query the Web service with.
            //LocationFinder lf = new LocationFinder(lambdaExpression.Body);
            //List<string> locations = lf.Locations;
            //if (locations.Count == 0)
            //    throw new InvalidQueryException("You must specify at least one place name in your query.");

            // Call the Web service and get the results.
            return (TResult)Activator.CreateInstance(typeof(TResult));
        }

        private static bool IsQueryOverDataSource(Expression expression)
        {
            // If expression represents an unqueried IQueryable data source instance, 
            // expression is of type ConstantExpression, not MethodCallExpression. 
            return (expression is MethodCallExpression);
        }
    }
}