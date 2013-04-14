using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace Biseth.Net.Settee.Linq
{
    public class DbQueryProvider : QueryProvider
    {
        private readonly DbConnection _connection;

        public DbQueryProvider(DbConnection connection)
        {
            _connection = connection;
        }

        public override string GetQueryText(Expression expression)
        {
            return Translate(expression).CommandText;
        }

        public override object Execute(Expression expression)
        {
            var result = Translate(expression);
            var projector = result.Projector.Compile();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = result.CommandText;
            var reader = cmd.ExecuteReader();

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