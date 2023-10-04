using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface ILinksManager
{
    Task<Link?> GetLinkAsync(string id, CancellationToken token = default);

    Task<PagedResults<Link>> GetLinksAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    ValueTask<IEnumerable<Link>> GetLinksByIdsAsync(string[] ids, CancellationToken token = default);

    ValueTask<PagedResults<Link>> GetLinksByTagsAsync(string[] tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    /// <summary>
    /// Gets a collection of tags.
    /// If tags are provided, then the returning tags will be related to them via the links.
    /// Else, if no tags are provided, then the returning tags will be the most popular tags.
    /// </summary>
    /// <param name="tags">The tags to filter by</param>
    /// <param name="count">The number of tags to return</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<LinkTag[]> GetTagsAsync(string[] tags, int? count = default, CancellationToken token = default);

    Task<string> SubmitLinkAsync(string url, string submittedByUserId, string usersTitle = "", string[]? tags = default, CancellationToken token = default);
}