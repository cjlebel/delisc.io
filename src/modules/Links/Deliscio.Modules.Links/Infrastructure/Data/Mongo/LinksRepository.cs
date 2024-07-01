using System.Collections.Immutable;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Domain.LinkTags;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Deliscio.Modules.Links.Infrastructure.Data.Mongo;

/// <summary>
/// Responsible for interacting with the MongoDb database for the Links module
/// </summary>
public sealed class LinksRepository : MongoRepository<LinkEntity>, ILinksRepository
{
    private readonly IMediator _mediator;

    private const string ERROR_INVALID_LINK_ID = "The link id is invalid";
    private const string ERROR_INVALID_LINK_URL = "The url is invalid";
    private const string ERROR_INVALID_USER_ID = "The user id is null or empty";

    private const string ERROR_LINK_NOT_FOUND_FOR_ID = "Link not found for id {0}";
    private const string ERROR_LINK_NOT_FOUND_FOR_URL = "Link not found for url {0}";

    private const string ERROR_PAGE_NUMBER = "Page number must be greater than 0";
    private const string ERROR_PAGE_SIZE = "Page size must be greater than 0";

    #region - Constructors -

    public LinksRepository(IOptions<MongoDbOptions> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    //public LinksRepository(IMongoDbClient client) : base(client) { }
    #endregion

    #region - Links

    public async Task<Result<(IReadOnlyList<Link> Results, int TotalPages, int TotalCount)>> FindLinksAsync(string term, string[] tags, string domain,
        int pageNo, int pageSize, int offset = 0,
        bool? isActive = default, bool? isFlagged = default, bool? isDeleted = false,
        CancellationToken token = default)
    {
        if (pageNo < 1)
            return Result.Fail(ERROR_PAGE_NUMBER);

        if (pageSize < 1)
            return Result.Fail(ERROR_PAGE_SIZE);

        var x = isDeleted == default;

        //var filter = Builders<LinkEntity>.Filter.Where(
        //        l => (string.IsNullOrWhiteSpace(term) || l.Title.Contains(term, StringComparison.InvariantCultureIgnoreCase)) &&
        //             (string.IsNullOrWhiteSpace(domain) || l.Domain.ToLowerInvariant() == domain.ToLowerInvariant()) &&
        //             (isDeleted == default || l.IsDeleted == isDeleted.Value)
        //       //(isActive == default || l.IsActive == isActive.Value) &&
        //       //(isFlagged == default || l.IsFlagged == isFlagged.Value) &&
        //       //(isDeleted == default || (l.IsDeleted == isDeleted.Value))
        //       );
        ////.All(link => link.TagsCollection.Select(tag => tag.Name), arrTags)

        var filter = Builders<LinkEntity>.Filter.Where(
            l => (string.IsNullOrWhiteSpace(term) || l.Title.ToUpperInvariant().Contains(term.ToUpperInvariant()))
                 && (string.IsNullOrWhiteSpace(domain) || l.Domain.ToUpperInvariant().Contains(domain.ToUpperInvariant()))
                 && (isActive == null || l.IsActive == isActive.Value)
                 //&& (isFlagged == default || l.IsFlagged == isFlagged.Value)
                 && (isDeleted == null || l.IsDeleted == isDeleted.Value)
        //&& (isActive == default || l.IsActive == isActive.Value)
        ////&& (isFlagged == default || l.IsFlagged == isFlagged.Value)
        //&& (isDeleted == default || l.IsDeleted == isDeleted.Value)
        //&&
        //(string.IsNullOrWhiteSpace(domain) || l.Domain.ToUpperInvariant().Contains(domain.ToUpperInvariant())) &&
        //(isActive == default || l.IsActive == isActive.Value) &&
        //(isFlagged == default || l.IsFlagged == isFlagged.Value) &&
        //(isDeleted == default || l.IsDeleted == isDeleted.Value)
        );

        var rslts = await FindAsync(filter, pageNo, pageSize, token);

        return (rslts.Results.Select(r => Link.Map(r)).ToImmutableList(), rslts.TotalPages, rslts.TotalCount);
    }

    /// <summary>
    /// Gets an individual link by its id
    /// </summary>
    /// <param name="linkId">The id of the link to retrieve</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>Domain.Links.Link</returns>
    public async Task<Result<Link>> GetLinkByIdAsync(string linkId, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(linkId) || !ObjectId.TryParse(linkId, out _))
            return Result.Fail(ERROR_INVALID_LINK_ID);

        var rslt = await GetAsync(linkId.ToObjectId(), token);

        if (rslt is null)
            return Result.Fail(string.Format(ERROR_LINK_NOT_FOUND_FOR_ID, linkId));

        var link = Link.Map(rslt);

        return Result.Ok(link);
    }

    /// <summary>
    /// Gets an individual link by its unique url
    /// </summary>
    /// <param name="url">The unique url of the link to retrieve</param>
    /// <param name="token">The cancellation token</param>
    /// <remarks>If more than one link is found for the Url, then a Result.Fail is returned with the list of offending ids</remarks>
    /// <returns>Domain.Links.Link</returns>
    public async Task<Result<Link>> GetLinkByUrlAsync(string url, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            return Result.Fail(ERROR_INVALID_LINK_URL);

        // Purposely filtering for a collection instead of a single entity, in case there are more than one result for the url (which should not happen)
        var rslts = await FindAsync(x => x.Url == url, token: token);

        if (!rslts.Results.Any())
            return Result.Fail(string.Format(ERROR_LINK_NOT_FOUND_FOR_URL, url));

        if (rslts.Results.TryGetNonEnumeratedCount(out var rsltCount) && rsltCount > 1)
        {
            const string ERR_MESSAGE = "More than one link with the url '{0}' were found ({1})";
            var badIds = string.Join(',', rslts.Results.Select(r => r.Id).ToArray());

            return Result.Fail(string.Format(ERR_MESSAGE, url, badIds));
        }

        var rslt = rslts.Results.First();

        var link = Link.Map(rslt);

        return Result.Ok(link);
    }

    /// <summary>
    /// Gets all links that a specific user has created.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<Result<IEnumerable<Link>>> GetLinksByUserAsync(string userId, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail(ERROR_INVALID_USER_ID);

        var newUserId = userId.ToObjectId();

        var rslts = await FindAsync(l => l.CreatedById.Equals(newUserId), token: token);

        var links = rslts.Results.Select(r => Link.Map(r));

        return Result.Ok(links);
    }

    /// <summary>
    /// Gets a collection of tags that are related to the tags that were specified.
    /// The tags that are returned are the same tags that are in all of the Links that would be found with the GetLinksByTagsAsync(), but without including the paging.
    /// </summary>
    /// <param name="tags">The tags to use to get all other related tags</param>
    /// <param name="count">The max number of tags to return</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IEnumerable<LinkTagEntity>> GetRelatedTagsAsync(string[] tags, int count, CancellationToken token = default)
    {
        if (count < 1)
            return Enumerable.Empty<LinkTagEntity>();

        // Because we're going to exclude the incoming tags from the results
        var tmpCount = count + tags.Length;

        var newTags = tags.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim().ToLower()).ToArray();

        // If tags.Length is 0, get all tags. Else, get only those that are related to the specified tags
        BsonDocument match = newTags.Length == 0 ? new BsonDocument("$match", new BsonDocument()) :
            new BsonDocument("$match", new BsonDocument("TagsCollection.Name", new BsonDocument("$all", new BsonArray(tags))));

        var pipeline = new BsonDocument[]
        {
            match,
            // Unwind the tags array to create a separate document for each tag
            new BsonDocument("$unwind", "$TagsCollection"),
            // Group the tags and count the occurrences of each tag
            new BsonDocument("$group",
                new BsonDocument
                {
                    { "_id", "$TagsCollection.Name" },
                    { "count", new BsonDocument("$sum", 1) }
                }
            ),
            // Project the result to include only the tag name and count
            new BsonDocument("$project",
                new BsonDocument
                {
                    { "_id", 0 },
                    { "TagName", "$_id" },
                    { "Count", "$count" }
                }
            )
        };

        // Execute the aggregation pipeline
        var cursor = await Collection.AggregateAsync<BsonDocument>(pipeline, cancellationToken: token);

        //var relatedTags = cursor?
        //    .ToList(token)?
        //    .OrderByDescending(t => t["Count"].AsInt32)
        //    //TODO: Filter out here. Wasn't able to get it to work
        //    //.Where(x => !tags.Contains(x["Name"].AsString))
        //    .Take(tmpCount)
        //    .Select(x => new LinkTagEntity(x["TagName"].AsString, x["Count"].AsInt32)).ToArray()
        //                  ?? Array.Empty<LinkTagEntity>();

        var relatedTags = cursor?
            .ToList(token)?
            .Select(t =>
                new LinkTagEntity(t["TagName"].AsString, t["Count"].AsInt32, t["Weight"].AsDecimal)).ToArray() ??
                          Array.Empty<LinkTagEntity>();

        if (!relatedTags.Any())
            return Enumerable.Empty<LinkTagEntity>();

        //TODO: Re-look at this. Incoming tags should be part of the calculations, but excluded from the results
        // Strip out the tags we used to get the related tags (we only want related)
        //relatedTags = relatedTags.Where(x => !tags.Contains(x.Name)).Take(count).ToArray();

        //var totalCounts = relatedTags.Sum(x => x.Count);
        //var totalCounts = relatedTags.Count();

        //foreach (var relatedTag in relatedTags)
        //{
        //    relatedTag.Weight = totalCounts > 0f ? (relatedTag.Count / (float)totalCounts) : 0f;
        //}

        return relatedTags.Where(x => !tags.Contains(x.Name)).OrderBy(t => t.Name);
    }

    public Task<IEnumerable<LinkTagEntity>> GetRelatedTagsByDomainAsync(string domain, int count, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    //public async Task<Result> DeleteLinkAsync(string linkId, string userId, CancellationToken token = default)
    //{
    //    if (string.IsNullOrWhiteSpace(linkId) || !ObjectId.TryParse(linkId, out _))
    //        return Result.Fail(ERROR_INVALID_LINK_ID);

    //    if (string.IsNullOrWhiteSpace(userId))
    //        return Result.Fail(ERROR_INVALID_USER_ID);

    //    var linkEntity = await GetAsync(linkId.ToObjectId(), token);

    //    if (linkEntity is null)
    //        return Result.Fail(string.Format(ERROR_LINK_NOT_FOUND_FOR_ID, linkId));

    //    linkEntity.IsDeleted = true;
    //    linkEntity.DeletedById = userId.ToObjectId();
    //    linkEntity.DateDeleted = DateTime.UtcNow;

    //    linkEntity.UpdatedById = userId.ToObjectId();
    //    linkEntity.DateUpdated = DateTime.UtcNow;

    //    var updateCount = await UpdateAsync(linkEntity, token);

    //    if (updateCount == 0)
    //    {
    //        // Log
    //        return Result.Fail("Link could not be deleted");
    //    }

    //    if (updateCount > 1)
    //    {
    //        // Log
    //        return Result.Fail("More than one link was deleted. This is embarrassing.");
    //    }

    //    return Result.Ok();
    //}

    //public async Task<Result<string[]>> DeleteLinksAsync(List<Link> links, CancellationToken token = default)
    //{
    //    if (links is null || links.Count == 0)
    //        return Result.Fail("No links were provided");

    //    var userObjectId = userId.ToObjectId();
    //    var objectIds = linkIds.Select(v => v.ToObjectId()).ToArray();

    //    var linkEntities = await GetAsync(objectIds, token);

    //    var dateDeleted = DateTimeOffset.UtcNow;

    //    var linkIdsDeleted = new List<string>();

    //    foreach (var linkEntity in linkEntities)
    //    {
    //        linkEntity.IsDeleted = true;
    //        linkEntity.DeletedById = userObjectId;
    //        linkEntity.DateDeleted = dateDeleted;

    //        linkEntity.UpdatedById = userObjectId;
    //        linkEntity.DateUpdated = dateDeleted;

    //        var updateCount = await UpdateAsync(linkEntity, token);

    //        if (updateCount == 1)
    //        {
    //            linkIdsDeleted.Add(linkEntity.Id.ToString());
    //        }
    //    }

    //    return Result.Ok(linkIdsDeleted.ToArray());
    //}


    public async Task<Result> UpdateLinkAsync(Link link, string updatedByUserId, CancellationToken token = default)
    {
        var linkId = link.Id.Value;

        var linkEntity = await GetAsync(linkId, token);

        if (linkEntity is null)
            return Result.Fail(string.Format(ERROR_LINK_NOT_FOUND_FOR_ID, linkId));

        linkEntity.Title = link.Title.Value;
        linkEntity.Description = link.Description.Value;
        linkEntity.Domain = link.Domain.Value;
        linkEntity.ImageUrl = link.ImageUrl.Value;
        linkEntity.Keywords = link.Keywords.ToArray();
        linkEntity.Tags = link.TagsCollection.Select(t => new LinkTagEntity(t.Name, t.Count, t.Weight)).ToArray();
        linkEntity.Url = link.Url.Value;

        linkEntity.IsActive = link.IsActive;
        linkEntity.IsDeleted = link.IsDeleted;
        linkEntity.IsFlagged = link.IsFlagged;

        linkEntity.SavesCount = link.SavesCount;
        linkEntity.LikesCount = link.LikesCount;

        linkEntity.UpdatedById = link.UpdatedById.Value;
        linkEntity.DateUpdated = link.DateUpdated;

        linkEntity.DeletedById = link.DeletedById.Value;
        linkEntity.DateDeleted = link.DateDeleted;

        var updateCount = await UpdateAsync(linkEntity, token);

        if (updateCount == 0)
        {
            // Log
            return Result.Fail("Link could not be updated");
        }

        if (updateCount > 1)
        {
            // Log
            return Result.Fail("More than one link was updated. This is embarrassing.");
        }

        return Result.Ok();
    }

    #endregion

    #region - TagsCollection -

    //public async Task AddTagAsync(Guid linkId, string tag, CancellationToken token)
    //{
    //    var filter = Builders<LinkEntity>.Filter.Eq("_id", linkId);
    //    var update = Builders<LinkEntity>.Update.Inc($"TagsCollection.{tag}", 1);

    //    var updateResult = await Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true }, cancellationToken: token);

    //    //!(updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)

    //}

    public async Task<Result<IEnumerable<LinkTag>>> GetTagsAsync(string linkId, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(linkId) || !ObjectId.TryParse(linkId, out _))
            return Result.Fail(ERROR_INVALID_LINK_ID);

        var linkEntity = await GetAsync(linkId.ToObjectId(), token);

        if (linkEntity is null)
            return Result.Fail(string.Format(ERROR_LINK_NOT_FOUND_FOR_ID, linkId));

        var tags = linkEntity.Tags.Select(t => LinkTag.Create(t.Name, t.Count, t.Weight)).ToArray();

        return tags;
    }

    ///// <summary>
    ///// Removes an occurrence of the tag from the Tag collection that belongs to Link with the id.<br />
    ///// If the tag's count becomes zero, then it will be removed from the collection.
    ///// </summary>
    ///// <param name="linkId">The id of the Link to update.</param>
    ///// <param name="tag">The tag in which its count will be incremented.</param>
    ///// <param name="token">The token.</param>
    //public async Task RemoveTagAsync(Guid linkId, string tag, CancellationToken token)
    //{
    //    var filter = Builders<LinkEntity>.Filter.Eq("_id", linkId);
    //    var update = Builders<LinkEntity>.Update.Combine(
    //        Builders<LinkEntity>.Update.Inc($"TagsCollection.{tag}", -1), // Decrease the count of the specified tag
    //        Builders<LinkEntity>.Update.PullFilter("TagsCollection",
    //            Builders<BsonDocument>.Filter.Eq(tag, 0)) // Remove the tag if its count becomes zero
    //    );

    //    await Collection.UpdateOneAsync(filter, update, cancellationToken: token);
    //}

    #endregion

    private async Task DispatchDomainEventsAsync(Link link, CancellationToken token = default)
    {
        var domainEvents = link.DomainEvents.ToList();

        link.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, token);
        }
    }
}