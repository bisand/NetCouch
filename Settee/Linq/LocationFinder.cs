using System.Collections.Generic;
using System.Linq.Expressions;
using Biseth.Net.Settee.Linq.TestModels;

namespace Biseth.Net.Settee.Linq
{
    internal class LocationFinder : ExpressionVisitor
    {
        private readonly Expression _expression;
        private List<string> _locations;

        public LocationFinder(Expression exp)
        {
            _expression = exp;
        }

        public List<string> Locations
        {
            get
            {
                if (_locations == null)
                {
                    _locations = new List<string>();
                    Visit(_expression);
                }
                return _locations;
            }
        }

        protected override Expression VisitBinary(BinaryExpression be)
        {
            if (be.NodeType == ExpressionType.Equal)
            {
                if (ExpressionTreeHelpers.IsMemberEqualsValueExpression(be, typeof (Place), "Name"))
                {
                    _locations.Add(ExpressionTreeHelpers.GetValueFromEqualsExpression(be, typeof (Place), "Name"));
                    return be;
                }
                if (ExpressionTreeHelpers.IsMemberEqualsValueExpression(be, typeof (Place), "State"))
                {
                    _locations.Add(ExpressionTreeHelpers.GetValueFromEqualsExpression(be, typeof (Place), "State"));
                    return be;
                }
                return base.VisitBinary(be);
            }
            return base.VisitBinary(be);
        }
    }
}