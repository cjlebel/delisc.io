using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface ILinksManager
{
    Task<Link?> GetLinkAsync(string id, CancellationToken token = default);

    Task<PagedResults<Link>> GetLinksAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    //Task<IEnumerable<Link>> GetAsync(int pageNo = 1, int pageSize = 25, string userId = "",
    //           CancellationToken token = default);

    Task<PagedResults<Link>> GetLinksByTagsAsync(string[] tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<string> SubmitLinkAsync(string url, string submittedByUserId, string usersTitle = "", string usersDescription = "", string[]? tags = default, CancellationToken token = default);
}