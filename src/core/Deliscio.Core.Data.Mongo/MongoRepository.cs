using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Deliscio.Core.Data.Interfaces;
using Deliscio.Core.Data.Mongo.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Structurizr.Annotations;

namespace Deliscio.Core.Data.Mongo;

[CodeElement("MongoRepository", Description = "Base Mongo Repository")]
public class MongoRepository<TDocument> : IRepository<TDocument> where TDocument : IEntityWithTypedId<Guid>
{
    private readonly IMongoDbContext<TDocument> _context;

    // Best to have your messages in a const for performance reasons
    private const string EXCEPTION_COLLECTION_NOT_FOUND = "The collection was not found";
    private const string EXCEPTION_ID_CANT_BE_EMPTY = "The Id(s) cannot be empty";

    protected IMongoCollection<TDocument> Collection;

    #region - Constructors -

    protected MongoRepository(IMongoDbClient client)
    {
        _context = new MongoDbContext<TDocument>(client);
        PopulateCollection();
    }

    protected MongoRepository(IOptions<MongoDbOptions> options)
    {
        _context = new MongoDbContext<TDocument>(options);
        PopulateCollection();
    }


    #endregion

    public void Add(TDocument entity)
    {
        Collection.InsertOne(entity);
    }

    public async Task AddAsync(TDocument entity, CancellationToken token = default)
    {
        await Collection.InsertOneAsync(entity, new InsertOneOptions(), token);
    }

    public void AddRange(IEnumerable<TDocument> entities, CancellationToken token = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        try
        {
            entities = entities.ToList();

            Collection.InsertMany(entities, cancellationToken: token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task AddRangeAsync(IEnumerable<TDocument> entities, CancellationToken token = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        entities = entities.ToList();

        if (entities.Any())
        {
            try
            {
                await Collection.InsertManyAsync(entities, new InsertManyOptions() { }, cancellationToken: token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    /// <summary>
    /// Finds a collection of records that match the predicate.
    /// </summary>
    /// <param name="predicate">The query to use to filter by</param>
    /// <param name="pageNo">The page number of the results to return</param>
    /// <param name="pageSize">The size of the page</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>An IEnumerable of TDocument</returns>
    public async Task<(IEnumerable<TDocument> Results, int TotalPages, int TotalCount)> FindAsync(Expression<Func<TDocument, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        var filter = Builders<TDocument>.Filter.Where(predicate);

        return await FindAsync(filter, pageNo, pageSize, token);
    }

    protected async Task<(IEnumerable<TDocument> Results, int TotalPages, int TotalCount)> FindAsync(FilterDefinition<TDocument> filter, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.Null(filter);
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var skip = (pageNo - 1) * pageSize;

        var totalCount = await Collection.CountDocumentsAsync(filter, null, token);

        if (totalCount == 0)
            return (Enumerable.Empty<TDocument>(), 0, 0);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var cursor = Collection.Find(filter)
            .Skip(skip)
            .Limit(pageSize)
            .Sort(Builders<TDocument>.Sort.Ascending(doc => doc.Id));

        var results = await cursor.ToListAsync(token);

        return (results, totalPages, (int)totalCount);
    }

    /// <summary>
    /// Gets an individual record by its unique id.
    /// </summary>
    /// <param name="id">The id of the record to be returned.</param>
    /// <returns>An individual TDocument</returns>
    /// <exception cref="System.ArgumentException">id</exception>
    public TDocument? Get(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(EXCEPTION_ID_CANT_BE_EMPTY, nameof(id));
        }

        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);

        return Collection.Find(filter, new FindOptions() { }).FirstOrDefault();
    }

    /// <summary>
    /// Gets a collection of records by their unique ids.
    /// </summary>
    /// <param name="ids">The collection of ids of the records to be returned.</param>
    /// <returns>An IEnumerable of TDocument</returns>
    /// <exception cref="System.ArgumentException">ids</exception>
    public IEnumerable<TDocument> Get(IEnumerable<Guid> ids)
    {
        if (!ids.TryGetNonEnumeratedCount(out var count) || count == 0)
            throw new ArgumentException(EXCEPTION_ID_CANT_BE_EMPTY, nameof(ids));

        var filter = Builders<TDocument>.Filter.In("_id", ids.ToArray());

        var cursor = Collection.Find(filter, null);

        return cursor.ToList() ?? Enumerable.Empty<TDocument>();
    }


    /// <summary>
    /// Gets an individual record by its unique id - asynchronously.
    /// </summary>
    /// <param name="id">The id of the record to be returned.</param>
    /// <param name="token"></param>
    /// <returns>An individual TDocument</returns>
    /// <exception cref="System.ArgumentException">id</exception>
    public async Task<TDocument?> GetAsync(Guid id, CancellationToken token = default)
    {
        if (id == Guid.Empty)
        {
            return await Task.FromException<TDocument>(new ArgumentException(EXCEPTION_ID_CANT_BE_EMPTY, nameof(id)));
        }

        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
        return await Collection.Find(filter)
            .SingleOrDefaultAsync(token);
    }

    /// <summary>
    /// Gets a collection of records by their unique ids - asynchronously.
    /// </summary>
    /// <param name="ids">The collection of ids of the records to be returned.</param>
    /// <param name="token"></param>
    /// <returns>An IEnumerable of TDocument</returns>
    /// <exception cref="System.ArgumentException">ids</exception>
    public async Task<IEnumerable<TDocument>> GetAsync(IEnumerable<Guid> ids, CancellationToken token = default)
    {
        if (!ids.TryGetNonEnumeratedCount(out var count) || count == 0)
            return await Task.FromException<IEnumerable<TDocument>>(new ArgumentException(EXCEPTION_ID_CANT_BE_EMPTY, nameof(ids)));

        var filter = Builders<TDocument>.Filter.In("_id", ids.ToArray());

        var cursor = await Collection.FindAsync(filter, null, token);

        return await cursor.ToListAsync(token) ?? Enumerable.Empty<TDocument>();
    }


    //TODO: public async Task<IEnumerable<TDocument>> GetAsync(int pageNo, int pageSize, string sortBy,       /* The field to sort by */ SortDirection sortDirection, /* The sort direction (ascending or descending) */ CancellationToken cancellationToken = default)
    public async Task<(IEnumerable<TDocument> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo, int pageSize, CancellationToken token = default)
    {
        return await FindAsync(_ => true, pageNo, pageSize, token);
    }

    public async Task<TDocument?> FirstOrDefault(Expression<Func<TDocument, bool>> predicate, CancellationToken token = default)
    {
        var filter = Builders<TDocument>.Filter.Where(predicate);

        var cursor = await Collection.Find(filter)
            .FirstOrDefaultAsync(cancellationToken: token);
        //.Sort(Builders<TDocument>.Sort.Ascending(doc => doc.Id));

        return cursor;
    }

    public void Remove(Guid id, CancellationToken token = default)
    {
        Collection.DeleteOne(d => d.Id == id, cancellationToken: token);
    }

    public void Remove(TDocument entity, CancellationToken token = default)
    {
        Collection.DeleteOne(d => d.Id == entity.Id, cancellationToken: token);
    }


    public async Task RemoveAsync(Guid id, CancellationToken token = default)
    {
        await Collection.DeleteOneAsync(d => d.Id == id, token);
    }

    public async Task RemoveAsync(TDocument entity, CancellationToken token = default)
    {
        await Collection.DeleteOneAsync(d => d.Id == entity.Id, token);
    }


    public void RemoveRange(IEnumerable<Guid> ids, CancellationToken token = default)
    {
        var filter = Builders<TDocument>.Filter.In(doc => doc.Id, ids);

        Collection.DeleteMany(filter, cancellationToken: token);
    }

    public void RemoveRange(IEnumerable<TDocument> entities, CancellationToken token = default)
    {
        var ids = entities.Select(x => x.Id).AsEnumerable();

        var filter = Builders<TDocument>.Filter.In(doc => doc.Id, ids);

        Collection.DeleteMany(filter, cancellationToken: token);
    }


    public Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken token = default)
    {
        var filter = Builders<TDocument>.Filter.In(doc => doc.Id, ids);

        return Collection.DeleteManyAsync(filter, cancellationToken: token);
    }

    public Task RemoveRangeAsync(IEnumerable<TDocument> entities, CancellationToken token = default)
    {
        var ids = entities.Select(x => x.Id).AsEnumerable();

        var filter = Builders<TDocument>.Filter.In(doc => doc.Id, ids);

        return Collection.DeleteManyAsync(filter, cancellationToken: token);
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }


    public void Update(TDocument entity, CancellationToken token = default)
    {
        entity.DateUpdated = DateTimeOffset.UtcNow;
        Collection.ReplaceOne(d => d.Id == entity.Id, entity, cancellationToken: token);
    }

    public async Task UpdateAsync(TDocument entity, CancellationToken token = default)
    {
        entity.DateUpdated = DateTime.UtcNow;
        await Collection.ReplaceOneAsync(d => d.Id == entity.Id, entity, cancellationToken: token);
    }

    #region - Privates -

    private void PopulateCollection()
    {
        Collection = _context.Collection() ?? throw new InvalidOperationException(EXCEPTION_COLLECTION_NOT_FOUND);
    }




    #endregion

}
