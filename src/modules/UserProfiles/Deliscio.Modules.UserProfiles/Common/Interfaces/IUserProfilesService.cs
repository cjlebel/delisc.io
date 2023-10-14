using Deliscio.Modules.UserProfiles.Common.Models;

namespace Deliscio.Modules.UserProfiles.Common.Interfaces;

public interface IUserProfilesService
{
    Task<UserProfile?> GetAsync(Guid userId, CancellationToken token = default);
}