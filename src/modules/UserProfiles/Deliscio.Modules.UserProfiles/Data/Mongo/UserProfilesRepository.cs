using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Interfaces;
using Deliscio.Modules.UserProfiles.Data.Entities;
using Deliscio.Modules.UserProfiles.Interfaces;
using Microsoft.Extensions.Options;

namespace Deliscio.Modules.UserProfiles.Data.Mongo;

internal class UserProfilesRepository : MongoRepository<UserProfileEntity>, IUserProfilesRepository
{
    #region - Constructors -
    public UserProfilesRepository(IOptions<MongoDbOptions> options) : base(options) { }

    public UserProfilesRepository(IMongoDbClient client) : base(client) { }
    #endregion
}