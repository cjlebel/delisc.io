using System.Linq.Expressions;
using System.Net;
using Ardalis.GuardClauses;
using Deliscio.Common.Abstracts;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Mappers;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Links;

public abstract class LinksBaseService<T> : ServiceBase
{
    protected readonly ILinksRepository LinksRepository;
    protected readonly ILogger<T> Logger;

    //TODO: Get these from config
    protected const int DEFAULT_LINKS_PAGE_SIZE = 50;
    protected const int DEFAULT_RELATED_LINKS_COUNT = 20;
    protected const int DEFAULT_TAGS_COUNT = 50;

    protected LinksBaseService(ILinksRepository linksRepository, ILogger<T> logger)
    {
        LinksRepository = Guard.Against.Null(linksRepository);
        Logger = Guard.Against.Null(logger);
    }

    //public LinksService(MongoDbClient client, ILogger<LinksService> logger)
    //{
    //    Guard.Against.Null(client);
    //    Guard.Against.Null(logger);

    //    LinksRepository = new LinksRepository(client);
    //    _logger = logger;
    //}

    //public LinksService(IOptions<MongoDbOptions> options, ILogger<LinksService> logger)
    //{
    //    Guard.Against.Null(options);
    //    Guard.Against.Null(logger);

    //    LinksRepository = new LinksRepository(options);
    //    _logger = logger;
    //}

    protected virtual async Task<PagedResults<LinkItem>> FindLinksAsync(string term, string[] tags, string domain,
        int pageNo, int pageSize, int skip = 0, bool? isActive = default, bool? isFlagged = default,
        bool? isDeleted = false, CancellationToken token = default)
    {
        if (pageNo < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNo));

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        var rslts = await LinksRepository.FindAsync(term, tags, domain, pageNo, pageSize, skip, isActive, isFlagged, isDeleted, token);

        var links = Mapper.Map<LinkItem>(rslts.Results);

        var page = new PagedResults<LinkItem>(links, pageNo, pageSize, rslts.TotalCount);

        return page;
    }

    /// <summary>
    /// Gets a single link by its id from the central link repository
    /// </summary>
    /// <param name="linkId">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public virtual async Task<Link?> GetAsync(string linkId, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(linkId);

        return await GetAsync(new Guid(linkId), token);
    }

    /// <summary>
    /// Gets a single link by its id from the central link repository
    /// </summary>
    /// <param name="linkId">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public virtual async Task<Link?> GetAsync(Guid linkId, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(linkId);

        var result = await LinksRepository.GetAsync(linkId.ToObjectId(), token);

        var link = Mapper.Map(result);

        return link;
    }

    /// <summary>
    /// Gets a collection of links from the central link repository.
    /// </summary>
    /// <param name="pageNo">The number of the page of results to be returned</param>
    /// <param name="pageSize">The number of results per page</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public virtual async Task<PagedResults<LinkItem>> GetAsync(int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        var newPageSize = pageSize ?? DEFAULT_LINKS_PAGE_SIZE;

        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

        var rslts = await FindLinksAsync(_ => true, pageNo, newPageSize, token);

        return rslts;
    }

    public virtual async Task<IEnumerable<LinkItem>> GetByIdsAsync(IEnumerable<Guid> linkIds, CancellationToken token = default)
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
    public virtual async Task<PagedResults<LinkItem>> GetLinksByDomainAsync(string domain, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
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
    public virtual async Task<PagedResults<LinkItem>> GetLinksByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        var array = tags.ToArray();

        Guard.Against.NullOrEmpty(array, message: $"{nameof(tags)} cannot be null or empty");

        var newPageSize = pageSize ?? DEFAULT_LINKS_PAGE_SIZE;

        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

        array = array.Select(t => WebUtility.UrlDecode(t).ToLowerInvariant()).ToArray();

        var rslts = await LinksRepository.GetLinksByTagsAsync(array, pageNo, newPageSize, token);

        var links = Mapper.Map<LinkItem>(rslts.Results);

        return new PagedResults<LinkItem>(links, pageNo, newPageSize, rslts.TotalCount);
    }

    /// <summary>
    /// Gets a link by its Url.
    /// This is useful when you want to check if a link has already been submitted but you don't know its Id.
    /// </summary>
    /// <param name="url">The url to use to perform the search</param>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    public virtual async Task<Link?> GetByUrlAsync(string url, CancellationToken token = default)
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
    public virtual async Task<LinkItem[]> GetRelatedLinksAsync(Guid linkId, int? count = default, CancellationToken token = default)
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
    public virtual async Task<LinkTag[]> GetRelatedTagsAsync(string[] tags, int? count = default, CancellationToken token = default)
    {
        var newCount = count ?? DEFAULT_TAGS_COUNT;

        Guard.Against.NegativeOrZero(newCount);

        tags = tags.Select(t => WebUtility.UrlDecode(t).ToLowerInvariant()).ToArray();

        var result = await LinksRepository.GetRelatedTagsAsync(tags, newCount, token);

        return Mapper.Map(result).ToArray();
    }

    public virtual async Task<LinkTag[]> GetRelatedTagsByDomainAsync(string domain, int? count = default, CancellationToken token = default)
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
    private async Task<PagedResults<LinkItem>> FindLinksAsync(Expression<Func<LinkEntity, bool>> predicate, int pageNo, int pageSize, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var results = await LinksRepository.FindAsync(predicate, pageNo, pageSize, token);

        if (!results.Results.Any())
            return new PagedResults<LinkItem>(Enumerable.Empty<LinkItem>(), pageNo, pageSize, 0);

        var links = Mapper.Map<LinkItem>(results.Results);

        var rslts = new PagedResults<LinkItem>(links, pageNo, pageSize, results.TotalCount);

        return rslts;
    }

    public virtual async Task<Guid> SubmitLinkAsync(string url, Guid submittedByUserId, string[]? tags = default, CancellationToken token = default)
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
}