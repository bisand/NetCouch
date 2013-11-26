using System.Linq.Expressions;
using System.Reflection;
using Biseth.Net.Couch.IQToolkit.Data.Common.Expressions;
using Biseth.Net.Couch.IQToolkit.Data.Common.Language;

namespace Biseth.Net.Couch.IQToolkit.Data.Providers.CouchDb
{
    public class CouchDbLanguage : QueryLanguage
    {
        private readonly CouchDbTypeSystem _typeSystem = new CouchDbTypeSystem();

        public override QueryTypeSystem TypeSystem
        {
            get { return _typeSystem; }
        }

        public override Expression GetGeneratedIdExpression(MemberInfo member)
        {
            return new FunctionExpression(TypeHelper.GetMemberType(member), "", null);
        }
        public static readonly QueryLanguage Default = new CouchDbLanguage();
    }
}