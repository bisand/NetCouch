using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Biseth.Net.Settee.Linq
{
    internal enum DbExpressionType
    {
        Table = 1000, // make sure these don't overlap with ExpressionType
        Column,
        Select,
        Projection
    }

    internal class TableExpression : Expression
    {
        private string alias;
        private string name;

        internal TableExpression(Type type, string alias, string name)
            : base((ExpressionType) DbExpressionType.Table, type)
        {
            this.alias = alias;
            this.name = name;
        }

        internal string Alias
        {
            get { return alias; }
        }

        internal string Name
        {
            get { return name; }
        }
    }

    internal class ColumnExpression : Expression
    {
        private string alias;
        private string name;
        private int ordinal;

        internal ColumnExpression(Type type, string alias, string name, int ordinal)
            : base((ExpressionType) DbExpressionType.Column, type)
        {
            this.alias = alias;
            this.name = name;
            this.ordinal = ordinal;
        }

        internal string Alias
        {
            get { return alias; }
        }

        internal string Name
        {
            get { return name; }
        }

        internal int Ordinal
        {
            get { return ordinal; }
        }
    }

    internal class ColumnDeclaration
    {
        private Expression expression;
        private string name;

        internal ColumnDeclaration(string name, Expression expression)
        {
            this.name = name;
            this.expression = expression;
        }

        internal string Name
        {
            get { return name; }
        }

        internal Expression Expression
        {
            get { return expression; }
        }
    }

    internal class SelectExpression : Expression
    {
        private string alias;
        private ReadOnlyCollection<ColumnDeclaration> columns;
        private Expression from;
        private Expression where;

        internal SelectExpression(Type type, string alias, IEnumerable<ColumnDeclaration> columns, Expression from,
                                  Expression where)
            : base((ExpressionType) DbExpressionType.Select, type)
        {
            this.alias = alias;
            this.columns = columns as ReadOnlyCollection<ColumnDeclaration>;
            if (this.columns == null)
            {
                this.columns = new List<ColumnDeclaration>(columns).AsReadOnly();
            }
            this.from = from;
            this.where = where;
        }

        internal string Alias
        {
            get { return alias; }
        }

        internal ReadOnlyCollection<ColumnDeclaration> Columns
        {
            get { return columns; }
        }

        internal Expression From
        {
            get { return @from; }
        }

        internal Expression Where
        {
            get { return @where; }
        }
    }

    internal class ProjectionExpression : Expression
    {
        private Expression projector;
        private SelectExpression source;

        internal ProjectionExpression(SelectExpression source, Expression projector)
            : base((ExpressionType) DbExpressionType.Projection, projector.Type)
        {
            this.source = source;
            this.projector = projector;
        }

        internal SelectExpression Source
        {
            get { return source; }
        }

        internal Expression Projector
        {
            get { return projector; }
        }
    }

    internal class DbExpressionVisitor : ExpressionVisitor
    {
        protected override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }
            switch ((DbExpressionType) exp.NodeType)
            {
                case DbExpressionType.Table:
                    return VisitTable((TableExpression) exp);
                case DbExpressionType.Column:
                    return VisitColumn((ColumnExpression) exp);
                case DbExpressionType.Select:
                    return VisitSelect((SelectExpression) exp);
                case DbExpressionType.Projection:
                    return VisitProjection((ProjectionExpression) exp);
                default:
                    return base.Visit(exp);
            }
        }

        protected virtual Expression VisitTable(TableExpression table)
        {
            return table;
        }

        protected virtual Expression VisitColumn(ColumnExpression column)
        {
            return column;
        }

        protected virtual Expression VisitSelect(SelectExpression select)
        {
            var from = VisitSource(select.From);
            var where = Visit(select.Where);
            var columns = VisitColumnDeclarations(select.Columns);
            if (from != select.From || where != select.Where || columns != select.Columns)
            {
                return new SelectExpression(select.Type, select.Alias, columns, from, where);
            }
            return select;
        }

        protected virtual Expression VisitSource(Expression source)
        {
            return Visit(source);
        }

        protected virtual Expression VisitProjection(ProjectionExpression proj)
        {
            var source = (SelectExpression) Visit(proj.Source);
            var projector = Visit(proj.Projector);
            if (source != proj.Source || projector != proj.Projector)
            {
                return new ProjectionExpression(source, projector);
            }
            return proj;
        }

        protected ReadOnlyCollection<ColumnDeclaration> VisitColumnDeclarations(
            ReadOnlyCollection<ColumnDeclaration> columns)
        {
            List<ColumnDeclaration> alternate = null;
            for (int i = 0, n = columns.Count; i < n; i++)
            {
                var column = columns[i];
                var e = Visit(column.Expression);
                if (alternate == null && e != column.Expression)
                {
                    alternate = columns.Take(i).ToList();
                }
                if (alternate != null)
                {
                    alternate.Add(new ColumnDeclaration(column.Name, e));
                }
            }
            if (alternate != null)
            {
                return alternate.AsReadOnly();
            }
            return columns;
        }
    }
}