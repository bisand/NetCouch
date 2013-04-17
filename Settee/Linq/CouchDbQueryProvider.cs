using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.CouchDb.Api.Extensions;
using Biseth.Net.Settee.Models.Couch.DesignDoc;

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
            var projector = result.Projector.Compile();

            var queryString = "keys=" + Uri.EscapeDataString("[[\"Saab\",\"1337\"]]") + "&include_docs=true";
            var headResult = _couchApi.Root().Db(_couchApi.DefaultDatabase).DesignDoc(result.DesignDocName).View(result.ViewName, queryString).Head();
            if (headResult != null && headResult.StatusCode == HttpStatusCode.NotFound)
            {
                //CreateView
            }
            var queryResult = _couchApi.Root().Db(_couchApi.DefaultDatabase).DesignDoc(result.DesignDocName).View(result.ViewName, queryString).Get<ViewResponse<T>>();

            var elementType = TypeSystem.GetElementType(expression.Type);
            return (queryResult.DataDeserialized.Rows ?? new List<ViewRow<T>>()).Select(x=>x.Doc);
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
            var proj = (ProjectionExpression) new QueryBinder().Bind(expression);
            var result = new QueryFormatter().Format(proj.Source);
            var projector = new ProjectionBuilder().Build(proj.Projector);
            result.Projector = projector;
            return result;
        }

    }
}