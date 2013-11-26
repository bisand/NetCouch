using System.Data;
using System.Data.Common;

namespace Biseth.Net.Couch.IQToolkit.Data.Providers.CouchDb
{
    public class CouchDbTransaction : DbTransaction
    {
        public override void Commit()
        {
            throw new System.NotImplementedException();
        }

        public override void Rollback()
        {
            throw new System.NotImplementedException();
        }

        protected override DbConnection DbConnection
        {
            get { throw new System.NotImplementedException(); }
        }

        public override IsolationLevel IsolationLevel
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}