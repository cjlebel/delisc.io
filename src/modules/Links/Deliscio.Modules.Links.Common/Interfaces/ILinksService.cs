using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Modules.Links.Common.Interfaces;

public interface ILinksService
{
    Task<Link?> GetAsync(string id, CancellationToken token = default);

    Task<Link?> GetAsync(Guid id, CancellationToken token = default);

    Task<PagedResults<Link>> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<PagedResults<Link>> GetByDomain(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<PagedResults<Link>> GetByTags(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    ///// <summary>
    ///// Lets a user submit a link. If the link is valid and doesn't already exist, it will be added to the central link repository.
    ///// Once it exists, an association will be made between the user and the link.
    ///// </summary>
    ///// <param name="request"></param>
    ///// <param name="token"></param>
    ///// <returns></returns>
    //ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default);
}