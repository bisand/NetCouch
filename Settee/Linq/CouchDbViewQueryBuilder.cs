using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbViewQueryBuilder
    {
        private readonly StringBuilder _view;
        private readonly StringBuilder _query;

        public CouchDbViewQueryBuilder()
        {
            _query = new StringBuilder();
            _view = new StringBuilder();
            _view.Append("function(doc) { if (doc.doc_type && doc.doc_type == '");
        }

        public ViewQuery Build(TranslateResult result)
        {
            var notEquals = result.Statements.Where(x => x.NodeType == ExpressionType.NotEqual).ToList();
            var equals = result.Statements.Where(x => x.NodeType == ExpressionType.Equal).ToList();
            var equalGroups = @equals.Select(x=>x.Level).GroupBy(g=>g).ToList();
           
            _view.Append(result.DesignDocName);
            _view.Append("') { ");
            // emit operatins goes here

            var count = notEquals.Count();
            if (count > 0)
            {
                _view.Append("if (");

                foreach (var statement in notEquals)
                {
                    if (statement.Left is ColumnExpression && statement.Right is ConstantExpression)
                    {
                        _view.Append("doc." + (statement.Left as ColumnExpression).Name + " != ");
                        _view.Append("'" + (statement.Right as ConstantExpression).Value + "' && ");
                    }
                    else if (statement.Left is ConstantExpression && statement.Right is ColumnExpression)
                    {
                        _view.Append("doc." + (statement.Right as ColumnExpression).Name + " != ");
                        _view.Append("'" + (statement.Left as ConstantExpression).Value + "' && ");
                    }
                }
                _view.Remove(_view.Length - 4, 4);
                _view.Append(") { ");
            }

            _query.Append("keys=[");
            foreach (var equalGroup in equalGroups)
            {
                var statements = @equals.Where(x => x.Level == equalGroup.Key).ToList();
                if (statements.Count > 1)
                {
                    _query.Append("[");
                    _view.Append("emit([");
                    foreach (var statement in statements)
                    {
                        if (statement.Right is ConstantExpression && statement.Left is ColumnExpression)
                        {
                            _view.Append("doc." + (statement.Left as ColumnExpression).Name + ",");
                            if ((statement.Right as ConstantExpression).Value is string)
                                _query.Append("\"" + (statement.Right as ConstantExpression).Value + "\",");
                            else
                                _query.Append((statement.Right as ConstantExpression).Value + ",");
                        }
                        else if (statement.Left is ConstantExpression && statement.Right is ColumnExpression)
                        {
                            _view.Append("doc." + (statement.Right as ColumnExpression).Name + ",");
                            if ((statement.Left as ConstantExpression).Value is string)
                                _query.Append("\"" + (statement.Left as ConstantExpression).Value + "\",");
                            else
                                _query.Append((statement.Left as ConstantExpression).Value + ",");
                        }
                    }
                    _view.Remove(_view.Length - 1, 1);
                    _view.Append("],null);");
                    _query.Remove(_query.Length - 1, 1);
                    _query.Append("],");
                }
                else if (statements.Count > 0)
                {
                    _view.Append("emit(");
                    if (statements[0].Right is ConstantExpression && statements[0].Left is ColumnExpression)
                    {
                        _view.Append("doc." + (statements[0].Left as ColumnExpression).Name);
                        if ((statements[0].Right as ConstantExpression).Value is string)
                            _query.Append("\"" + (statements[0].Right as ConstantExpression).Value + "\"");
                        else
                            _query.Append((statements[0].Right as ConstantExpression).Value);
                    }
                    else if (statements[0].Left is ConstantExpression && statements[0].Right is ColumnExpression)
                    {
                        _view.Append("doc." + (statements[0].Right as ColumnExpression).Name);
                        if ((statements[0].Left as ConstantExpression).Value is string)
                            _query.Append("\"" + (statements[0].Left as ConstantExpression).Value + "\"");
                        else
                            _query.Append((statements[0].Left as ConstantExpression).Value);
                    }
                    _view.Append(",null);");
                    _query.Append(",");
                }

            }

            _query.Remove(_query.Length - 1, 1);
            _query.Append("]");
            if (count > 0)
                _view.Append(" } ");

            _view.Append(" } ");
            _view.Append(" }");

            return new ViewQuery {View = _view.ToString(), Query = _query.ToString()};
        }
    }

    public class ViewQuery
    {
        public string View { get; set; }
        public string Query { get; set; }
    }
}