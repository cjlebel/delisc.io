using Ardalis.GuardClauses;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Deliscio.Modules.Links.Data.Mongo;

/// <summary>
/// 
/// </summary>
public sealed class LinksRepository : MongoRepository<LinkEntity>, ILinksRepository
{
    #region - Constructors -
    public LinksRepository(IOptions<MongoDbOptions> options) : base(options) { }

    public LinksRepository(string connectionString, string databaseName) : base(connectionString, databaseName) { }

    public LinksRepository(MongoDbContext<LinkEntity> context) : base(context) { }
    #endregion

    #region - Links
    public async Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> GetByDomainAsync(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(domain);

        return await FindAsync(x => x.Domain == domain, pageNo, pageSize, token);
    }

    public async Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> GetByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        var arrTags = tags as string[] ?? Array.Empty<string>();

        if (!arrTags.Any())
            return (Enumerable.Empty<LinkEntity>(), 0, 0);

        var filter =
            Builders<LinkEntity>.Filter.All(bookmark => bookmark.Tags.Select(tag => tag.Name), arrTags); // & Builders<LinkEntity>.Filter.Eq(bookmark => bookmark.IsActive, true);

        return await FindAsync(filter, pageNo, pageSize, token);
    }

    #endregion


    #region - Tags -

    public async Task AddTag(Guid id, string tag, CancellationToken token)
    {
        var filter = Builders<LinkEntity>.Filter.Eq("_id", id);
        var update = Builders<LinkEntity>.Update.Inc($"Tags.{tag}", 1);

        var updateResult = await Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, cancellationToken: token);

        //!(updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)

    }

    public async Task<IEnumerable<TagEntity>> GetTags(Guid id, CancellationToken token)
    {
        //var filter = Builders<LinkEntity>.Filter.Eq("_id", id);

        //var rslt = await Collection.FindSync(filter, cancellationToken: token).SingleOrDefaultAsync(cancellationToken: token);

        //if (rslt == null)
        //{
        //    return Enumerable.Empty<TagEntity>();
        //}

        //return new rslt.Ta.;

        return Enumerable.Empty<TagEntity>();
    }

    /// <summary>
    /// Removes an occurrence of the tag from the Tag collection that belongs to Link with the id.<br />
    /// If the tag's count becomes zero, then it will be removed from the collection.
    /// </summary>
    /// <param name="id">The id of the Link to update.</param>
    /// <param name="tag">The tag in which its count will be incremented.</param>
    /// <param name="token">The token.</param>
    public async Task RemoveTag(Guid id, string tag, CancellationToken token)
    {
        var filter = Builders<LinkEntity>.Filter.Eq("_id", id);
        var update = Builders<LinkEntity>.Update.Combine(
            Builders<LinkEntity>.Update.Inc($"Tags.{tag}", -1), // Decrease the count of the specified tag
            Builders<LinkEntity>.Update.PullFilter("Tags",
                Builders<BsonDocument>.Filter.Eq(tag, 0)) // Remove the tag if its count becomes zero
        );

    }

    #endregion
}