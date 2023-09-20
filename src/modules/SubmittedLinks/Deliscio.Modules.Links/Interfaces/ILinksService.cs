using Deliscio.Modules.Links.Models;

namespace Deliscio.Modules.Links.Interfaces;

public interface ILinksService
{
    Task<Link?> GetAsync(string id, CancellationToken token = default);

    Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetByDomain(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetByTags(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);


}