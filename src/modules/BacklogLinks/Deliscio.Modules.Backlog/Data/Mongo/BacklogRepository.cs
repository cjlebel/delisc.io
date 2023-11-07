using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Interfaces;
using Deliscio.Modules.BackLog.Data.Entities;
using Deliscio.Modules.BackLog.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace Deliscio.Modules.BackLog.Data.Mongo;

internal sealed class BacklogRepository : MongoRepository<BacklogItemEntity>, IBacklogRepository
{
    #region - Constructors -

    public BacklogRepository(IOptions<MongoDbOptions> options) : base(options) { }

    //public BacklogRepository(IMongoDbClient client) : base(client) { }

    #endregion

    public async ValueTask<Guid> AddAsync(string url, string title, string createById, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        if (string.IsNullOrWhiteSpace(createById))
            throw new ArgumentNullException(nameof(createById));

        var existingId = await Exists(url, token);

        if (existingId != Guid.Empty)
            return existingId;

        var entity = BacklogItemEntity.Create(url, title, createById);

        try
        {
            await Collection.InsertOneAsync(entity, new InsertOneOptions(), token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            //TODO: Log and continue

            return Guid.Empty;
        }

        return entity.Id;
    }

    //public async ValueTask<IEnumerable<Guid>> AddRangeAsync(IEnumerable<BacklogItemEntity> entities, CancellationToken token)
    //{
    //    if (entities == null)
    //        throw new ArgumentNullException(nameof(entities));

    //    try
    //    {
    //        entities = entities.ToList();

    //        await Collection.InsertManyAsync(entities, cancellationToken: token);

    //        var ids = entities.Select(x => x.Id).AsEnumerable();

    //        return ids;
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e);
    //        throw;
    //    }
    //}

    public async ValueTask<bool> AddOrUpdateAsync(string url, string title, string createdByUserId,
        CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        if (string.IsNullOrWhiteSpace(createdByUserId))
            throw new ArgumentNullException(nameof(createdByUserId));

        var entity = new BacklogItemEntity(url, title, createdByUserId);

        return await AddOrUpdateAsync(entity, token);
    }

    private async ValueTask<bool> AddOrUpdateAsync(BacklogItemEntity entity, CancellationToken token = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var builder = Builders<BacklogItemEntity>.Filter;

        // If this entity url and created by already exist, then update it.
        var filterDefinition = new[]
        {
            builder.Eq(doc => doc.Url, entity.Url), builder.Eq(doc => doc.CreatedById, entity.CreatedById)
        };

        var filter = builder.And(filterDefinition);

        var options = new ReplaceOptions { IsUpsert = true };

        ReplaceOneResult? result;

        try
        {
            result = await Collection.ReplaceOneAsync(filter, entity, options, token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            //TODO: Log and continue

            return false;
        }

        return result.IsAcknowledged;
    }

    public async ValueTask<Guid> Exists(string url, CancellationToken token)
    {
        var builder = Builders<BacklogItemEntity>.Filter;

        // If this entity url and created by already exist, then update it.
        var filterDefinition = new[]
        {
            builder.Eq(doc => doc.Url, url)
        };

        var filter = builder.And(filterDefinition);

        var entity = (await Collection.FindAsync(filter, new FindOptions<BacklogItemEntity>() { Limit = 1 }, token)).Single(CancellationToken.None);

        return entity?.Id ?? Guid.Empty;
    }


    public async ValueTask<(IEnumerable<BacklogItemEntity> Items, int TotalCount)> GetAsync(int page, int size,
        bool? isProcessed = false)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAllAsync(CancellationToken token = default)
    {
        var builder = Builders<BacklogItemEntity>.Filter;

        // If this entity url and created by already exist, then update it.
        var filterDefinition = new[]
        {
            builder.Eq(doc => doc.IsProcessed, false)
        };

        await Collection.DeleteManyAsync(builder.And(filterDefinition), token);
    }
}