using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Modules.Links.Common.Interfaces;

public interface ILinksService
{
    Task<Guid> AddAsync(Link link, CancellationToken token = default);
    Task<Guid> AddAsync(string url, string title, Guid submittedById, string[]? tags = default, CancellationToken token = default);

    Task<Link?> GetAsync(string id, CancellationToken token = default);

    Task<Link?> GetAsync(Guid id, CancellationToken token = default);

    Task<PagedResults<Link>> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<PagedResults<Link>> GetByDomainAsync(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<PagedResults<Link>> GetByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<Link?> GetByUrlAsync(string url, CancellationToken token = default);

    ValueTask<Guid> SubmitLinkAsync(string url, Guid submittedByUserId, string[]? tags = default, CancellationToken token = default);

    ///// <summary>
    ///// Lets a user submit a link. If the link is valid and doesn't already exist, it will be added to the central link repository.
    ///// Once it exists, an association will be made between the user and the link.
    ///// </summary>
    ///// <param name="request"></param>
    ///// <param name="token"></param>
    ///// <returns></returns>
    //ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default);
}