using System;
using System.Data;
using System.Data.Common;
using System.Net;
using Biseth.Net.Couch.Db.Api;

namespace Biseth.Net.Couch.IQToolkit.Data.Providers.CouchDb
{
    public class CouchDbConnection : DbConnection
    {
        private readonly CouchDatabase _couchDatabase;

        public CouchDbConnection(CouchDatabase couchDatabase)
        {
            _couchDatabase = couchDatabase;
            ConnectionString = couchDatabase.ServerUrl;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new CouchDbTransaction();
        }

        public override void Close()
        {
        }

        public override void ChangeDatabase(string databaseName)
        {
            _couchDatabase.ChangeDatabase(databaseName);
        }

        public override void Open()
        {
            _couchDatabase.Initialize();
        }

        public override string ConnectionString { get; set; }

        public override string Database
        {
            get { return _couchDatabase.DefaultDatabase; }
        }

        public override ConnectionState State
        {
            get
            {
                var responseData = _couchDatabase.Initialize();
                if (responseData.StatusCode == HttpStatusCode.OK)
                {
                    var version = responseData.DataDeserialized.Version;
                    if (!string.IsNullOrWhiteSpace(version))
                        return ConnectionState.Open;
                }
                return ConnectionState.Closed;
            }
        }

        public override string DataSource
        {
            get { return _couchDatabase.ServerUrl; }
        }

        public override string ServerVersion
        {
            get
            {
                var responseData = _couchDatabase.Initialize();
                if (responseData.StatusCode == HttpStatusCode.OK)
                {
                    var version = responseData.DataDeserialized.Version;
                    if (!string.IsNullOrWhiteSpace(version))
                        return version;
                }
                return "n/a";
            }
        }

        protected override DbCommand CreateDbCommand()
        {
            return new CouchDbCommand();
        }
    }
}