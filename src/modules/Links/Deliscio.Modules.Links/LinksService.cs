using System.Linq.Expressions;

using Ardalis.GuardClauses;

using Deliscio.Core.Abstracts;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.Mappers;
using Deliscio.Modules.Links.Requests;

using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Links;

public class LinksService : ServiceBase, ILinksService
{
    private readonly ILogger<LinksService> _logger;
    private readonly ILinksRepository _linksRepository;

    public LinksService(ILinksRepository linksRepository, ILogger<LinksService> logger)
    {
        Guard.Against.Null(linksRepository);
        Guard.Against.Null(logger);

        _linksRepository = linksRepository;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(Link link, CancellationToken token = default)
    {
        Guard.Against.Null(link);
        Guard.Against.NullOrWhiteSpace(link.Url);
        Guard.Against.NullOrWhiteSpace(link.Title);
        Guard.Against.NullOrEmpty(link.SubmittedById);

        var entity = Mapper.Map(link);

        if (entity != null)
        {
            await _linksRepository.AddAsync(entity, token);

            return entity.Id;
        }

        return Guid.Empty;
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

        await _linksRepository.AddAsync(entity, token);

        return entity.Id;
    }

    /// <summary>
    /// Gets a single link by its id from the central link repository
    /// </summary>
    /// <param name="id">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<Link?> GetAsync(string id, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(id);

        return await GetAsync(new Guid(id), token);
    }

    /// <summary>
    /// Gets a single link by its id from the central link repository
    /// </summary>
    /// <param name="id">The id of the link to retrieve</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<Link?> GetAsync(Guid id, CancellationToken token = default)
    {
        Guard.Against.NullOrEmpty(id);

        var result = await _linksRepository.GetAsync(id, token);

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
    public async Task<PagedResults<Link>> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        var rslts = await Find(_ => true, pageNo, pageSize, token);

        return GetPageOfResults(rslts.Results, pageNo, pageSize, rslts.TotalCount);
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
    public async Task<PagedResults<Link>> GetByDomainAsync(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        var rslts = await _linksRepository.GetByDomainAsync(domain, pageNo, pageSize, token);

        var links = Mapper.Map(rslts.Results);

        return GetPageOfResults(links, pageNo, pageSize, rslts.TotalCount);
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
    public async Task<PagedResults<Link>> GetByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        var array = tags?.ToArray() ?? Array.Empty<string>();

        Guard.Against.NullOrEmpty(array, message: $"{nameof(tags)} cannot be null or empty");

        var rslts = await _linksRepository.GetByTagsAsync(array, pageNo, pageSize, token);

        var links = Mapper.Map(rslts.Results);

        return GetPageOfResults(links, pageNo, pageSize, rslts.TotalCount);
    }

    public async Task<Link?> GetByUrlAsync(string url, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);

        var result = await _linksRepository.GetByUrlAsync(url, token);

        return result is null ? null : Mapper.Map(result);
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
    private async Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> Find(Expression<Func<LinkEntity, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var results = await _linksRepository.FindAsync(predicate, pageNo, pageSize, token);

        if (!results.Results.Any())
            return (Enumerable.Empty<Link>(), 0, 0);

        var links = Mapper.Map(results.Results);

        return (links, results.TotalPages, results.TotalCount);
    }

    public async ValueTask<Guid> SubmitLinkAsync(string url, Guid submittedByUserId, string[]? tags = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);
        Guard.Against.NullOrEmpty(submittedByUserId);

        var link = await _linksRepository.GetByUrlAsync(url, token);

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

            await _linksRepository.UpdateAsync(link, token);


        }

        //await _linksRepository.AddAsync(link, token);

        //return link.Id;

        return link.Id;
    }

    //public async ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default)
    //{
    //    var link = await _linksRepository.GetByUrlAsync(request.Url, token);

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