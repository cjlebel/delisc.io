using Ardalis.GuardClauses;
using Deliscio.Core.Data.Mongo;
using Microsoft.Extensions.Options;

namespace Deliscio.Modules.UserProfiles.Data;

internal class UserProfilesRepository : MongoRepository<UserProfileEntity>, IUserProfilesRepository
{
    #region - Constructors -
    public UserProfilesRepository(IOptions<MongoDbOptions> options) : base(options) { }

    //public UserProfilesRepository(IMongoDbClient client) : base(client) { }
    #endregion

    public Task<UserProfileEntity?> GetAync(Guid userId, CancellationToken token = default)
    {
        Guard.Against.Default(userId);

        return FirstOrDefaultAsync(x => x.Id == userId, token);
    }

    //Note: Need to rethink this. I only want to return parts of the user profile, not all of the details for each one.
    //May need to not use the underlying generic repo and instead have my own implementation.
    public Task<(IEnumerable<UserProfileEntity> Results, int TotalPages, int TotalCount)> SearchAsync(string displayName = "", string email = "", int page = 1, int pageSize = 50, CancellationToken token = default)
    {
        return FindAsync(x =>
            (string.IsNullOrWhiteSpace(displayName) || x.DisplayName.Contains(displayName)) ||
            (string.IsNullOrWhiteSpace(email) || x.Email.Contains(email)), page, pageSize, token
        );
    }
}