using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.UserLinks.Data.Entities;
using MongoDB.Bson;

namespace Deliscio.Modules.UserLinks.Interfaces;

public interface IUserLinksRepository : IRepository<UserLinkEntity, ObjectId>
{
    Task<UserLinkEntity?> GetAsync(string userId, string linkId, CancellationToken token = default);

    Task<(IEnumerable<UserLinkEntity> Results, int TotalPages, int TotalCount)> GetAsync(string userId, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    //Task<(IEnumerable<UserLinkEntity> Results, int TotalPages, int TotalCount)> GetByDomain(Guid userId, string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<UserLinkEntity> Results, int TotalPages, int TotalCount)> GetByTagsAsync(string userId, IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);
}