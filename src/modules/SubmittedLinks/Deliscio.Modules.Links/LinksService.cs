using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.Mappers;
using Deliscio.Modules.Links.Models;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Links;

public class LinksService : ILinksService
{
    private readonly ILogger<LinksService> _logger;
    private readonly ILinksRepository _linksRepository;

    public LinksService(ILinksRepository linksRepository,
        ILogger<LinksService> logger)
    {
        _logger = logger;
        _linksRepository = linksRepository;
    }

    public async Task<Link?> GetAsync(string id, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(id);

        var result = await _linksRepository.GetAsync(new Guid(id), token);

        var link = Mapper.Map(result);

        return link;
    }

    public Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        return Find(_ => true, pageNo, pageSize, token);
    }

    public async Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetByDomain(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        var results = await _linksRepository.GetByDomainAsync(domain, pageNo, pageSize, token);

        var links = Mapper.Map(results.Results);

        return (links, results.TotalPages, results.TotalCount);
    }

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
}