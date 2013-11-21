using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Biseth.Net.Couch.Linq
{
    public class CouchDbVisitor<T> : ExpressionVisitor
    {
        private readonly CouchDbTranslation _queryTranslation;

        public CouchDbVisitor(CouchDbTranslation queryTranslation)
        {
            _queryTranslation = queryTranslation;
        }

        public CouchDbTranslation Execute(Expression expression)
        {
            Visit(expression);
            return _queryTranslation;
        }

        //public override Expression Visit(Expression node)
        //{
        //    return base.Visit(node);
        //}

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

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Debug.Write(node.Value);
            return base.VisitConstant(node);
        }

        protected override Expression VisitBinary(BinaryExpression bnode)
        {
            switch (bnode.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    Visit(bnode.Left);
                    Visit(bnode.Right);
                    return bnode;

                case ExpressionType.Equal:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    return VisitBinaryComparison(bnode);
                default:
                    throw new NotSupportedException(string.Format(
                        "The binary operator {0} is not supported", bnode.NodeType));
            }
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