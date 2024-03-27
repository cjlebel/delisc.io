using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.UserAuthentication.Data.Entities;
using MongoDB.Bson;

namespace Deliscio.Modules.UserAuthentication.Interfaces;

public interface IUserRepository : IRepository<UserEntity, ObjectId>
{

}