using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Biseth.Net.Settee.Linq.Old;

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

        public CouchDbTranslation Execute(Expression expression)
        {
            VisitExpression(expression);
            return _queryTranslation;
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
                        VisitStatement(Expression.MakeBinary(ExpressionType.Equal, expression.Object, expression.Arguments[0]));
                        break;
                    case 2:
                        VisitStatement(Expression.MakeBinary(ExpressionType.Equal, expression.Arguments[0], expression.Arguments[1]));
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
                        {
                            //luceneQuery.OpenSubclause();
                        }
                        VisitExpression(((UnaryExpression)expression.Arguments[1]).Operand);
                        if (_chainedWhere == false && _insideWhere > 1)
                        {
                            //luceneQuery.CloseSubclause();
                        }
                        if (_chainedWhere)
                        {
                            //luceneQuery.CloseSubclause();
                        }
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
                        VisitSelect(((UnaryExpression)expression.Arguments[1]).Operand);
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

        private bool _insideSelect;
        private readonly bool _isMapReduce;
        private ExpressionType _lastExpressionType;
        private int _level;

        private void VisitSelect(Expression operand)
        {
            var lambdaExpression = operand as LambdaExpression;
            var body = lambdaExpression != null ? lambdaExpression.Body : operand;
            switch (body.NodeType)
            {
                case ExpressionType.Convert:
                    _insideSelect = true;
                    try
                    {
                        VisitSelect(((UnaryExpression)body).Operand);
                    }
                    finally
                    {
                        _insideSelect = false;
                    }
                    break;
                case ExpressionType.MemberAccess:
                    MemberExpression memberExpression = ((MemberExpression)body);
                    AddToFieldsToFetch(GetSelectPath(memberExpression), memberExpression.Member.Name);
                    if (_insideSelect == false)
                    {
                        //foreach (var renamedField in FieldsToRename.Where(x => x.OriginalField == memberExpression.Member.Name).ToArray())
                        //{
                        //    FieldsToRename.Remove(renamedField);
                        //}
                        //FieldsToRename.Add(new RenamedField
                        //{
                        //    NewField = null,
                        //    OriginalField = memberExpression.Member.Name
                        //});
                    }
                    break;
                //Anonymous types come through here .Select(x => new { x.Cost } ) doesn't use a member initializer, even though it looks like it does
                //See http://blogs.msdn.com/b/sreekarc/archive/2007/04/03/immutable-the-new-anonymous-type.aspx
                case ExpressionType.New:
                    var newExpression = ((NewExpression)body);
                    newExpressionType = newExpression.Type;
                    for (int index = 0; index < newExpression.Arguments.Count; index++)
                    {
                        var field = newExpression.Arguments[index] as MemberExpression;
                        if (field == null)
                            continue;
                        //var expression = linqPathProvider.GetMemberExpression(newExpression.Arguments[index]);
                        //var renamedField = GetSelectPath(expression);
                        //AddToFieldsToFetch(renamedField, newExpression.Members[index].Name);
                    }
                    break;
                //for example .Select(x => new SomeType { x.Cost } ), it's member init because it's using the object initializer
                case ExpressionType.MemberInit:
                    var memberInitExpression = ((MemberInitExpression)body);
                    newExpressionType = memberInitExpression.NewExpression.Type;
                    foreach (MemberBinding t in memberInitExpression.Bindings)
                    {
                        var field = t as MemberAssignment;
                        if (field == null)
                            continue;

                        //var expression = linqPathProvider.GetMemberExpression(field.Expression);
                        //var renamedField = GetSelectPath(expression);
                        //AddToFieldsToFetch(renamedField, field.Member.Name);
                    }
                    break;
                case ExpressionType.Parameter: // want the full thing, so just pass it on.
                    break;

                default:
                    throw new NotSupportedException("Node not supported: " + body.NodeType);
            }
        }

        private string GetSelectPath(MemberExpression expression)
        {
            var sb = new StringBuilder(expression.Member.Name);
            expression = expression.Expression as MemberExpression;
            while (expression != null)
            {
                sb.Insert(0, ".");
                sb.Insert(0, expression.Member.Name);
                expression = expression.Expression as MemberExpression;
            }
            return sb.ToString();
        }

        private void AddToFieldsToFetch(string docField, string renamedField)
        {
            //var identityProperty = luceneQuery.DocumentConvention.GetIdentityProperty(typeof(T));
            //if (identityProperty != null && identityProperty.Name == docField)
            //{
            //    FieldsToFetch.Add(Constants.DocumentIdFieldName);
            //    if (identityProperty.Name != renamedField)
            //    {
            //        docField = Constants.DocumentIdFieldName;
            //    }
            //}
            //else
            //{
            //    FieldsToFetch.Add(docField);
            //}
            //if (docField != renamedField)
            //{
            //    if (identityProperty == null)
            //    {
            //        var idPropName = luceneQuery.DocumentConvention.FindIdentityPropertyNameFromEntityName(luceneQuery.DocumentConvention.GetTypeTagName(typeof(T)));
            //        if (docField == idPropName)
            //        {
            //            FieldsToRename.Add(new RenamedField
            //            {
            //                NewField = renamedField,
            //                OriginalField = Constants.DocumentIdFieldName
            //            });
            //        }
            //    }
            //    FieldsToRename.Add(new RenamedField
            //    {
            //        NewField = renamedField,
            //        OriginalField = docField
            //    });
            //}
        }

        private void VisitMemberAccess(MemberExpression expression, bool b)
        {
            _queryTranslation.ViewName = _queryTranslation.ViewName == null
                                             ? expression.Member.Name
                                             : _queryTranslation.ViewName + expression.Member.Name;
            _queryTranslation.QueryProperties.Add(expression.Member.Name);
        }

        private void VisitBinaryExpression(BinaryExpression expression)
        {
            _level++;
            VisitExpression(expression.Left);
            switch (expression.NodeType)
            {
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    VisitOrElse(expression);
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    VisitAndAlso(expression);
                    break;
                case ExpressionType.NotEqual:
                    VisitStatement(expression);
                    break;
                case ExpressionType.Equal:
                    VisitStatement(expression);
                    break;
                case ExpressionType.GreaterThan:
                    VisitStatement(expression);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    VisitStatement(expression);
                    break;
                case ExpressionType.LessThan:
                    VisitStatement(expression);
                    break;
                case ExpressionType.LessThanOrEqual:
                    VisitStatement(expression);
                    break;
            }
            VisitExpression(expression.Right);
            _level--;
        }

        private void VisitStatement(BinaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, expression.Left,
                                                                   expression.NodeType, expression.Right));
                    break;
                case ExpressionType.NotEqual:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, expression.Left,
                                                                   expression.NodeType, expression.Right));
                    _queryTranslation.ViewName += "Not";
                    if (expression.Left is ConstantExpression)
                    {
                        var leftExpression = expression.Left as ConstantExpression;
                        if (leftExpression != null)
                            _queryTranslation.ViewName += leftExpression.Value;
                    }
                    if (expression.Right is ConstantExpression)
                    {
                        var rightExpression = expression.Right as ConstantExpression;
                        if (rightExpression != null)
                            _queryTranslation.ViewName += rightExpression.Value;
                    }
                    break;
            }
        }

        private void VisitAndAlso(BinaryExpression expression)
        {
            _queryTranslation.ViewName += "And";
            _lastExpressionType = ExpressionType.And;
        }

        private void VisitOrElse(BinaryExpression expression)
        {
            _queryTranslation.ViewName += "Or";
            _lastExpressionType = ExpressionType.Or;
        }
    }
}