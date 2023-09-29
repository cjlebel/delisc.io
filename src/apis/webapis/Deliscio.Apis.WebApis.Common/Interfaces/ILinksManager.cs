using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface ILinksManager
{
    Task<Link?> GetLinkAsync(string id, CancellationToken token = default);

    Task<PagedResults<Link>> GetLinksAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    ValueTask<IEnumerable<Link>> GetLinksByIdsAsync(string[] ids, CancellationToken token = default);

    ValueTask<PagedResults<Link>> GetLinksByTagsAsync(string[] tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<LinkTag[]> GetRelatedTagsAsync(string[] tags, int? count = default, CancellationToken token = default);

    Task<string> SubmitLinkAsync(string url, string submittedByUserId, string usersTitle = "", string[]? tags = default, CancellationToken token = default);
}