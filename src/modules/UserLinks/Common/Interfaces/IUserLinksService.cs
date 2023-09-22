using Deliscio.Modules.UserLinks.Common.Models;

namespace Deliscio.Modules.UserLinks.Common.Interfaces;

public interface IUserLinksService
{
    Task<UserLink> GetAsync(string id, CancellationToken token = default);

    Task<(IEnumerable<UserLink> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<UserLink> Results, int TotalPages, int TotalCount)> GetByDomain(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<UserLink> Results, int TotalPages, int TotalCount)> GetByTags(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);


}