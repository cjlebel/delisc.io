using System.Linq.Expressions;

using Deliscio.Core.Data.Interfaces;

namespace Deliscio.Core.Data;

public abstract class BaseRepository<T> : BaseRepositoryWithTypedId<T, Guid>, IRepository<T>
    where T : class, IEntityWithTypedId<Guid>
{
    //public Repository(MsSqlDbContext context) : base(context) { }
}

/// <summary>
/// The idea is that this BaseRepository can be implemented to work with any type of database (eg: A BaseMsSqlRepository or a BaseMongoRepository).
/// Therefore, no implementation details are provided here.
/// </summary>
/// <typeparam name="T">The Type of the Entity</typeparam>
/// <typeparam name="TId">The Type of the Id that is used as the key for the Entity.</typeparam>
/// <seealso cref="Deliscio.Core.Data.Interfaces.IRepositoryWithTypedId&lt;T, TId&gt;" />
public abstract class BaseRepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId> where T : class, IEntityWithTypedId<TId>
{
    //public RepositoryWithTypedId(MsSqlDbContext context)
    //{
    //    Context = context;
    //    DbSet = Context.Set<T>();
    //}

    //protected DbContext Context { get; }

    //protected DbSet<T> DbSet { get; }
    public abstract void Add(T entity);

    public abstract Task AddAsync(T entity, CancellationToken token = default);

    public abstract void AddRange(IEnumerable<T> entities, CancellationToken token = default);

    public abstract Task AddRangeAsync(IEnumerable<T> entities, CancellationToken token = default);
    public abstract Task<(IEnumerable<T> Results, int TotalPages, int TotalCount)> FindAsync(Expression<Func<T, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    public abstract T? Get(TId id);

    public abstract IEnumerable<T> Get(IEnumerable<TId> ids);
    public abstract Task<T?> GetAsync(TId id, CancellationToken token = default);

    public abstract Task<IEnumerable<T>> GetAsync(IEnumerable<TId> ids, CancellationToken token = default);

    public abstract Task<(IEnumerable<T> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo, int pageSize, CancellationToken token = default);

    public abstract Task<T?> GetAsync(TId id);

    public abstract Task<IEnumerable<T>> GetAsync(IEnumerable<TId> ids);

    public abstract void Remove(TId id, CancellationToken token = default);

    public abstract void Remove(T entity, CancellationToken token = default);

    public abstract Task RemoveAsync(TId id, CancellationToken token = default);

    public abstract Task RemoveAsync(T entity, CancellationToken token = default);

    public abstract void RemoveRange(IEnumerable<TId> ids, CancellationToken token = default);

    public abstract void RemoveRange(IEnumerable<T> entities, CancellationToken token = default);

    public abstract Task RemoveRangeAsync(IEnumerable<TId> ids, CancellationToken token = default);

    public abstract Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken token = default);

    public abstract void Save();
    public abstract Task SaveAsync(CancellationToken token = default);

    public abstract void Update(T entity, CancellationToken token = default);

    public abstract Task UpdateAsync(T entity, CancellationToken token = default);
}