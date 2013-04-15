using System;
using System.Linq.Expressions;
using System.Reflection;
using Biseth.Net.Settee.CouchDb.Api;
using Biseth.Net.Settee.CouchDb.Api.Extensions;

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
            return Translate(expression).CommandText;
        }

        public override object Execute(Expression expression)
        {
            var result = Translate(expression);
            var projector = result.Projector.Compile();

            var reader = _couchApi.Root().Db(_couchApi.DefaultDatabase).DesignDoc("test").View("test").Get<T>();

            var elementType = TypeSystem.GetElementType(expression.Type);
            return Activator.CreateInstance(
                typeof (ProjectionReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {reader, projector},
                null
                );
        }

        private static TranslateResult Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            var proj = (ProjectionExpression) new QueryBinder().Bind(expression);
            var commandText = new QueryFormatter().Format(proj.Source);
            var projector = new ProjectionBuilder().Build(proj.Projector);
            return new TranslateResult {CommandText = commandText, Projector = projector};
        }

        internal class TranslateResult
        {
            internal string CommandText;
            internal LambdaExpression Projector;
        }
    }
}