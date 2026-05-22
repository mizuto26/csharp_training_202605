using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MSTest_WebApp.Tests.TestDoubles;

internal sealed class ThrowingDbSet<TEntity> : DbSet<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>
    where TEntity : class
{
    private readonly IQueryable<TEntity> _queryable = Array.Empty<TEntity>().AsQueryable();
    private readonly Exception _exception;

    public ThrowingDbSet()
        : this(new InvalidOperationException("Database access failed."))
    {
    }

    public ThrowingDbSet(Exception exception)
    {
        _exception = exception;
    }

    public override IEntityType EntityType => throw new NotSupportedException();

    Type IQueryable.ElementType => _queryable.ElementType;

    Expression IQueryable.Expression => _queryable.Expression;

    IQueryProvider IQueryable.Provider => new ThrowingQueryProvider(_exception);

    public override EntityEntry<TEntity> Add(TEntity entity)
    {
        throw _exception;
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
    {
        throw _exception;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw _exception;
    }

    private sealed class ThrowingQueryProvider : IQueryProvider
    {
        private readonly Exception _exception;

        public ThrowingQueryProvider(Exception exception)
        {
            _exception = exception;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw _exception;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            throw _exception;
        }

        public object? Execute(Expression expression)
        {
            throw _exception;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw _exception;
        }
    }
}
