using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;

namespace Deliscio.Modules.UserProfiles.Data;

/// <summary>
/// Represents a registered user's profile in the collection.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("UserProfile")]
[BsonCollection("userprofiles")]
public class UserProfileEntity : MongoEntityBase
{
    public Guid UserId { get; set; }

    public string DisplayName { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    public string? Location { get; set; } = string.Empty;

    public UserProfileEntity(Guid id, string email, string displayName)
    {
        Id = id;
        Email = email;
        DisplayName = displayName;
    }

    public static UserProfileEntity Create(Guid id, string email, string displayName)
    {
        return new UserProfileEntity(id, email, displayName);
    }
}