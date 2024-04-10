using System.Linq.Expressions;
using System.Net;
using Ardalis.GuardClauses;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Models.Requests;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Mappers;

using Microsoft.Extensions.Logging;
using Structurizr.Annotations;

namespace Deliscio.Modules.Links;

[Component(Description = "Deliscio service that deals with the central Links", Technology = "C#")]
[UsedBySoftwareSystem("Deliscio.Apis.WebApi", Description = "Links Service")]
public sealed class LinksService : LinksBaseService<LinksService>, ILinksService
{
    public LinksService(ILinksRepository linksRepository, ILogger<LinksService> logger) : base(linksRepository, logger) { }


    //public LinksService(MongoDbClient client, ILogger<LinksService> logger)
    //{
    //    Guard.Against.Null(client);
    //    Guard.Against.Null(logger);

    //    LinksRepository = new LinksRepository(client);
    //    Logger = logger;
    //}

    //public LinksService(IOptions<MongoDbOptions> options, ILogger<LinksService> logger)
    //{
    //    Guard.Against.Null(options);
    //    Guard.Against.Null(logger);

    //    LinksRepository = new LinksRepository(options);
    //    Logger = logger;
    //}

    public Task<PagedResults<LinkItem>> FindAsync(FindLinksRequest request, CancellationToken token = default)
    {
        return base.FindLinksAsync(request.SearchTerm, request.Tags, request.Domain, request.PageNo, request.PageSize, request.Skip, request.IsActive, request.IsFlagged, request.IsDeleted, token);
    }

    /// <summary>
    /// Gets a single link by its id from the central link repository
    /// </summary>
    /// <param name="linkId">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override async Task<Link?> GetAsync(string linkId, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(linkId);

        return await base.GetAsync(new Guid(linkId), token);
    }

    /// <summary>
    /// Gets a single link by its id from the central link repository
    /// </summary>
    /// <param name="linkId">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override Task<Link?> GetAsync(Guid linkId, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(linkId);

        return base.GetAsync(linkId, token);
    }

    /// <summary>
    /// Gets a collection of links from the central link repository.
    /// </summary>
    /// <param name="pageNo">The number of the page of results to be returned</param>
    /// <param name="pageSize">The number of results per page</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public override Task<PagedResults<LinkItem>> GetAsync(int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        var newPageSize = pageSize ?? DEFAULT_LINKS_PAGE_SIZE;

        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

        return base.GetAsync(pageNo, pageSize, token);
    }

    public override async Task<IEnumerable<LinkItem>> GetByIdsAsync(IEnumerable<Guid> linkIds, CancellationToken token = default)
    {
        var rslts = await LinksRepository.GetAsync(linkIds.ToObjectIds(), token);

        var links = Mapper.Map<LinkItem>(rslts);

        return links;
    }

    /// <summary>
    /// Gets a collection of links from the central link repository by their domain name
    /// </summary>
    /// <param name="domain">Gets a collection of links by their domain name (eg: github.com)</param>
    /// <param name="pageNo">The number of the page of results to be returned</param>
    /// <param name="pageSize">The number of results per page</param>
    /// <param name="token"></param>
    /// /// <exception cref="ArgumentNullException">If the domain is null or empty</exception>
    /// <returns></returns>
    public override async Task<PagedResults<LinkItem>> GetLinksByDomainAsync(string domain, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        var newPageSize = pageSize ?? DEFAULT_LINKS_PAGE_SIZE;

        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

        var rslts = await LinksRepository.GetLinksByDomainAsync(domain, pageNo, newPageSize, token);

        var links = Mapper.Map<LinkItem>(rslts.Results);

        return new PagedResults<LinkItem>(links, pageNo, newPageSize, rslts.TotalCount);
    }

    /// <summary>
    /// Gets a collection of links from the central link repository that contain all of the specified tags, 
    /// </summary>
    /// <param name="tags">A collection of tags for which each result that is returned must contain</param>
    /// <param name="pageNo">The number of the page of results to be returned</param>
    /// <param name="pageSize">The number of results per page</param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override Task<PagedResults<LinkItem>> GetLinksByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        return GetLinksByTagsAsync(tags, pageNo, pageSize, token);
    }

    /// <summary>
    /// Gets a link by its Url.
    /// This is useful when you want to check if a link has already been submitted but you don't know its Id.
    /// </summary>
    /// <param name="url">The url to use to perform the search</param>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    public override async Task<Link?> GetByUrlAsync(string url, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);

        var result = await LinksRepository.GetLinkByUrlAsync(url, token);

        return result is null ? null : Mapper.Map(result);
    }

    /// <summary>
    /// Attempts to get a collection of links that are related to a specific one.
    /// </summary>
    /// <param name="linkId">The id of the link for which to get the related links for</param>
    /// <param name="count">The max number of related links to return</param>
    /// <param name="token"></param>
    /// <returns>An array of link items</returns>
    public override async Task<LinkItem[]> GetRelatedLinksAsync(Guid linkId, int? count = default, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(linkId);

        var newCount = count ?? DEFAULT_RELATED_LINKS_COUNT;

        Guard.Against.NegativeOrZero(newCount);

        var linkItems = Array.Empty<LinkItem>();

        var link = await GetAsync(linkId, token);

        if (link is null)
            return Array.Empty<LinkItem>();

        var linkTags = link.Tags.Select(t => t.Name).ToArray();

        // First try to get links that have the same tags
        if (linkTags.Length > 0)
        {
            var results = await LinksRepository.GetLinksByTagsAsync(linkTags, 1, newCount, token);

            if (results.Results.TryGetNonEnumeratedCount(out int resultsCount) && resultsCount > 0)
                linkItems = Mapper.Map<LinkItem>(results.Results).ToArray();
        }

        // Then try to get links that have the same domain
        if (linkItems.Length < newCount)
        {
            var results = await LinksRepository.GetLinksByDomainAsync(link.Domain, 1, newCount, token);

            if (results.Results.TryGetNonEnumeratedCount(out int resultsCount) && resultsCount > 0)
                linkItems = Mapper.Map<LinkItem>(results.Results).ToArray();
        }

        return linkItems.Take(newCount).ToArray();
    }

    /// <summary>
    /// Gets a collection of tags that are related to the provided tags
    /// </summary>
    /// <param name="tags">The tags to use as the bait to lure out the related tags. If no tags are provided, then all will be returned.</param>
    /// <param name="count">The number of related tags to return</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<LinkTag[]> GetRelatedTagsAsync(string[] tags, int? count = default, CancellationToken token = default)
    {
        var newCount = count ?? DEFAULT_TAGS_COUNT;

        Guard.Against.NegativeOrZero(newCount);

        tags = tags.Select(t => WebUtility.UrlDecode(t).ToLowerInvariant()).ToArray();

        var result = await LinksRepository.GetRelatedTagsAsync(tags, newCount, token);

        return Mapper.Map(result).ToArray();
    }

    public async Task<LinkTag[]> GetRelatedTagsByDomainAsync(string domain, int? count = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        var newCount = count ?? DEFAULT_TAGS_COUNT;

        Guard.Against.NegativeOrZero(newCount);

        var result = await LinksRepository.GetRelatedTagsByDomainAsync(domain, newCount, token);

        return Mapper.Map(result).ToArray();
    }

    /// <summary>
    /// Helper method to retrieve links from the repository
    /// </summary>
    /// <param name="predicate">The filter to apply to the links</param>
    /// <param name="pageNo">The current number of the page of results</param>
    /// <param name="pageSize">The size of the page - max number of results to return</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>An IEnumerable of Link models, along with the total number of pages of results, and the total number of all results for this filter</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if either pageNo or pageSize are less than 1</exception>
    private async Task<(IEnumerable<LinkItem> Results, int TotalPages, int TotalCount)> FindAsync(Expression<Func<LinkEntity, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var results = await LinksRepository.FindAsync(predicate, pageNo, pageSize, token);

        if (!results.Results.Any())
            return (Enumerable.Empty<LinkItem>(), 0, 0);

        var links = Mapper.Map<LinkItem>(results.Results);

        return (links, results.TotalPages, results.TotalCount);
    }

    public async Task<Guid> SubmitLinkAsync(string url, Guid submittedByUserId, string[]? tags = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);
        Guard.Against.NullOrEmpty(submittedByUserId);

        var linkEntity = await LinksRepository.GetLinkByUrlAsync(url, token);

        // If the link already exists in the central link repository, then we just need to add/update the tags for it
        // Then assign to the user who submitted it
        if (linkEntity == null)
            throw new NotImplementedException("The ability to add a new link from here does not exist yet. It's in the Queue service");
        else
            await UpdateExistingLinkAsync(linkEntity, tags, token);

        //await LinksRepository.AddAsync(link, token);

        //return link.Id;

        return linkEntity.Id.ToGuid();
    }

    #region - CRUD -

    public async Task<Guid> AddAsync(Link link, CancellationToken token = default)
    {
        Guard.Against.Null(link);
        Guard.Against.NullOrWhiteSpace(link.Url);
        Guard.Against.NullOrWhiteSpace(link.Title);
        Guard.Against.NullOrEmpty(link.SubmittedById);

        var entity = Mapper.Map(link);

        if (entity == null)
        {
            Logger.LogError("Unable to map link to entity");
            return Guid.Empty;
        }

        await LinksRepository.AddAsync(entity, token);

        return entity.Id.ToGuid();
    }

    /// <summary>
    /// Simplest way to add a link to the central link repository.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="title"></param>
    /// <param name="submittedById"></param>
    /// <param name="tags"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<Guid> AddAsync(string url, string title, Guid submittedById, string[]? tags = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);
        Guard.Against.NullOrWhiteSpace(title);
        Guard.Against.NullOrEmpty(submittedById);

        var entity = LinkEntity.Create(url, title, submittedById, tags);

        await LinksRepository.AddAsync(entity, token);

        return entity.Id.ToGuid();
    }

    public async Task<bool> DeleteAsync(Guid linkId, Guid deletedById, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(linkId);
        Guard.Against.NullOrEmpty(deletedById);

        try
        {
            var link = await LinksRepository.GetAsync(linkId.ToObjectId(), token);

            if (link is null)
                return false;

            // Temporarily mark the link as deleted
            // Have task delete it at a later time
            link.IsDeleted = true;
            link.DeletedById = deletedById.ToObjectId();

            link.IsActive = false;
            link.UpdatedById = deletedById.ToObjectId();
            link.DateUpdated = DateTime.UtcNow;

            await LinksRepository.UpdateAsync(link, token);

            // Log

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async ValueTask<(bool IsSuccess, string Message)> UpdateLinkAsync(Guid updatedById,
    Guid id, string title, string description, bool isActive, string[]? tags = default, CancellationToken token = default)
    {
        if (updatedById == Guid.Empty)
            return (false, "Must have a User Id to Update this Link");

        if (id == Guid.Empty)
            return (false, "Must have a Link Id to Update this Link");

        if (string.IsNullOrWhiteSpace(title))
            return (false, "Must have a Title to Update this Link");

        var linkEntity = await LinksRepository.GetAsync(id.ToObjectId(), token);

        if (linkEntity == null)
            return (false, "Link not found");

        linkEntity.IsActive = isActive;
        linkEntity.Title = title;
        linkEntity.Description = description;
        linkEntity.Tags = tags?.Select(LinkTagEntity.Create).Distinct().ToList() ?? [];
        linkEntity.DateUpdated = DateTime.UtcNow;

        try
        {
            await LinksRepository.UpdateAsync(linkEntity, token);
        }
        catch (Exception e)
        {
            return (false, e.Message);
        }

        return (true, string.Empty);
    }

    #endregion

    private async Task UpdateExistingLinkAsync(LinkEntity linkEntity, string[]? tags = default, CancellationToken token = default)
    {
        Guard.Against.Null(linkEntity);
        Guard.Against.NullOrWhiteSpace(linkEntity.Url);
        Guard.Against.NullOrWhiteSpace(linkEntity.Title);
        Guard.Against.NullOrEmpty(linkEntity.SubmittedById.ToString());

        if (tags is { Length: > 0 })
        {
            tags = tags.Distinct().ToArray();

            var linkTagNames = linkEntity.Tags.Select(t => t.Name).ToArray();

            var existingTagNames = tags.Where(t => linkTagNames.Contains(t)).ToArray();
            var nonExistingTagNames = tags.Where(t => !linkTagNames.Contains(t)).ToArray();

            // If the tag already exists, then just increment the count
            foreach (var existingTagName in existingTagNames)
            {
                var t = linkEntity.Tags.First(t => t.Name == existingTagName);
                t.Count++;
            }

            // If the tag doesn't exist, then add it to the existing tags
            linkEntity.Tags.AddRange(nonExistingTagNames.Select(LinkTagEntity.Create));

            await LinksRepository.UpdateAsync(linkEntity, token);
        }
    }

    //public async ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default)
    //{
    //    var link = await LinksRepository.GetLinkByUrlAsync(request.Url, token);

    //    var userTags = request.UsersTags.Any() ? request.UsersTags.Select(t => LinkTagEntity.Create(t)).ToArray() : Array.Empty<LinkTagEntity>();

    //    if (link == null)
    //    {
    //        //link = LinkEntity.C
    //    }


    //    if (!link.Tags.Any())
    //    {
    //        link.Tags.AddRange(userTags);
    //    }
    //    else
    //    {
    //        foreach (var tag in link.Tags)
    //        {
    //            var linkTag = link.Tags.FindAsync(t => t.Name == tag.Name);

    //            if (linkTag != null)
    //            {
    //                linkTag.Count++;
    //            }
    //            else
    //            {
    //                link.Tags.Add(tag);
    //            }
    //        }
    //    }

    //    // Associate link with user


    //    return true;
    //}
}