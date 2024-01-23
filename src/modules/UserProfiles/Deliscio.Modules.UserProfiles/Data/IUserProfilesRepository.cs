using Deliscio.Core.Data.Interfaces;

namespace Deliscio.Modules.UserProfiles.Data;

public interface IUserProfilesRepository : IRepository<UserProfileEntity>
{
    /// <summary>
    /// Gets a user's profile by the user's id.
    /// </summary>
    /// <param name="userId">THe id of the user</param>
    /// <param name="token"></param>
    /// <returns>
    /// A <see cref="UserProfileEntity"/> if the user exists, otherwise default(UserProfileEntity).
    /// </returns>
    Task<UserProfileEntity?> GetAync(Guid userId, CancellationToken token = default);

    Task<(IEnumerable<UserProfileEntity> Results, int TotalPages, int TotalCount)> SearchAsync(string displayName = "", string email = "", int page = 1, int pageSize = 50, CancellationToken token = default);
}