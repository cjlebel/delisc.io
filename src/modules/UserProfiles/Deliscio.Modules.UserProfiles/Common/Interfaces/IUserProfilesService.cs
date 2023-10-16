using Deliscio.Modules.UserProfiles.Common.Models;

namespace Deliscio.Modules.UserProfiles.Common.Interfaces;

public interface IUserProfilesService
{
    Task<string> AddAsync(UserProfile userProfile, CancellationToken token = default);

    Task<UserProfile?> GetAsync(Guid userId, CancellationToken token = default);
}