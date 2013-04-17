using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Biseth.Net.Settee.Linq
{
    internal class QueryFormatter : DbExpressionVisitor
    {
        private int _depth;
        private int _indent = 2;
        private StringBuilder _query;
        private string _designDocName;
        private string _viewName;
        private List<string> _queryProperties;
        private List<string> _queryValues;

        internal QueryFormatter()
        {
        }

        internal int IdentationWidth
        {
            get { return _indent; }
            set { _indent = value; }
        }

        internal TranslateResult Format(Expression expression)
        {
            _query = new StringBuilder();
            _queryProperties = new List<string>();
            _queryValues = new List<string>();
            Visit(expression);
            var result = new TranslateResult();
            result.QueryText = _query.ToString();
            result.QueryProperties = _queryProperties;
            result.QueryValues = _queryValues;
            result.DesignDocName = (_designDocName ?? "").ToLower();
            result.ViewName = _viewName;
            return result;
        }

        private void AppendNewLine(Identation style)
        {
            //_query.AppendLine();
            if (style == Identation.Inner)
            {
                _depth++;
            }
            else if (style == Identation.Outer)
            {
                _depth--;
                System.Diagnostics.Debug.Assert(_depth >= 0);
            }
            for (int i = 0, n = _depth*_indent; i < n; i++)
            {
                //_query.Append(" ");
            }
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
                    //_query.Append(" NOT ");
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
                        _viewName += (b.Left as ConstantExpression).Value;
                    if (b.Right is ConstantExpression)
                        _viewName += (b.Right as ConstantExpression).Value;
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
                //_query.Append("NULL");
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
                //_query.Append(column.Alias);
                //_query.Append(".");
            }
            //_query.Append(column.Name);
            _viewName = _viewName == null ? column.Name : _viewName + column.Name;
            _queryProperties.Add(column.Name);
            return column;
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            _query.Append("keys=");
            for (int i = 0, n = select.Columns.Count; i < n; i++)
            {
                //var column = select.Columns[i];
                //if (i > 0)
                //{
                //    sb.Append(", ");
                //}
                //var c = Visit(column.Expression) as ColumnExpression;
                //if (c == null || c.Name != select.Columns[i].Name)
                //{
                //    sb.Append(" AS ");
                //    sb.Append(column.Name);
                //}
            }
            if (select.From != null)
            {
                AppendNewLine(Identation.Same);
                //_query.Append("FROM ");
                
                VisitSource(select.From);
            }
            if (select.Where != null)
            {
                AppendNewLine(Identation.Same);
                //_query.Append("WHERE ");
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
                    _designDocName = (select.From as TableExpression).Name;
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