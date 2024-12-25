using System.Linq;
using NetCouch;
using NetCouch.IQToolkit;
using NetCouch.IQToolkit.Data.Common;
using NetCouch.IQToolkit.Data.Mapping;
using NetCouch.IQToolkit.Data.Providers.CouchDb;
using NUnit.Framework;

namespace NetCouchTests
{
    [TestFixture]
    public class EntityProviderTests
    {
        [Test]
        public void TestNewInstance()
        {
            using (var database = new CouchDatabase("http://localhost:5984/"))
            {
                var mapping = new ImplicitMapping();
                var policy = new QueryPolicy();

                var provider = new CouchDbQueryProvider(new CouchDbConnection(database), mapping, policy);
                var query = new Query<Car>(provider);
                var queryable = query.Where(x => x.Make == "Audi" && x.HorsePowers == 123);
                var cars = queryable.ToList();
                foreach (var car in cars)
                {
                    car.Model = "Cool";
                }
            }
        }
    }
}