using Deliscio.Core.Models;
using Deliscio.Modules.UserLinks.Common.Models;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface IUserLinksManager
{
    /// <summary>
    /// Associates an existing link with a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="linkId"></param>
    /// <param name="title"></param>
    /// <param name="tags"></param>
    /// <param name="isPrivate"></param>
    /// <param name="token"></param>
    /// <returns>The id of the new user link as a string</returns>
    Task<string> AddLinkAsync(string userId, string linkId, string title = "", string[]? tags = default, bool isPrivate = false, CancellationToken token = default);

    /// <summary>
    /// Gets a specific link for a specific user.
    /// </summary>
    /// <param name="userId">The id of the user who owns the link</param>
    /// <param name="linkId">The id of the link to be returned</param>
    /// <param name="token"></param>
    /// <returns>
    /// If this user has a link with this id, then it will return the link.
    /// </returns>
    Task<UserLink?> GetUserLinkAsync(string userId, string linkId, CancellationToken token = default);

    Task<PagedResults<UserLink>> GetUserLinksAsync(string userId, int pageNo, int pageSize, CancellationToken token = default);
}