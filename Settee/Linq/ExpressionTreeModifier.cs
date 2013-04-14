using System.Linq;
using System.Linq.Expressions;
using Biseth.Net.Settee.Linq.TestModels;

namespace Biseth.Net.Settee.Linq
{
    internal class ExpressionTreeModifier : ExpressionVisitor
    {
        private readonly IQueryable<Place> _queryablePlaces;

        internal ExpressionTreeModifier(IQueryable<Place> places)
        {
            _queryablePlaces = places;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            // Replace the constant QueryableTerraServerData arg with the queryable Place collection. 
            return c.Type == typeof (CouchDbQuery<Place>) ? Expression.Constant(_queryablePlaces) : c;
        }
    }
}