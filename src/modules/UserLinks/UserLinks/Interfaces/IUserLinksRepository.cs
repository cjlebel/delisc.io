using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.UserLinks.Data.Entities;

namespace Deliscio.Modules.UserLinks.Interfaces;

public interface IUserLinksRepository : IRepository<UserLinkEntity>
{
    Task<UserLinkEntity?> GetAsync(Guid userId, Guid linkId, CancellationToken token = default);

    Task<(IEnumerable<UserLinkEntity> Results, int TotalPages, int TotalCount)> GetAsync(Guid userId, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    //Task<(IEnumerable<UserLinkEntity> Results, int TotalPages, int TotalCount)> GetByDomain(Guid userId, string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<UserLinkEntity> Results, int TotalPages, int TotalCount)> GetByTagsAsync(Guid userId, IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);
}