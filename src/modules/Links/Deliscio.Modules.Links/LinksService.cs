using System.Linq.Expressions;
using System.Net;
using Ardalis.GuardClauses;

using Deliscio.Core.Abstracts;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.Mappers;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Structurizr.Annotations;

namespace Deliscio.Modules.Links;

[Component(Description = "Deliscio service that deals with the central Links", Technology = "C#")]
[UsedBySoftwareSystem("Deliscio.Apis.WebApi", Description = "Links Service")]
public class LinksService : ServiceBase, ILinksService
{
    private readonly ILogger<LinksService> _logger;
    private readonly ILinksRepository _repository;

    //TODO: Get these from config
    private readonly int _defaultLinksPageSize = 50;
    private readonly int _defaultRelatedLinksCount = 20;
    private readonly int _defaultTagsCount = 50;

    //public LinksService(ILinksRepository linksRepository, ILogger<LinksService> logger)
    //{
    //    Guard.Against.Null(linksRepository);
    //    Guard.Against.Null(logger);

    //    _repository = linksRepository;
    //    _logger = logger;
    //}

    public LinksService(IOptions<MongoDbOptions> options, ILogger<LinksService> logger)
    {
        Guard.Against.Null(options);
        Guard.Against.Null(logger);

        _repository = new LinksRepository(options);
        _logger = logger;
    }

    public async Task<Guid> AddAsync(Link link, CancellationToken token = default)
    {
        Guard.Against.Null(link);
        Guard.Against.NullOrWhiteSpace(link.Url);
        Guard.Against.NullOrWhiteSpace(link.Title);
        Guard.Against.NullOrEmpty(link.SubmittedById);

        var entity = Mapper.Map(link);

        if (entity == null)
        {
            _logger.LogError("Unable to map link to entity");
            return Guid.Empty;
        }

        await _repository.AddAsync(entity, token);

        return entity.Id;
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

        await _repository.AddAsync(entity, token);

        return entity.Id;
    }

    /// <summary>
    /// Gets a single link by its id from the central link repository
    /// </summary>
    /// <param name="linkId">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<Link?> GetAsync(string linkId, CancellationToken token = default)
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
    public async Task<Link?> GetAsync(Guid linkId, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(linkId);

        var result = await _repository.GetAsync(linkId, token);

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
    public async Task<PagedResults<LinkItem>> GetAsync(int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        var newPageSize = pageSize ?? _defaultLinksPageSize;

        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

        var rslts = await Find(_ => true, pageNo, newPageSize, token);

        return GetPageOfResults(rslts.Results, pageNo, newPageSize, rslts.TotalCount);
    }

    public async Task<IEnumerable<LinkItem>> GetByIdsAsync(IEnumerable<Guid> linkIds, CancellationToken token = default)
    {
        var rslts = await _repository.GetAsync(linkIds, token);

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
    public async Task<PagedResults<LinkItem>> GetLinksByDomainAsync(string domain, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        var newPageSize = pageSize ?? _defaultLinksPageSize;

        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

        var rslts = await _repository.GetLinksByDomainAsync(domain, pageNo, newPageSize, token);

        var links = Mapper.Map<LinkItem>(rslts.Results);

        return GetPageOfResults(links, pageNo, newPageSize, rslts.TotalCount);
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
    public async Task<PagedResults<LinkItem>> GetLinksByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
    {
        var array = tags.ToArray();

        Guard.Against.NullOrEmpty(array, message: $"{nameof(tags)} cannot be null or empty");

        var newPageSize = pageSize ?? _defaultLinksPageSize;

        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

        array = array.Select(t => WebUtility.UrlDecode(t).ToLowerInvariant()).ToArray();

        var rslts = await _repository.GetLinksByTagsAsync(array, pageNo, newPageSize, token);

        var links = Mapper.Map<LinkItem>(rslts.Results);

        return GetPageOfResults(links, pageNo, newPageSize, rslts.TotalCount);
    }

    /// <summary>
    /// Gets a link by its Url.
    /// This is useful when you want to check if a link has already been submitted but you don't know its Id.
    /// </summary>
    /// <param name="url">The url to use to perform the search</param>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    public async Task<Link?> GetByUrlAsync(string url, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);

        var result = await _repository.GetLinkByUrlAsync(url, token);

        return result is null ? null : Mapper.Map(result);
    }

    /// <summary>
    /// Attempts to get a collection of links that are related to a specific one.
    /// </summary>
    /// <param name="linkId">The id of the link for which to get the related links for</param>
    /// <param name="count">The max number of related links to return</param>
    /// <param name="token"></param>
    /// <returns>An array of link items</returns>
    public async Task<LinkItem[]> GetRelatedLinksAsync(Guid linkId, int? count = default, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(linkId);

        var newCount = count ?? _defaultRelatedLinksCount;

        Guard.Against.NegativeOrZero(newCount);

        var linkItems = Array.Empty<LinkItem>();

        var link = await GetAsync(linkId, token);

        if (link is null)
            return Array.Empty<LinkItem>();

        var linkTags = link.Tags.Select(t => t.Name).ToArray();

        if (linkTags.Length > 0)
        {
            var results = await _repository.GetLinksByTagsAsync(linkTags, 1, newCount, token);

            if (results.Results.TryGetNonEnumeratedCount(out int resultsCount) && resultsCount > 0)
                linkItems = Mapper.Map<LinkItem>(results.Results).ToArray();
        }

        if (linkItems.Length < newCount)
        {
            var results = await _repository.GetLinksByDomainAsync(link.Domain, 1, newCount, token);

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
        var newCount = count ?? _defaultTagsCount;

        Guard.Against.NegativeOrZero(newCount);

        tags = tags.Select(t => WebUtility.UrlDecode(t).ToLowerInvariant()).ToArray();

        var result = await _repository.GetRelatedTagsAsync(tags, newCount, token);

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
    private async Task<(IEnumerable<LinkItem> Results, int TotalPages, int TotalCount)> Find(Expression<Func<LinkEntity, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var results = await _repository.FindAsync(predicate, pageNo, pageSize, token);

        if (!results.Results.Any())
            return (Enumerable.Empty<LinkItem>(), 0, 0);

        var links = Mapper.Map<LinkItem>(results.Results);

        return (links, results.TotalPages, results.TotalCount);
    }

    public async ValueTask<Guid> SubmitLinkAsync(string url, Guid submittedByUserId, string[]? tags = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);
        Guard.Against.NullOrEmpty(submittedByUserId);

        var link = await _repository.GetLinkByUrlAsync(url, token);

        // If the link already exists in the central link repository, then we just need to add/update the tags for it
        // Then assign to the user who submitted it
        if (link == null)
            return Guid.Empty;

        if (tags is { Length: > 0 })
        {
            tags = tags.Distinct().ToArray();

            var linkTagNames = link.Tags.Select(t => t.Name).ToArray();

            var existingTagNames = tags.Where(t => linkTagNames.Contains(t)).ToArray();
            var nonExistingTagNames = tags.Where(t => !linkTagNames.Contains(t)).ToArray();

            foreach (var existingTagName in existingTagNames)
            {
                var t = link.Tags.First(t => t.Name == existingTagName);
                t.Count++;
            }

            link.Tags.AddRange(nonExistingTagNames.Select(LinkTagEntity.Create));

            await _repository.UpdateAsync(link, token);


        }

        //await _repository.AddAsync(link, token);

        //return link.Id;

        return link.Id;
    }

    //public async ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default)
    //{
    //    var link = await _repository.GetLinkByUrlAsync(request.Url, token);

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
    //            var linkTag = link.Tags.Find(t => t.Name == tag.Name);

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