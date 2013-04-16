using System;
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
            Visit(expression);
            var result = new TranslateResult();
            result.QueryText = _query.ToString();
            return result;
        }

        private void AppendNewLine(Identation style)
        {
            _query.AppendLine();
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
                _query.Append(" ");
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
                    _query.Append(" NOT ");
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
            _query.Append("(");
            Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    _query.Append(" AND ");
                    break;
                case ExpressionType.AndAlso:
                    _query.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    _query.Append(" OR");
                    break;
                case ExpressionType.OrElse:
                    _query.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    _query.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    _query.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    _query.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _query.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    _query.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _query.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            Visit(b.Right);
            _query.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
                _query.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        _query.Append(((bool) c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        _query.Append("'");
                        _query.Append(c.Value);
                        _query.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                    default:
                        _query.Append(c.Value);
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
            _designDocName = _designDocName == null ? column.Name : _designDocName + "_" + column.Name;
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
                    //_query.Append(table.Name);
                    //_query.Append(" AS ");
                    //_query.Append(table.Alias);
                    break;
                case DbExpressionType.Select:
                    var select = (SelectExpression) source;
                    //_query.Append("(");
                    //AppendNewLine(Identation.Inner);
                    //Visit(select);
                    //AppendNewLine(Identation.Outer);
                    //_query.Append(")");
                    //_query.Append(" AS ");
                    //_query.Append(select.Alias);
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
    }

}