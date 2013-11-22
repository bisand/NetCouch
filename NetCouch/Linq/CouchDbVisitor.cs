using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Biseth.Net.Couch.Linq
{
    public class CouchDbVisitor<T> : ExpressionVisitor
    {
        private readonly CouchDbTranslation _queryTranslation;
        private ExpressionType _lastExpressionType;
        private int _level;

        public CouchDbVisitor(CouchDbTranslation queryTranslation)
        {
            _queryTranslation = queryTranslation ?? new CouchDbTranslation();
        }

        public CouchDbTranslation Execute(Expression expression)
        {
            Visit(expression);
            return _queryTranslation;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            // Recurse down to see if we can simplify...
            var expression = Visit(node.Expression);
            if (expression != node.Expression)
            {
                return Expression.MakeMemberAccess(expression, node.Member);
            }

            // If we've ended up with a constant, and it's a property or a field,
            // we can simplify ourselves to a constant
            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
            {
                var container = constantExpression.Value;
                var member = node.Member;
                var fieldInfo = member as FieldInfo;
                if (fieldInfo != null)
                {
                    var value = fieldInfo.GetValue(container);
                    return Expression.Constant(value);
                }
                var propertyInfo = member as PropertyInfo;
                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(container, null);
                    return Expression.Constant(value);
                }
            }

            if (node.Member.MemberType == MemberTypes.Field || node.Member.MemberType == MemberTypes.Property)
            {
                _queryTranslation.QueryProperties.Add(node.Member.Name);
                _queryTranslation.ViewName += node.Member.Name;
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Debug.Write(node.Value);
            switch (Type.GetTypeCode(node.Value.GetType()))
            {
                case TypeCode.String:
                    _queryTranslation.QueryValues.Add("'" + node.Value + "'");
                    break;
                case TypeCode.Object:
                    //throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", node.Value));
                    if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(CouchDbQuery<>))
                    {
                        var type = node.Type.GetGenericArguments()[0];
                        _queryTranslation.DesignDocName = type.Name;
                    }
                    break;
                default:
                    _queryTranslation.QueryValues.Add(node.Value.ToString());
                    break;
            }
            return base.VisitConstant(node);
        }

        protected override Expression VisitBinary(BinaryExpression bnode)
        {
            _level++;
            Visit(bnode.Left);
            switch (bnode.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _lastExpressionType = ExpressionType.And;
                    _queryTranslation.ViewName += "And";
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _lastExpressionType = ExpressionType.Or;
                    _queryTranslation.ViewName += "Or";
                    break;

                case ExpressionType.Equal:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, bnode.Left, bnode.NodeType, bnode.Right));
                    break;
                case ExpressionType.NotEqual:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, bnode.Left, bnode.NodeType, bnode.Right));
                    _queryTranslation.ViewName += "Not";
                    if (bnode.Left is ConstantExpression)
                    {
                        var leftExpression = bnode.Left as ConstantExpression;
                        if (leftExpression != null)
                            _queryTranslation.ViewName += leftExpression.Value;
                    }
                    if (bnode.Right is ConstantExpression)
                    {
                        var rightExpression = bnode.Right as ConstantExpression;
                        if (rightExpression != null)
                            _queryTranslation.ViewName += rightExpression.Value;
                    }
                    break;
                case ExpressionType.LessThan:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, bnode.Left, bnode.NodeType, bnode.Right));
                    break;
                case ExpressionType.LessThanOrEqual:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, bnode.Left, bnode.NodeType, bnode.Right));
                    break;
                case ExpressionType.GreaterThan:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, bnode.Left, bnode.NodeType, bnode.Right));
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, bnode.Left, bnode.NodeType, bnode.Right));
                    return VisitBinaryComparison(bnode);
                default:
                    throw new NotSupportedException(string.Format(
                        "The binary operator {0} is not supported", bnode.NodeType));
            }
            Visit(bnode.Right);
            _level--;
            return bnode;
        }

        private Expression VisitBinaryComparison(BinaryExpression node)
        {
            var constant = (node.Left as ConstantExpression ?? node.Right as ConstantExpression);
            var memberAccess = (node.Left as MemberExpression ?? node.Right as MemberExpression);

            if (memberAccess == null || constant == null)
            {
                throw new NotSupportedException(string.Format("One of the operand not supported for operator {0}", node.NodeType));
            }

            if (constant.Value == null)
            {
                throw new NotSupportedException(string.Format("NULL is not supported for {0}", node));
            }

            var constantTypeCode = Type.GetTypeCode(constant.Value.GetType());
            if (constantTypeCode != TypeCode.Int32 && constantTypeCode != TypeCode.String)
                throw new NotSupportedException(string.Format("Constant {0} is of an unsupported type {1}",
                                                              constant, constant.Value.GetType().Name));
            TranslateStandardComparison(node.NodeType, constant, memberAccess);
            return node;
        }

        private void TranslateStandardComparison(ExpressionType nodeType, ConstantExpression constant, MemberExpression memberAccess)
        {
        }
    }
}