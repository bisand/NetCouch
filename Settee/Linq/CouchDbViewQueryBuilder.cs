using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbViewQueryBuilder
    {
        private readonly StringBuilder _query;
        private readonly StringBuilder _view;

        public CouchDbViewQueryBuilder()
        {
            _query = new StringBuilder();
            _view = new StringBuilder();
            _view.Append("function(doc) { if (doc.doc_type && doc.doc_type == '");
        }

        public ViewAndQuery Build(CouchDbTranslation result)
        {
            var notEqualStatements = result.Statements.Where(x => x.NodeType == ExpressionType.NotEqual).ToList();
            var equalStatemens = result.Statements.Where(x => x.NodeType == ExpressionType.Equal).ToList();
            var equalGroups = equalStatemens.Select(x => x.Level).GroupBy(g => g).ToList();

            _view.Append(result.DesignDocName);
            _view.Append("') { ");
            // emit operatins goes here

            var count = notEqualStatements.Count();
            if (count > 0)
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
                _query.Append("keys=[");
            Statement prevExpr = null;
            foreach (var eq in equalStatemens)
            {
                if (prevExpr != null && eq.LastExprType == ExpressionType.Or)
                {
                    _view.Remove(_view.Length - 1, 1);
                    _view.Append("],null);");
                    _query.Remove(_query.Length - 1, 1);
                    _query.Append("],");
                }
                if (prevExpr == null || eq.LastExprType == ExpressionType.Or)
                {
                    _query.Append("[");
                    _view.Append("emit([");
                }
                if (eq.Left is MemberExpression && eq.Right is ConstantExpression)
                {
                    _view.Append("doc." + (eq.Left as MemberExpression).Member.Name + ",");
                    if ((eq.Right as ConstantExpression).Value is string)
                        _query.Append("\"" + (eq.Right as ConstantExpression).Value + "\",");
                    else
                        _query.Append((eq.Right as ConstantExpression).Value + ",");
                }
                else if (eq.Left is ConstantExpression && eq.Right is MemberExpression)
                {
                    _view.Append("doc." + (eq.Right as MemberExpression).Member.Name + ",");
                    if ((eq.Left as ConstantExpression).Value is string)
                        _query.Append("\"" + (eq.Left as ConstantExpression).Value + "\",");
                    else
                        _query.Append((eq.Left as ConstantExpression).Value + ",");
                }
                prevExpr = eq;
            }

            _view.Remove(_view.Length - 1, 1);
            _view.Append("],null);");
            _query.Remove(_query.Length - 1, 1);
            _query.Append("]]");
            if (count > 0)
                _view.Append(" } ");

            _view.Append(" } ");
            _view.Append(" }");

            return new ViewAndQuery {View = _view.ToString(), Query = _query.ToString()};
        }
    }
}