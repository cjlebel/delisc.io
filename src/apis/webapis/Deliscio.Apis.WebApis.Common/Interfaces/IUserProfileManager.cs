using Deliscio.Modules.UserProfiles.Common.Models;

namespace Deliscio.Apis.WebApi.Common.Interfaces;

public interface IUserProfileManager
{
    Task<UserProfile?> GetAsync(string userId, CancellationToken token = default);
}