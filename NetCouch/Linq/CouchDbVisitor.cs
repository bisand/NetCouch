using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace NetCouch.Linq
{
    public class CouchDbVisitor<T> : ExpressionVisitor
    {
        private readonly CouchDbTranslation _queryTranslation;
        private ExpressionType _lastExpressionType;
        private int _level;

        public CouchDbVisitor(CouchDbTranslation? queryTranslation)
        {
            _queryTranslation = queryTranslation ?? new CouchDbTranslation();
        }

        public CouchDbTranslation Execute(Expression expression)
        {
            Visit(expression);
            return _queryTranslation;
        }

        public override Expression? Visit(Expression? node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var expression = Visit(node.Expression);
            if (expression != node.Expression)
            {
                return Expression.MakeMemberAccess(expression, node.Member);
            }

            if (expression is ConstantExpression constantExpression)
            {
                var container = constantExpression.Value;
                var member = node.Member;
                if (member is FieldInfo fieldInfo)
                {
                    var value = fieldInfo.GetValue(container);
                    return Expression.Constant(value);
                }
                if (member is PropertyInfo propertyInfo)
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

        protected override Expression VisitConstant(ConstantExpression? node)
        {
            Debug.Write(node?.Value);
            switch (Type.GetTypeCode(node?.Value?.GetType()))
            {
                case TypeCode.String:
                    _queryTranslation.QueryValues.Add("'" + node?.Value + "'");
                    break;
                case TypeCode.Object:
                    if (node?.Type != null && node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(CouchDbQuery<>))
                    {
                        var type = node.Type.GetGenericArguments()[0];
                        _queryTranslation.DesignDocName = type.Name;
                    }
                    break;
                default:
                    if (node?.Value is not null)
                    {
                        _queryTranslation.QueryValues.Add($"{node?.Value}");
                    }
                    break;
            }
            return node != null ? base.VisitConstant(node) : Expression.Constant(null, typeof(object));
        }

        protected override Expression VisitBinary(BinaryExpression bnode)
        {
            _level++;
            Visit(bnode.Left);
            HandleBinaryNodeType(bnode);
            Visit(bnode.Right);
            _level--;
            return bnode;
        }

        private void HandleBinaryNodeType(BinaryExpression bnode)
        {
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
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    _queryTranslation.Statements.Add(new Statement(_lastExpressionType, _level, bnode.Left, bnode.NodeType, bnode.Right));
                    if (bnode.NodeType == ExpressionType.NotEqual)
                    {
                        _queryTranslation.ViewName += "Not";
                        AppendConstantValuesToViewName(bnode);
                    }
                    break;
                default:
                    throw new NotSupportedException($"The binary operator {bnode.NodeType} is not supported");
            }
        }

        private void AppendConstantValuesToViewName(BinaryExpression bnode)
        {
            if (bnode.Left is ConstantExpression leftExpression)
            {
                _queryTranslation.ViewName += leftExpression.Value;
            }
            if (bnode.Right is ConstantExpression rightExpression)
            {
                _queryTranslation.ViewName += rightExpression.Value;
            }
        }

        private Expression VisitBinaryComparison(BinaryExpression node)
        {
            var constant = node.Left as ConstantExpression ?? node.Right as ConstantExpression;
            var memberAccess = node.Left as MemberExpression ?? node.Right as MemberExpression;

            if (memberAccess == null || constant == null)
            {
                throw new NotSupportedException($"One of the operand not supported for operator {node.NodeType}");
            }

            if (constant.Value == null)
            {
                throw new NotSupportedException($"NULL is not supported for {node}");
            }

            var constantTypeCode = Type.GetTypeCode(constant.Value.GetType());
            if (constantTypeCode != TypeCode.Int32 && constantTypeCode != TypeCode.String)
            {
                throw new NotSupportedException($"Constant {constant} is of an unsupported type {constant.Value.GetType().Name}");
            }

            TranslateStandardComparison(node.NodeType, constant, memberAccess);
            return node;
        }

        private void TranslateStandardComparison(ExpressionType nodeType, ConstantExpression constant, MemberExpression memberAccess)
        {
            // Implementation needed
        }
    }
}