using Deliscio.Modules.UserProfiles.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;

namespace Deliscio.Modules.UserProfiles;

public class UserProfilesService : IUserProfilesService
{
    public Task<UserProfile?> GetAsync(Guid userId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
