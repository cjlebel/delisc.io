using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Deliscio.Modules.UserLinks.Data.Entities;

/// <summary>
/// Represents a link that is associated with a individual user.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("UserLink")]
[BsonCollection("user_links")]
public class UserLinkEntity : MongoEntityBase
{
    [BsonId]
    public Guid LinkId { get; set; }

    /// <summary>
    /// The description/summary of the page that the link points to.
    /// This is retrieved/built from the page itself and not user submitted*.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    public bool IsPrivate { get; set; } = false;

    /// <summary>
    /// Represents all of the tags that are associated with this link, as well as the number of times they've been used.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public TagEntity[] Tags { get; set; } = Array.Empty<TagEntity>();

    /// <summary>
    /// The title of the page, from the page itself, that the link points to.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    public string Url { get; set; }

    [BsonId]
    public Guid UserId { get; set; }

    private UserLinkEntity() { }

    public UserLinkEntity(Guid id, Guid userId, Guid linkId, string url = "", string title = "", string description = "", TagEntity[]? tags = default, bool isPrivate = default) : this()
    {
        //Id = Guid.NewGuid();


    }

    public static UserLinkEntity Create(string userId, string linkId)
    {
        var now = DateTimeOffset.UtcNow;

        return new UserLinkEntity
        {
            UserId = Guid.Parse(userId),
            LinkId = Guid.Parse(linkId),

            DateCreated = now,
            DateUpdated = now
        };
    }
}