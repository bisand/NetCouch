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

        internal TranslateResult Format(Expression expression)
        {
            _query = new StringBuilder();
            _queryProperties = new List<string>();
            _queryValues = new List<string>();
            
            // Start parsing the expression tree.
            Visit(expression);

            // Return the result.
            var result = new TranslateResult
                {
                    QueryText = _query.ToString(),
                    QueryProperties = _queryProperties,
                    QueryValues = _queryValues,
                    DesignDocName = (_designDocName ?? "").ToLower(),
                    ViewName = _viewName
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
            _queryProperties.Add("(");
            _queryValues.Add("(");
            Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    _queryProperties.Add("{AND}");
                    _queryValues.Add("{AND}");
                    _viewName += "And";
                    break;
                case ExpressionType.AndAlso:
                    _queryProperties.Add("{AND}");
                    _queryValues.Add("{AND}");
                    _viewName += "And";
                    break;
                case ExpressionType.Or:
                    _queryProperties.Add("{OR}");
                    _queryValues.Add("{OR}");
                    _viewName += "Or";
                    break;
                case ExpressionType.OrElse:
                    _queryProperties.Add("{OR}");
                    _queryValues.Add("{OR}");
                    _viewName += "Or";
                    break;
                case ExpressionType.Equal:
                    _queryProperties.Add("{=}");
                    _queryValues.Add("{=}");
                    break;
                case ExpressionType.NotEqual:
                    _queryProperties.Add("{!=}");
                    _queryValues.Add("{!=}");
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
                    _queryProperties.Add("{<}");
                    _queryValues.Add("{<}");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _queryProperties.Add("{<=}");
                    _queryValues.Add("{<=}");
                    break;
                case ExpressionType.GreaterThan:
                    _queryProperties.Add("{>}");
                    _queryValues.Add("{>}");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _queryProperties.Add("{>=}");
                    _queryValues.Add("{>=}");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            Visit(b.Right);
            _queryProperties.Add(")");
            _queryValues.Add(")");
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

    internal class TranslateResult
    {
        internal string QueryText;
        internal string DesignDocName;
        internal string ViewName;
        internal LambdaExpression Projector;
        public List<string> QueryProperties { get; set; }
        public List<string> QueryValues { get; set; }
    }

}