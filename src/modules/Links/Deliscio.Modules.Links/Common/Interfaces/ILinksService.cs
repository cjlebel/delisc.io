using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Models.Requests;

namespace Deliscio.Modules.Links.Common.Interfaces;

public interface ILinksService
{
    //Task<PagedResults<LinkItem>> FindAsync(string term, string tags = "", int pageNo = 1, int? pageSize = default, CancellationToken token = default);

    Task<PagedResults<LinkItem>> FindAsync(FindLinksRequest request, CancellationToken token = default);

    Task<Link?> GetAsync(Guid linkId, CancellationToken token = default);

    [Obsolete("Use FindAsync instead")]
    Task<PagedResults<LinkItem>> GetAsync(int pageNo = 1, int? pageSize = default, CancellationToken token = default);

    Task<IEnumerable<LinkItem>> GetByIdsAsync(IEnumerable<Guid> linkIds, CancellationToken token = default);

    Task<Link?> GetByUrlAsync(string url, CancellationToken token = default);

    Task<PagedResults<LinkItem>> GetLinksByDomainAsync(string domain, int pageNo = 1, int? pageSize = default, CancellationToken token = default);

    Task<PagedResults<LinkItem>> GetLinksByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int? pageSize = default, CancellationToken token = default);

    Task<LinkItem[]> GetRelatedLinksAsync(Guid linkId, int? count = default, CancellationToken token = default);

    /// <summary>
    /// Gets a collection of tags that are related to the ones that are passed in.
    /// </summary>
    /// <param name="tags"></param>
    /// <param name="count">The number of tags to return</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<LinkTag[]> GetRelatedTagsAsync(string[] tags, int? count = default, CancellationToken token = default);

    /// <summary>
    /// Gets a collection of tags for all links that belong to the same domain.
    /// </summary>
    /// <param name="domain">The domain to use for retrieving the collection of tags</param>
    /// <param name="count">The number of tags to return</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<LinkTag[]> GetRelatedTagsByDomainAsync(string domain, int? count = default, CancellationToken token = default);

    Task<Guid> SubmitLinkAsync(string url, Guid submittedByUserId, string[]? tags = default, CancellationToken token = default);

    /*    ValueTask<(bool IsSuccess, string Message)> UpdateLinkAsync(Guid updatedById, Guid id, string title, string description, bool isActive, string[]? tags = default, CancellationToken token = default)*/

    #region - CRUD -
    Task<Guid> AddAsync(Link link, CancellationToken token = default);

    Task<bool> DeleteAsync(Guid linkId, Guid deletedById, CancellationToken token = default);

    ValueTask<(bool IsSuccess, string Message)> UpdateLinkAsync(Guid updatedById,
        Guid id, string title, string description, bool isActive, string[]? tags = default,
        CancellationToken token = default);

    #endregion
}