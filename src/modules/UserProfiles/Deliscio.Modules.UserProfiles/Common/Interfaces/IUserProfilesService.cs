using Deliscio.Core.Models;
using Deliscio.Modules.UserProfiles.Common.Models;

namespace Deliscio.Modules.UserProfiles.Common.Interfaces;

public interface IUserProfilesService
{
    Task<string> AddAsync(UserProfile userProfile, CancellationToken token = default);

    Task<Results<UserProfile?>> GetAsync(Guid userId, CancellationToken token = default);

    Task<PagedResults<UserProfileItem>> SearchAsync(string displayName = "", string email = "", int page = 1, int pageSize = 50, CancellationToken token = default);
}