using System.Linq.Expressions;
using System.Reflection;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbVisitor<T> : ExpressionVisitor
    {
        private readonly ICouchDbQueryGenerator _queryGenerator;
        private readonly CouchDbTranslation _queryTranslation;

        public CouchDbVisitor(ICouchDbQueryGenerator queryGenerator, CouchDbTranslation queryTranslation)
        {
            _queryGenerator = queryGenerator;
            _queryTranslation = queryTranslation;
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
    }
}