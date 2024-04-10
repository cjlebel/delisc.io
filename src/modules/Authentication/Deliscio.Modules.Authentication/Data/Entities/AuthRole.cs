using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace Deliscio.Modules.Authentication.Data.Entities;

[CollectionName("Auth_Roles")]
public class AuthRole : MongoIdentityRole<ObjectId>
{

}