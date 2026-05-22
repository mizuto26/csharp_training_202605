using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MSTest_WebApp.Tests.TestDoubles;

internal sealed class QueryableDbSet<TEntity>(IEnumerable<TEntity> entities) : DbSet<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>
    where TEntity : class
{
    private readonly List<TEntity> _entities = entities.ToList();

    public IReadOnlyList<TEntity> Entities => _entities;

    public override IEntityType EntityType => throw new NotSupportedException();

    private IQueryable<TEntity> Queryable => _entities.AsQueryable();

    Type IQueryable.ElementType => Queryable.ElementType;

    Expression IQueryable.Expression => Queryable.Expression;

    IQueryProvider IQueryable.Provider => Queryable.Provider;

    public override EntityEntry<TEntity> Add(TEntity entity)
    {
        _entities.Add(entity);
        return null!;
    }

    public override void AddRange(params TEntity[] entities)
    {
        _entities.AddRange(entities);
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
    {
        return Queryable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Queryable.GetEnumerator();
    }
}
