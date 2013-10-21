using System.Linq.Expressions;

namespace Biseth.Net.Couch.Linq
{
    public class Statement
    {
        private readonly ExpressionType _lastExprType;
        private readonly Expression _left;
        private readonly int _level;
        private readonly ExpressionType _nodeType;
        private readonly Expression _right;

        public Statement(ExpressionType lastExprType, int level, Expression left, ExpressionType nodeType, Expression right)
        {
            _lastExprType = lastExprType;
            _level = level;
            _left = left;
            _nodeType = nodeType;
            _right = right;
        }

        public int Level
        {
            get { return _level; }
        }

        public Expression Left
        {
            get { return _left; }
        }

        public ExpressionType NodeType
        {
            get { return _nodeType; }
        }

        public Expression Right
        {
            get { return _right; }
        }

        public ExpressionType? LastExprType
        {
            get { return _lastExprType; }
        }
    }
}