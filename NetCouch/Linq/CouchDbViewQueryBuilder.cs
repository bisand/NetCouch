using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Biseth.Net.Couch.Linq
{
    public class CouchDbViewQueryBuilder
    {
        private readonly StringBuilder _query;
        private readonly CouchDbTranslation _translation;
        private readonly StringBuilder _view;

        public CouchDbViewQueryBuilder(CouchDbTranslation translation)
        {
            _translation = translation;
            _query = new StringBuilder();
            _view = new StringBuilder();
            _view.Append("function(doc) { if (doc.doc__type && doc.doc__type == '");
        }

        public ViewAndQuery Build()
        {
            var notEqualStatements = _translation.Statements.Where(x => x.NodeType == ExpressionType.NotEqual).ToList();
            var equalStatemens = _translation.Statements.Where(x => x.NodeType == ExpressionType.Equal).ToList();
            var equalGroups = equalStatemens.Select(x => x.Level).GroupBy(g => g).ToList();

            _view.Append(_translation.DesignDocName);
            _view.Append("') { ");
            // emit operatins goes here

            var notEqualCount = notEqualStatements.Count();
            if (notEqualCount > 0)
            {
                _view.Append("if (");
                var i = 0;
                foreach (var statement in notEqualStatements)
                {
                    if (i > 0 && statement.LastExprType == ExpressionType.And)
                        _view.Append(" && ");
                    else if (i > 0 && statement.LastExprType == ExpressionType.Or)
                        _view.Append(" || ");

                    if (statement.Left is MemberExpression && statement.Right is ConstantExpression)
                    {
                        _view.Append("doc." + (statement.Left as MemberExpression).Member.Name + " != ");
                        _view.Append("'" + (statement.Right as ConstantExpression).Value + "'");
                    }
                    else if (statement.Left is ConstantExpression && statement.Right is MemberExpression)
                    {
                        _view.Append("doc." + (statement.Right as MemberExpression).Member.Name + " != ");
                        _view.Append("'" + (statement.Left as ConstantExpression).Value + "'");
                    }
                    i++;
                }
                _view.Append(") { ");
            }

            if (equalStatemens.Any())
            {
                _query.Append("keys=[");
                Statement prevExpr = null;
                foreach (var eq in equalStatemens)
                {
                    if (prevExpr == null || eq.LastExprType == ExpressionType.Or)
                    {
                        _query.Append("[");
                        _view.Append("emit([");
                    }
                    if (prevExpr != null && eq.LastExprType == ExpressionType.Or)
                    {
                        _view.Remove(_view.Length - 1, 1);
                        _view.Append("],null);");
                        _query.Remove(_query.Length - 1, 1);
                        _query.Append("],");
                    }

                    string memberName;
                    object expressionValue;
                    if (IsMemberExpression(eq.Left, out memberName) && ContainsValue(eq.Right, out expressionValue))
                    {
                        _view.AppendFormat("doc.{0},", memberName);
                        if (expressionValue is string)
                            _query.AppendFormat("'{0}',", expressionValue);
                        else
                            _query.AppendFormat("{0},", expressionValue);
                    }
                    else if (ContainsValue(eq.Left, out expressionValue) && IsMemberExpression(eq.Right, out memberName))
                    {
                        _view.AppendFormat("doc.{0},", memberName);
                        if (expressionValue is string)
                            _query.AppendFormat("'{0}',", expressionValue);
                        else
                            _query.AppendFormat("{0},", expressionValue);
                    }
                    prevExpr = eq;
                }
                _view.Remove(_view.Length - 1, 1);
                _view.Append("],null);");
                _query.Remove(_query.Length - 1, 1);
                _query.Append("]]");
            }
            else
            {
                _view.Append("emit(null, null);");
            }

            if (notEqualCount > 0)
                _view.Append(" } ");

            _view.Append(" } ");
            _view.Append(" }");

            return new ViewAndQuery {View = _view.ToString(), Query = _query.ToString()};
        }

        private static bool IsMemberExpression(Expression expression, out string memberName)
        {
            memberName = null;
            if (expression is MemberExpression)
            {
                memberName = (expression as MemberExpression).Member.Name;
                return true;
            }
            return false;
        }

        private static bool ContainsValue(Expression expression, out object expressionValue)
        {
            expressionValue = null;

            var constExpr = expression as ConstantExpression;
            if (constExpr != null)
            {
                object value = new CouchDbVisitor<object>(null).Visit(constExpr);
                expressionValue = value;
                return true;
            }
            var memberExp = expression as MemberExpression;
            if (memberExp != null)
            {
                object value = new CouchDbVisitor<object>(null).Visit(memberExp);
                if (ContainsValue(memberExp.Expression, out value))
                {
                    expressionValue = value;
                    return true;
                }
            }

            return false;
        }
    }
}