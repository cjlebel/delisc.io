using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Interfaces;
using Deliscio.Modules.UserAuthentication.Data.Entities;
using Deliscio.Modules.UserAuthentication.Interfaces;
using Microsoft.Extensions.Options;

namespace Deliscio.Modules.UserAuthentication.Data.Mongo;

internal class UserRepository : MongoRepository<UserEntity>, IUserRepository
{
    #region - Constructors -
    public UserRepository(IOptions<MongoDbOptions> options) : base(options) { }

    //public UserRepository(IMongoDbClient client) : base(client) { }
    #endregion
}