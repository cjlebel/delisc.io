using Deliscio.Core.Models;
using Deliscio.Modules.UserLinks.Common.Models;

namespace Deliscio.Modules.UserLinks.Common.Interfaces;

public interface IUserLinksService
{
    Task<string> AddAsync(string userId, string linkId, string title = "", string[]? tags = default, bool isPrivate = false, CancellationToken token = default);

    Task<UserLink?> GetAsync(string userId, string linkId, CancellationToken token = default);

    /// <summary>
    /// Gets a page of user links that belong to the user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<PagedResults<UserLink>> GetAsync(string userId, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    /// <summary>
    /// Gets a page of user links that belong to the provided domain.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="domain"></param>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<PagedResults<UserLink>> GetByDomainAsync(string userId, string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    /// <summary>
    /// Gets a page of user links that are related to the provided tags.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tags"></param>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<PagedResults<UserLink>> GetByTagsAsync(string userId, IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    /// <summary>
    /// Gets a list of most popular tags for this user.
    /// If any tags are provided, then the returning tags will be related to the provided tags.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="count"></param>
    /// <param name="tags"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UserLinkTag> GetTagAsync(string userId, int count = 25, string[]? tags = default, CancellationToken token = default);
}