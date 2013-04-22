using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Biseth.Net.Settee.Linq
{
    internal class QueryFormatter : DbExpressionVisitor
    {
        private StringBuilder _query;
        private string _designDocName;
        private string _viewName;
        private List<string> _queryProperties;
        private List<string> _queryValues;
        private List<Statement> _statements;
        private int _level;
        private ExpressionType _lastExpressionType;

        internal TranslateResult Format(Expression expression)
        {
            _query = new StringBuilder();
            _queryProperties = new List<string>();
            _queryValues = new List<string>();
            _statements = new List<Statement>();
            _level = 0;
            // Start parsing the expression tree.
            Visit(expression);

            // Return the result.
            var result = new TranslateResult
                {
                    QueryText = _query.ToString(),
                    QueryProperties = _queryProperties,
                    QueryValues = _queryValues,
                    DesignDocName = (_designDocName ?? "").ToLower(),
                    ViewName = _viewName,
                    Statements = _statements,
                };
            return result;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported",
                                                                  u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            _level++;
            Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    _viewName += "And";
                    _lastExpressionType = ExpressionType.And;
                    break;
                case ExpressionType.AndAlso:
                    _viewName += "And";
                    _lastExpressionType = ExpressionType.And;
                    break;
                case ExpressionType.Or:
                    _viewName += "Or";
                    _lastExpressionType = ExpressionType.Or;
                    break;
                case ExpressionType.OrElse:
                    _viewName += "Or";
                    _lastExpressionType = ExpressionType.Or;
                    break;

                case ExpressionType.Equal:
                    _statements.Add(new Statement(_lastExpressionType, _level, b.Left, b.NodeType, b.Right));
                    break;
                case ExpressionType.NotEqual:
                    _statements.Add(new Statement(_lastExpressionType, _level, b.Left, b.NodeType, b.Right));
                    _viewName += "Not";
                    if (b.Left is ConstantExpression)
                    {
                        var leftExpression = b.Left as ConstantExpression;
                        if (leftExpression != null) 
                            _viewName += leftExpression.Value;
                    }
                    if (b.Right is ConstantExpression)
                    {
                        var rightExpression = b.Right as ConstantExpression;
                        if (rightExpression != null) 
                            _viewName += rightExpression.Value;
                    }
                    break;
                case ExpressionType.LessThan:
                    _statements.Add(new Statement(_lastExpressionType, _level, b.Left, b.NodeType, b.Right));
                    break;
                case ExpressionType.LessThanOrEqual:
                    _statements.Add(new Statement(_lastExpressionType, _level, b.Left, b.NodeType, b.Right));
                    break;
                case ExpressionType.GreaterThan:
                    _statements.Add(new Statement(_lastExpressionType, _level, b.Left, b.NodeType, b.Right));
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _statements.Add(new Statement(_lastExpressionType, _level, b.Left, b.NodeType, b.Right));
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            Visit(b.Right);
            _level--;
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.String:
                        _queryValues.Add("'" + c.Value + "'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                    default:
                        _queryValues.Add(c.Value.ToString());
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            if (!string.IsNullOrEmpty(column.Alias))
            {
            }
            _viewName = _viewName == null ? column.Name : _viewName + column.Name;
            _queryProperties.Add(column.Name);
            return column;
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            if (select.From != null)
            {
                VisitSource(select.From);
            }
            if (select.Where != null)
            {
                Visit(select.Where);
            }
            return select;
        }

        protected override Expression VisitSource(Expression source)
        {
            switch ((DbExpressionType) source.NodeType)
            {
                case DbExpressionType.Table:
                    var table = (TableExpression) source;
                    _designDocName = table.Name.ToLower();
                    break;
                case DbExpressionType.Select:
                    var select = (SelectExpression) source;
                    var tableExpression = @select.From as TableExpression;
                    if (tableExpression != null)
                        _designDocName = tableExpression.Name;
                    break;
                default:
                    throw new InvalidOperationException("Select source is not valid type");
            }
            return source;
        }

        protected enum Identation
        {
            Same,
            Inner,
            Outer
        }
    }

    public class Statement
    {
        private readonly ExpressionType _lastExpressionType;
        private readonly int _level;
        private readonly Expression _left;
        private readonly ExpressionType _nodeType;
        private readonly Expression _right;

        public Statement(ExpressionType lastExpressionType, int level, Expression left, ExpressionType nodeType, Expression right)
        {
            _lastExpressionType = lastExpressionType;
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

        public ExpressionType LastExpressionType
        {
            get { return _lastExpressionType; }
        }
    }

    public class TranslateResult
    {
        internal string QueryText;
        internal string DesignDocName;
        internal string ViewName;
        public List<string> QueryProperties { get; set; }
        public List<string> QueryValues { get; set; }
        public List<Statement> Statements { get; set; }
    }

}