using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Interfaces;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Deliscio.Modules.Links.Infrastructure.Data.Entities;

/// <summary>
/// Represents a link that has been submitted to the central repository.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("Link")]
[BsonCollection("links")]
public sealed class LinkEntity : MongoEntityBase, IIsSoftDeletableBy<ObjectId>
{
    /// <summary>
    /// The title of the page, from the page itself, that the link points to.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// The description/summary of the page that the link points to.
    /// This is retrieved/built from the page itself and not user submitted*.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The domain of the link. If the link is a sub-domain (excluding www or similar), then the sub-domain will be returned.
    /// </summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Whether this link has a screenshot.
    /// If it does, then the name of the file will be the same as the ID of the link*.
    /// </summary>
    public bool HasScreenshot { get; set; } = false;

    /// <summary>
    /// The full URL of the image that is associated with this link.
    /// This is retrieved from the page itself, if it exists.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// These are the keywords that were retrieved from the page itself
    /// </summary>
    public string[] Keywords { get; set; } = Array.Empty<string>();

    public bool IsActive { get; set; } = true;

    public bool IsFlagged { get; set; } = false;

    /// <summary>
    /// The number of times this link has been saved.
    /// </summary>
    public int SavesCount { get; set; }

    /// <summary>
    /// The number of times this link has been liked.
    /// </summary>
    public int LikesCount { get; set; }

    /// <summary>
    /// Represents all the tags that are associated with this link, as well as the number of times they've been used.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public LinkTagEntity[] Tags { get; set; } = [];

    /// <summary>
    /// The URL of the link to go to the page
    /// </summary>
    public string Url { get; set; }

    [Obsolete("Use CreatedByUserId")]
    public ObjectId SubmittedById { get; set; }

    public bool IsDeleted { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset? DateDeleted { get; set; }

    public ObjectId DeletedById { get; set; }

    //public LinkEntity(ObjectId linkId, string url, string title, string createdById, string[]? tags = null)
    //{
    //    LinkId = linkId;
    //    CreatedByUserId = ObjectId.Parse(createdById);
    //    //Keeping this here until the existing data can be updated
    //    CreatedByUserId = ObjectId.Parse(createdById);

    //    Title = title;
    //    Url = url;

    //    TagsCollection = tags?.Select(x => new LinkTagEntity(x)).ToArray() ?? [];
    //}
}