using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;

namespace Deliscio.Modules.UserAuthentication.Data.Entities;

/// <summary>
/// Represents a registered user in the collection.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("User")]
[BsonCollection("users")]
public class UserEntity : MongoEntityBase
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }

    private UserEntity() { }

    public UserEntity(string username, string password)
    {
        Username = username;
        PasswordHash = password;
    }

    public static UserEntity Create(string username, string passwordHash)
    {
        var now = DateTimeOffset.UtcNow;

        return new UserEntity
        {
            Username = username,
            PasswordHash = passwordHash,

            DateCreated = now,
            DateUpdated = now
        };
    }
}