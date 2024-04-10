using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;

namespace Deliscio.Modules.UserProfiles.Data;

/// <summary>
/// Represents a registered user's profile (not to be confused with the Authenticated User object).
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("UserProfile")]
[BsonCollection("userprofiles")]
public class UserProfileEntity : MongoEntityBase
{
    [Required]
    public string DisplayName { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; }

    public string? ImageUrl { get; set; } = string.Empty;

    public string? Location { get; set; } = string.Empty;

    public DateTimeOffset DateRegistered { get; set; }

    public DateTimeOffset DateLastSeen { get; set; }

    public UserProfileEntity(Guid id, string email, string displayName)
    {
        Id = id.ToObjectId();
        Email = email;
        DisplayName = displayName;
    }

    public static UserProfileEntity Create(Guid id, string email, string displayName)
    {
        return new UserProfileEntity(id, email, displayName);
    }


}