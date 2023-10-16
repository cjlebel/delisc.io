using Ardalis.GuardClauses;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Interfaces;
using Microsoft.Extensions.Options;

namespace Deliscio.Modules.UserProfiles.Data;

internal class UserProfilesRepository : MongoRepository<UserProfileEntity>, IUserProfilesRepository
{
    #region - Constructors -
    public UserProfilesRepository(IOptions<MongoDbOptions> options) : base(options) { }

    public UserProfilesRepository(IMongoDbClient client) : base(client) { }
    #endregion

    public Task<UserProfileEntity?> GetAync(Guid userId, CancellationToken token = default)
    {
        Guard.Against.Default(userId);

        return FirstOrDefaultAsync(x => x.Id == userId, token);
    }
}