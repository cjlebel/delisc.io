using Deliscio.Core.Data.MsSql.Interfaces;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Deliscio.Core.Data.Interfaces;
using System.Linq.Expressions;

namespace Deliscio.Core.Data.MsSql;

public class MsSqlRepository<T> : MsSqlRepositoryWithTypedId<T, Guid>, IRepository<T>
    where T : class, IEntityWithTypedId<Guid>
{
    public MsSqlRepository(MsSqlDbContext context) : base(context) { }
}

public class MsSqlRepositoryWithTypedId<T, TId> : BaseRepositoryWithTypedId<T, TId>, IRepositoryWithTypedId<T, TId> where T : class, IEntityWithTypedId<TId>
{
    public MsSqlRepositoryWithTypedId(MsSqlDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    protected DbContext Context { get; }

    protected DbSet<T> DbSet { get; }

    public override void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public override Task AddAsync(T entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public override void AddRange(IEnumerable<T> entity, CancellationToken token = default)
    {
        DbSet.AddRange(entity);
    }

    public override Task AddRangeAsync(IEnumerable<T> entities, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }

    public override Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
    public override T? Get(TId id)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<T> Get(IEnumerable<TId> ids)
    {
        throw new NotImplementedException();
    }

    public override Task<T?> GetAsync(TId id)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<T>> GetAsync(IEnumerable<TId> ids)
    {
        throw new NotImplementedException();
    }

    public override void Remove(T entity, CancellationToken token = default)
    {
        DbSet.Remove(entity);
    }

    public override Task RemoveAsync(T entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public override void Save()
    {
        Context.SaveChanges();
    }

    public override Task SaveAsync()
    {
        return Context.SaveChangesAsync();
    }
    public override void Update(T entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(T entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}