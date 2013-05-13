using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProviderProcessor<T>
    {
        private readonly ICouchDbQueryGenerator _queryGenerator;
        private readonly CouchDbTranslation _queryTranslation;

        private bool _chainedWhere;
        private int _insideWhere;
        private Expression<Func<T, bool>> predicate;
        private Type newExpressionType;
        private string currentPath = string.Empty;
        private int subClauseDepth;

        public CouchDbQueryProviderProcessor(ICouchDbQueryGenerator queryGenerator, CouchDbTranslation queryTranslation)
        {
            _queryGenerator = queryGenerator;
            _queryTranslation = queryTranslation;
        }

        public object Execute(Expression expression)
        {
            VisitExpression(expression);
            return new List<T>();
        }

        protected void VisitExpression(Expression expression)
        {
            if (expression is BinaryExpression)
            {
                VisitBinaryExpression((BinaryExpression) expression);
            }
            else
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        VisitMemberAccess((MemberExpression) expression, true);
                        break;
                    case ExpressionType.Not:
                        var unaryExpressionOp = ((UnaryExpression) expression).Operand;
                        switch (unaryExpressionOp.NodeType)
                        {
                            case ExpressionType.MemberAccess:
                                VisitMemberAccess((MemberExpression) unaryExpressionOp, false);
                                break;
                            case ExpressionType.Call:
                                VisitMethodCall((MethodCallExpression) unaryExpressionOp, true);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(unaryExpressionOp.NodeType.ToString());
                        }
                        break;
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        VisitExpression(((UnaryExpression) expression).Operand);
                        break;
                    default:
                        if (expression is MethodCallExpression)
                        {
                            VisitMethodCall((MethodCallExpression) expression);
                        }
                        else if (expression is LambdaExpression)
                        {
                            VisitExpression(((LambdaExpression) expression).Body);
                        }
                        break;
                }
            }
        }

        private void VisitMethodCall(MethodCallExpression expression, bool negated = false)
        {
            var declaringType = expression.Method.DeclaringType;
            Debug.Assert(declaringType != null);
            if (declaringType != typeof(string) && expression.Method.Name == "Equals")
            {
                switch (expression.Arguments.Count)
                {
                    case 1:
                        VisitEquals(Expression.MakeBinary(ExpressionType.Equal, expression.Object, expression.Arguments[0]));
                        break;
                    case 2:
                        VisitEquals(Expression.MakeBinary(ExpressionType.Equal, expression.Arguments[0], expression.Arguments[1]));
                        break;
                    default:
                        throw new ArgumentException("Can't understand Equals with " + expression.Arguments.Count + " arguments");
                }
                return;
            }
            if (declaringType == typeof(Queryable))
            {
                VisitQueryableMethodCall(expression);
                return;
            }

            if (declaringType == typeof(String))
            {
                //VisitStringMethodCall(expression);
                return;
            }

            if (declaringType == typeof(Enumerable))
            {
                //VisitEnumerableMethodCall(expression, negated);
                return;
            }
            if (declaringType.IsGenericType &&
                declaringType.GetGenericTypeDefinition() == typeof(List<>))
            {
                //VisitListMethodCall(expression);
                return;
            }

            var method = declaringType.Name + "." + expression.Method.Name;
            throw new NotSupportedException(string.Format("Method not supported: {0}. Expression: {1}.", method, expression));
        }

        private void VisitQueryableMethodCall(MethodCallExpression expression)
        {
            switch (expression.Method.Name)
            {
                case "OfType":
                    VisitExpression(expression.Arguments[0]);
                    break;
                case "Where":
                    {
                        _insideWhere++;
                        VisitExpression(expression.Arguments[0]);
                        if (_chainedWhere)
                        {
                            //luceneQuery.AndAlso();
                            //luceneQuery.OpenSubclause();
                        }
                        if (_chainedWhere == false && _insideWhere > 1)
                            //luceneQuery.OpenSubclause();
                        VisitExpression(((UnaryExpression)expression.Arguments[1]).Operand);
                        if (_chainedWhere == false && _insideWhere > 1)
                            //luceneQuery.CloseSubclause();
                        if (_chainedWhere)
                            //luceneQuery.CloseSubclause();
                        _chainedWhere = true;
                        _insideWhere--;
                        break;
                    }
                case "Select":
                    {
                        if (expression.Arguments[0].Type.IsGenericType &&
                            expression.Arguments[0].Type.GetGenericTypeDefinition() == typeof(IQueryable<>) &&
                            expression.Arguments[0].Type != expression.Arguments[1].Type)
                        {
                            //luceneQuery.AddRootType(expression.Arguments[0].Type.GetGenericArguments()[0]);
                        }
                        VisitExpression(expression.Arguments[0]);
                        //VisitSelect(((UnaryExpression)expression.Arguments[1]).Operand);
                        break;
                    }
                case "Skip":
                    {
                        VisitExpression(expression.Arguments[0]);
                        //VisitSkip(((ConstantExpression)expression.Arguments[1]));
                        break;
                    }
                case "Take":
                    {
                        VisitExpression(expression.Arguments[0]);
                        //VisitTake(((ConstantExpression)expression.Arguments[1]));
                        break;
                    }
                case "First":
                case "FirstOrDefault":
                    {
                        VisitExpression(expression.Arguments[0]);
                        if (expression.Arguments.Count == 2)
                        {
                            if (_chainedWhere)
                            {
                                //luceneQuery.AndAlso();
                            }
                            VisitExpression(((UnaryExpression)expression.Arguments[1]).Operand);
                        }

                        if (expression.Method.Name == "First")
                        {
                            //VisitFirst();
                        }
                        else
                        {
                            //VisitFirstOrDefault();
                        }
                        _chainedWhere = _chainedWhere || expression.Arguments.Count == 2;
                        break;
                    }
                case "Single":
                case "SingleOrDefault":
                    {
                        VisitExpression(expression.Arguments[0]);
                        if (expression.Arguments.Count == 2)
                        {
                            if (_chainedWhere)
                            {
                                //luceneQuery.AndAlso();
                            }
                            VisitExpression(((UnaryExpression)expression.Arguments[1]).Operand);
                        }

                        if (expression.Method.Name == "Single")
                        {
                            //VisitSingle();
                        }
                        else
                        {
                            //VisitSingleOrDefault();
                        }
                        _chainedWhere = _chainedWhere || expression.Arguments.Count == 2;
                        break;
                    }
                case "All":
                    {
                        VisitExpression(expression.Arguments[0]);
                        //VisitAll((Expression<Func<T, bool>>)((UnaryExpression)expression.Arguments[1]).Operand);
                        break;
                    }
                case "Any":
                    {
                        VisitExpression(expression.Arguments[0]);
                        if (expression.Arguments.Count == 2)
                        {
                            if (_chainedWhere)
                            {
                                //luceneQuery.AndAlso();
                            }
                            VisitExpression(((UnaryExpression)expression.Arguments[1]).Operand);
                        }

                        //VisitAny();
                        break;
                    }
                case "Count":
                    {
                        VisitExpression(expression.Arguments[0]);
                        if (expression.Arguments.Count == 2)
                        {
                            if (_chainedWhere)
                            {
                                //luceneQuery.AndAlso();
                            }
                            VisitExpression(((UnaryExpression)expression.Arguments[1]).Operand);
                        }

                        //VisitCount();
                        break;
                    }
                case "LongCount":
                    {
                        VisitExpression(expression.Arguments[0]);
                        if (expression.Arguments.Count == 2)
                        {
                            if (_chainedWhere)
                            {
                                //luceneQuery.AndAlso();
                            }
                            VisitExpression(((UnaryExpression)expression.Arguments[1]).Operand);
                        }

                        //VisitLongCount();
                        break;
                    }
                case "Distinct":
                    //luceneQuery.GroupBy(AggregationOperation.Distinct);
                    VisitExpression(expression.Arguments[0]);
                    break;
                case "OrderBy":
                case "ThenBy":
                case "ThenByDescending":
                case "OrderByDescending":
                    VisitExpression(expression.Arguments[0]);
                    //VisitOrderBy((LambdaExpression)((UnaryExpression)expression.Arguments[1]).Operand, expression.Method.Name.EndsWith("Descending"));
                    break;
                default:
                    {
                        throw new NotSupportedException("Method not supported: " + expression.Method.Name);
                    }
            }
        }

        private void VisitMemberAccess(MemberExpression expression, bool b)
        {
        }

        private void VisitBinaryExpression(BinaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.OrElse:
                    VisitOrElse(expression);
                    break;
                case ExpressionType.AndAlso:
                    VisitAndAlso(expression);
                    break;
                case ExpressionType.NotEqual:
                    VisitNotEquals(expression);
                    break;
                case ExpressionType.Equal:
                    VisitEquals(expression);
                    break;
                case ExpressionType.GreaterThan:
                    VisitGreaterThan(expression);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    VisitGreaterThanOrEqual(expression);
                    break;
                case ExpressionType.LessThan:
                    VisitLessThan(expression);
                    break;
                case ExpressionType.LessThanOrEqual:
                    VisitLessThanOrEqual(expression);
                    break;
            }
        }

        private void VisitLessThanOrEqual(BinaryExpression expression)
        {
        }

        private void VisitLessThan(BinaryExpression expression)
        {
        }

        private void VisitGreaterThanOrEqual(BinaryExpression expression)
        {
        }

        private void VisitGreaterThan(BinaryExpression expression)
        {
        }

        private void VisitEquals(BinaryExpression expression)
        {
        }

        private void VisitNotEquals(BinaryExpression expression)
        {
        }

        private void VisitAndAlso(BinaryExpression expression)
        {
        }

        private void VisitOrElse(BinaryExpression expression)
        {
        }
    }
}