using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace Deliscio.Modules.Authentication.Data.Entities;

[CollectionName("Auth_Users")]
public class AuthUserEntity : MongoIdentityUser<ObjectId>
{
    public DateTimeOffset? DateLastLoggedIn { get; set; }

    public DateTimeOffset? DateLastSeen { get; set; }

    public static AuthUserEntity Create(string username, string email, string password)
    {
        return new AuthUserEntity
        {
            UserName = username,
            Email = email,
            PasswordHash = password
        };
    }
}