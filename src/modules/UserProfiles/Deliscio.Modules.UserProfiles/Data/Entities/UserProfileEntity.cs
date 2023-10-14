using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;

namespace Deliscio.Modules.UserProfiles.Data.Entities;

/// <summary>
/// Represents a registered user's profile in the collection.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("UserProfile")]
[BsonCollection("userprofiles")]
public class UserProfileEntity : MongoEntityBase
{
    public string DisplayName { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; }

    public string? ImageUrl { get; set; }

    public string? Location { get; set; }
}