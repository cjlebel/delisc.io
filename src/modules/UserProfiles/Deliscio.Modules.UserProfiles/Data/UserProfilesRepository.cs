using Ardalis.GuardClauses;
using Deliscio.Core.Data.Mongo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Deliscio.Modules.UserProfiles.Data;

public class UserProfilesRepository : MongoRepository<UserProfileEntity>, IUserProfilesRepository
{
    private readonly ILogger<UserProfilesRepository> _logger;

    #region - Constructors -

    public UserProfilesRepository(IOptions<MongoDbOptions> options, ILogger<UserProfilesRepository> logger) :
        base(options)
    {
        _logger = logger;
    }

    //public UserProfilesRepository(IMongoDbClient client) : base(client) { }
    #endregion

    public Task<UserProfileEntity?> GetAsync(string userId, CancellationToken token = default)
    {
        Guard.Against.Default(userId);

        if (ObjectId.TryParse(userId, out var userObjectId))
        {
            return FirstOrDefaultAsync(x => x.Id == userObjectId, token);
        }

        _logger.LogWarning("Could not find profile for user id {UserId}", userId);

        return Task.FromResult<UserProfileEntity?>(default);
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