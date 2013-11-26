using System;
using System.Collections.Generic;
using System.Data.Common;
using Biseth.Net.Couch.IQToolkit.Data.Common;
using Biseth.Net.Couch.IQToolkit.Data.Common.Language;
using Biseth.Net.Couch.IQToolkit.Data.Common.Mapping;

namespace Biseth.Net.Couch.IQToolkit.Data.Providers.CouchDb
{
    public class CouchDbQueryProvider : DbEntityProvider
    {
        public CouchDbQueryProvider(CouchDbConnection connection, QueryMapping mapping, QueryPolicy policy)
            : base(connection, CouchDbLanguage.Default, mapping, policy)
        {
        }

        protected override QueryExecutor CreateExecutor()
        {
            return new Executor(this);
        }

        public override DbEntityProvider New(DbConnection connection)
        {
            return base.New(connection);
        }

        public override DbEntityProvider New(DbConnection connection, QueryMapping mapping, QueryPolicy policy)
        {
            return new CouchDbQueryProvider((CouchDbConnection)connection, mapping, policy);
        }

        public new class Executor : DbEntityProvider.Executor
        {
            private CouchDbQueryProvider _provider;

            public Executor(CouchDbQueryProvider provider)
                : base(provider)
            {
                _provider = provider;
            }

            protected override bool BufferResultRows
            {
                get { return base.BufferResultRows; }
            }

            protected override DbCommand GetCommand(QueryCommand query, object[] paramValues)
            {
                return base.GetCommand(query, paramValues);
            }

            protected override void AddParameter(DbCommand command, QueryParameter parameter, object value)
            {
                base.AddParameter(command, parameter, value);
            }

            public override IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues)
            {
                return base.Execute(command, fnProjector, entity, paramValues);
            }

            public override object Convert(object value, Type type)
            {
                return base.Convert(value, type);
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream)
            {
                return base.ExecuteBatch(query, paramSets, batchSize, stream);
            }

            public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, MappingEntity entity, int batchSize, bool stream)
            {
                return base.ExecuteBatch(query, paramSets, fnProjector, entity, batchSize, stream);
            }

            public override int ExecuteCommand(QueryCommand query, object[] paramValues)
            {
                return base.ExecuteCommand(query, paramValues);
            }

            public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues)
            {
                return base.ExecuteDeferred(query, fnProjector, entity, paramValues);
            }

            protected override DbDataReader ExecuteReader(DbCommand command)
            {
                return base.ExecuteReader(command);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            protected override void GetParameterValues(DbCommand command, object[] paramValues)
            {
                base.GetParameterValues(command, paramValues);
            }

            protected override void LogCommand(QueryCommand command, object[] paramValues)
            {
                base.LogCommand(command, paramValues);
            }

            protected override void LogMessage(string message)
            {
                base.LogMessage(message);
            }

            protected override void LogParameters(QueryCommand command, object[] paramValues)
            {
                base.LogParameters(command, paramValues);
            }

            protected override IEnumerable<T> Project<T>(DbDataReader reader, Func<FieldReader, T> fnProjector, MappingEntity entity, bool closeReader)
            {
                return base.Project(reader, fnProjector, entity, closeReader);
            }

            public override int RowsAffected
            {
                get { return base.RowsAffected; }
            }

            protected override void SetParameterValues(QueryCommand query, DbCommand command, object[] paramValues)
            {
                base.SetParameterValues(query, command, paramValues);
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }
    }
}