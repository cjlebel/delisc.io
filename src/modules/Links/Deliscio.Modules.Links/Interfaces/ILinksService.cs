using Deliscio.Modules.Links.Models;
using Deliscio.Modules.Links.Requests;

namespace Deliscio.Modules.Links.Interfaces;

public interface ILinksService
{
    Task<Link?> GetAsync(string id, CancellationToken token = default);

    Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetByDomain(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<Link> Results, int TotalPages, int TotalCount)> GetByTags(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    /// <summary>
    /// Lets a user submit a link. If the link is valid and doesn't already exist, it will be added to the central link repository.
    /// Once it exists, an association will be made between the user and the link.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default);
}