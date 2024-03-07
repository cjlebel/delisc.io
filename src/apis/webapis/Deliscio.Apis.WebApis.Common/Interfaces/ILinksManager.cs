using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface ILinksManager
{
    Task<PagedResults<LinkItem>> FindAsync(string search = "", string tags = "", int pageNo = 1, int pageSize = 50, CancellationToken token = default);

    Task<Link?> GetLinkAsync(string id, CancellationToken token = default);

    Task<PagedResults<LinkItem>> GetLinksAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<PagedResults<LinkItem>> GetLinksByDomainAsync(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    ValueTask<IEnumerable<LinkItem>> GetLinksByIdsAsync(string[] ids, CancellationToken token = default);

    ValueTask<PagedResults<LinkItem>> GetLinksByTagsAsync(string tags = "", int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    /// <summary>
    /// Gets a collection of links that are related to the link with the provided id.
    /// </summary>
    /// <param name="id">THe id of the link for which the related links will be based on</param>
    /// <returns></returns>
    ValueTask<LinkItem[]> GetRelatedLinksAsync(string id, CancellationToken token = default);

    /// <summary>
    /// Gets a collection of tags.
    /// If tags are provided, then the returning tags will be related to them via the links.
    /// Else, if no tags are provided, then the returning tags will be the most popular tags.
    /// </summary>
    /// <param name="tags">The tags to filter by</param>
    /// <param name="count">The number of tags to return</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<LinkTag[]> GetTagsRelatedToTagsAsync(string tags, int? count = default, CancellationToken token = default);

    Task<string> SubmitLinkAsync(string url, string submittedByUserId, string usersTitle = "", string usersDescription = "", string[]? tags = default, CancellationToken token = default);
}