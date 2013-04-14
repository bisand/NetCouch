using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Biseth.Net.Settee.Linq
{
    public class Customers
    {
        public string City;
        public string ContactName;
        public string Country;
        public string CustomerID;
        public string Phone;
    }

    public class Orders
    {
        public string CustomerID;
        public DateTime OrderDate;
        public int OrderID;
    }

    public class Northwind
    {
        public Query<Customers> Customers;
        public Query<Orders> Orders;

        public Northwind(DbConnection connection)
        {
            QueryProvider provider = new DbQueryProvider(connection);
            Customers = new Query<Customers>(provider);
            Orders = new Query<Orders>(provider);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var constr =
                @"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\data\Northwind.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            using (var con = new SqlConnection(constr))
            {
                con.Open();
                var db = new Northwind(con);

                var city = "London";
                var query = db.Customers.Select(c => new
                    {
                        Name = c.ContactName,
                        Location = new
                            {
                                City = c.City,
                                Country = c.Country
                            }
                    }).Where(c => c.Location.City == city);

                Console.WriteLine("Query:\n{0}\n", query);

                var list = query.ToList();
                foreach (var item in list)
                {
                    Console.WriteLine("{0}", item);
                }

                Console.ReadLine();
            }
        }
    }
}