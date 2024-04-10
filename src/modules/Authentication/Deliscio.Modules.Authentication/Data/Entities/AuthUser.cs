using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace Deliscio.Modules.Authentication.Data.Entities;

[CollectionName("Auth_Users")]
//[BsonCollection("Auth_Users")]
public class AuthUser : MongoIdentityUser<ObjectId>
{
    public DateTimeOffset? DateLastLoggedIn { get; set; }

    public DateTimeOffset? DateLastSeen { get; set; }

    public static AuthUser Create(string username, string email, string password)
    {
        return new AuthUser
        {
            UserName = username,
            Email = email,
            PasswordHash = password
        };
    }
}