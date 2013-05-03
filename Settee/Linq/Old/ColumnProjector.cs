using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Biseth.Net.Settee.Linq.Old
{
    internal sealed class ProjectedColumns
    {
        private ReadOnlyCollection<ColumnDeclaration> columns;
        private Expression projector;

        internal ProjectedColumns(Expression projector, ReadOnlyCollection<ColumnDeclaration> columns)
        {
            this.projector = projector;
            this.columns = columns;
        }

        internal Expression Projector
        {
            get { return projector; }
        }

        internal ReadOnlyCollection<ColumnDeclaration> Columns
        {
            get { return columns; }
        }
    }

    internal class ColumnProjector : DbExpressionVisitor
    {
        private HashSet<Expression> candidates;
        private HashSet<string> columnNames;
        private List<ColumnDeclaration> columns;
        private string existingAlias;
        private int iColumn;
        private Dictionary<ColumnExpression, ColumnExpression> map;
        private string newAlias;
        private Nominator nominator;

        internal ColumnProjector(Func<Expression, bool> fnCanBeColumn)
        {
            nominator = new Nominator(fnCanBeColumn);
        }

        internal ProjectedColumns ProjectColumns(Expression expression, string newAlias, string existingAlias)
        {
            map = new Dictionary<ColumnExpression, ColumnExpression>();
            columns = new List<ColumnDeclaration>();
            columnNames = new HashSet<string>();
            this.newAlias = newAlias;
            this.existingAlias = existingAlias;
            candidates = nominator.Nominate(expression);
            return new ProjectedColumns(Visit(expression), columns.AsReadOnly());
        }

        protected override Expression Visit(Expression expression)
        {
            if (candidates.Contains(expression))
            {
                if (expression.NodeType == (ExpressionType) DbExpressionType.Column)
                {
                    var column = (ColumnExpression) expression;
                    ColumnExpression mapped;
                    if (map.TryGetValue(column, out mapped))
                    {
                        return mapped;
                    }
                    if (existingAlias == column.Alias)
                    {
                        var ordinal = columns.Count;
                        var columnName = GetUniqueColumnName(column.Name);
                        columns.Add(new ColumnDeclaration(columnName, column));
                        mapped = new ColumnExpression(column.Type, newAlias, columnName, ordinal);
                        map[column] = mapped;
                        columnNames.Add(columnName);
                        return mapped;
                    }
                    // must be referring to outer scope
                    return column;
                }
                else
                {
                    var columnName = GetNextColumnName();
                    var ordinal = columns.Count;
                    columns.Add(new ColumnDeclaration(columnName, expression));
                    return new ColumnExpression(expression.Type, newAlias, columnName, ordinal);
                }
            }
            else
            {
                return base.Visit(expression);
            }
        }

        private bool IsColumnNameInUse(string name)
        {
            return columnNames.Contains(name);
        }

        private string GetUniqueColumnName(string name)
        {
            var baseName = name;
            var suffix = 1;
            while (IsColumnNameInUse(name))
            {
                name = baseName + (suffix++);
            }
            return name;
        }

        private string GetNextColumnName()
        {
            return GetUniqueColumnName("c" + (iColumn++));
        }

        private class Nominator : DbExpressionVisitor
        {
            private HashSet<Expression> candidates;
            private Func<Expression, bool> fnCanBeColumn;
            private bool isBlocked;

            internal Nominator(Func<Expression, bool> fnCanBeColumn)
            {
                this.fnCanBeColumn = fnCanBeColumn;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                candidates = new HashSet<Expression>();
                isBlocked = false;
                Visit(expression);
                return candidates;
            }

            protected override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    var saveIsBlocked = isBlocked;
                    isBlocked = false;
                    base.Visit(expression);
                    if (!isBlocked)
                    {
                        if (fnCanBeColumn(expression))
                        {
                            candidates.Add(expression);
                        }
                        else
                        {
                            isBlocked = true;
                        }
                    }
                    isBlocked |= saveIsBlocked;
                }
                return expression;
            }
        }
    }
}