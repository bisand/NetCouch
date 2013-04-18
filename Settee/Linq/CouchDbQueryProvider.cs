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
            var notEquals = result.Statements.Where(x => x.NodeType == ExpressionType.NotEqual);
            _view.Append(result.DesignDocName);
            _view.Append("') { ");
            // emit operatins goes here
            foreach (var statement in notEquals)
            {
                _view.Append("if (");
                if (statement.Left.NodeType is ColumnExpression)
                {
                    _view.Append("");
                }
                else
                {
                    
                }
            }
            _view.Append(" } ");
            _view.Append(" } ");
        }
    }
}