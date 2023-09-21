using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.Mappers;
using Deliscio.Modules.Links.Models;
using Deliscio.Modules.Links.Requests;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Links;

public class LinksService : ILinksService
{
    private readonly ILogger<LinksService> _logger;
    private readonly ILinksRepository _linksRepository;

    public LinksService(ILinksRepository linksRepository, ILogger<LinksService> logger)
    {
        _logger = logger;
        _linksRepository = linksRepository;
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

        var result = await _linksRepository.GetAsync(new Guid(id), token);

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
    public Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        return Find(_ => true, pageNo, pageSize, token);
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
    public async Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetByDomain(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        var results = await _linksRepository.GetByDomainAsync(domain, pageNo, pageSize, token);

        var links = Mapper.Map(results.Results);

        return (links, results.TotalPages, results.TotalCount);
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
    public Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetByTags(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        throw new NotImplementedException();
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
    private async Task<(IEnumerable<Link>, int TotalPages, int TotalCount)> Find(Expression<Func<LinkEntity, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var results = await _linksRepository.FindAsync(predicate, pageNo, pageSize, token);

        if (!results.Results.Any())
            return (Enumerable.Empty<Link>(), 0, 0);

        var links = Mapper.Map(results.Results);

        return (links, results.TotalPages, results.TotalCount);
    }

    public async ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default)
    {
        var link = await _linksRepository.GetByUrlAsync(request.Url, token);

        var userTags = request.UsersTags.Any() ? request.UsersTags.Select(t => TagEntity.Create(t)).ToArray() : Array.Empty<TagEntity>();

        if (link == null)
        {
            link = LinkEntity.C
        }


        if (!link.Tags.Any())
        {
            link.Tags.AddRange(userTags);
        }
        else
        {
            foreach (var tag in link.Tags)
            {
                var linkTag = link.Tags.Find(t => t.Name == tag.Name);

                if (linkTag != null)
                {
                    linkTag.Count++;
                }
                else
                {
                    link.Tags.Add(tag);
                }
            }
        }

        // Associate link with user


        return new ValueTask<bool>(true);
    }
}