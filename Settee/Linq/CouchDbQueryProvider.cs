using System.Text;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.CouchDb.Api.Extensions;
using Biseth.Net.Settee.Models.Couch.DesignDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace Biseth.Net.Settee.Linq
{
    public class CouchDbQueryProvider<T> : QueryProvider
    {
        private readonly ICouchApi _couchApi;

        public CouchDbQueryProvider(ICouchApi couchApi)
        {
            _couchApi = couchApi;
        }

        public override string GetQueryText(Expression expression)
        {
            return Translate(expression).QueryText;
        }

        public override object Execute(Expression expression)
        {
            var result = Translate(expression);
            new CouchDbViewBuilder().Build(result);

            // p => (p.Make == "Saab" && p.Model == "1337" || p.Make != "Volvo" && p.Model != "2013")
            // The not equal operators should be put into an index. If a combined statement produce an empty query,
            // we probably would like to do another query with no keys...

            var queryString = "keys=" + Uri.EscapeDataString("[[\"Saab\",\"1337\"]]") + "&include_docs=true";
            var headResult = _couchApi.Root().Db(_couchApi.DefaultDatabase).DesignDoc(result.DesignDocName).View(result.ViewName, queryString).Head();
            if (headResult != null && headResult.StatusCode == HttpStatusCode.NotFound)
            {
                //CreateView
            }
            var queryResult = _couchApi.Root().Db(_couchApi.DefaultDatabase).DesignDoc(result.DesignDocName).View(result.ViewName, queryString).Get<ViewResponse<T>>();

            var elementType = TypeSystem.GetElementType(expression.Type);
            return (queryResult.DataDeserialized.Rows ?? new List<ViewRow<T>>()).Select(x => x.Doc);
            //return Activator.CreateInstance(
            //    typeof (ProjectionReader<>).MakeGenericType(elementType),
            //    BindingFlags.Instance | BindingFlags.NonPublic, null,
            //    new object[] {reader, projector},
            //    null
            //    );
        }

        private static TranslateResult Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            var proj = (ProjectionExpression)new QueryBinder().Bind(expression);
            var result = new QueryFormatter().Format(proj.Source);
            return result;
        }
    }

    public class CouchDbViewBuilder
    {
        private StringBuilder _view;

        public CouchDbViewBuilder()
        {
            _view = new StringBuilder();
            _view.Append("function(doc) { if (doc.doc_type && doc.doc_type == '");
        }

        public void Build(TranslateResult result)
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

            foreach (var equalGroup in equalGroups)
            {
                var statements = @equals.Where(x => x.Level == equalGroup.Key).ToList();
                if (statements.Count > 1)
                {
                    _view.Append("emit([");
                    foreach (var statement in statements)
                    {
                        if (statement.Right is ConstantExpression && (statement.Right as ConstantExpression).Value is string)
                            _view.Append("'" + (statement.Right as ConstantExpression).Value + "',");
                        else if (statement.Right is ConstantExpression)
                            _view.Append((statement.Right as ConstantExpression).Value + ",");
                    }
                    _view.Remove(_view.Length-1, 1);
                    _view.Append("],null);");
                }
                else if (statements.Count > 0)
                {
                    _view.Append("emit(");
                    if (statements[0].Right is ConstantExpression && (statements[0].Right as ConstantExpression).Value is string)
                        _view.Append("'" + (statements[0].Right as ConstantExpression).Value + "'");
                    else if (statements[0].Right is ConstantExpression)
                        _view.Append((statements[0].Right as ConstantExpression).Value);
                    _view.Append(",null);");
                }

            }
            
            if (count > 0)
                _view.Append(" } ");

            _view.Append(" } ");
            _view.Append(" }");
        }
    }
}