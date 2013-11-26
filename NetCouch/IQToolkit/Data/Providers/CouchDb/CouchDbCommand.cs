using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Biseth.Net.Couch.IQToolkit.Data.Providers.CouchDb
{
    public class CouchDbCommand : DbCommand
    {
        private DbParameterCollection _parameters;

        public CouchDbCommand()
        {
//            _parameters = new CouchDbParameterCollection();
        }

        public override void Prepare()
        {
        }

        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return _parameters; }
        }

        protected override DbTransaction DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; }

        public override void Cancel()
        {
        }

        protected override DbParameter CreateDbParameter()
        {
            return new OleDbParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new DataTableReader(new DataTable());
        }

        public override int ExecuteNonQuery()
        {
            return 0;
        }

        public override object ExecuteScalar()
        {
            return 0;
        }
    }
}