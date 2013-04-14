using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace Biseth.Net.Settee.Linq
{
    internal class ProjectionBuilder : DbExpressionVisitor
    {
        private static MethodInfo miGetValue;
        private ParameterExpression row;

        internal ProjectionBuilder()
        {
            if (miGetValue == null)
            {
                miGetValue = typeof (ProjectionRow).GetMethod("GetValue");
            }
        }

        internal LambdaExpression Build(Expression expression)
        {
            row = Expression.Parameter(typeof (ProjectionRow), "row");
            var body = Visit(expression);
            return Expression.Lambda(body, row);
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            return Expression.Convert(Expression.Call(row, miGetValue, Expression.Constant(column.Ordinal)), column.Type);
        }
    }

    public abstract class ProjectionRow
    {
        public abstract object GetValue(int index);
    }

    internal class ProjectionReader<T> : IEnumerable<T>, IEnumerable
    {
        private Enumerator enumerator;

        internal ProjectionReader(DbDataReader reader, Func<ProjectionRow, T> projector)
        {
            enumerator = new Enumerator(reader, projector);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var e = enumerator;
            if (e == null)
            {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }
            enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : ProjectionRow, IEnumerator<T>, IEnumerator, IDisposable
        {
            private T current;
            private Func<ProjectionRow, T> projector;
            private DbDataReader reader;

            internal Enumerator(DbDataReader reader, Func<ProjectionRow, T> projector)
            {
                this.reader = reader;
                this.projector = projector;
            }

            public T Current
            {
                get { return current; }
            }

            object IEnumerator.Current
            {
                get { return current; }
            }

            public bool MoveNext()
            {
                if (reader.Read())
                {
                    current = projector(this);
                    return true;
                }
                return false;
            }

            public void Reset()
            {
            }

            public void Dispose()
            {
                reader.Dispose();
            }

            public override object GetValue(int index)
            {
                if (index >= 0)
                {
                    if (reader.IsDBNull(index))
                    {
                        return null;
                    }
                    else
                    {
                        return reader.GetValue(index);
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }
    }
}