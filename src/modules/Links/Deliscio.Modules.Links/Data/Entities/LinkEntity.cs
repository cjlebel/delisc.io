using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Interfaces;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Data.Entities;

/// <summary>
/// Represents a link that has been submitted to the central repository.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("Link")]
[BsonCollection("links")]
public sealed class LinkEntity : MongoEntityBase, IIsSoftDeletableBy<Guid>
{
    /// <summary>
    /// The title of the page, from the page itself, that the link points to.
    /// </summary>
    public string Title { get; set; }

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
    /// Whether or not this link has a screen shot.
    /// If it does, then the name of the file will be the same as the ID of the link*.
    /// </summary>
    public bool HasScreenshot { get; set; } = false;

    /// <summary>
    /// The full URL of the image that is associated with this link.
    /// This is retrieved from the page itself, if it exists.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    public string[] Keywords { get; set; } = Array.Empty<string>();

    public bool IsActive { get; set; }

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
    /// Represents all of the tags that are associated with this link, as well as the number of times they've been used.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public List<LinkTagEntity> Tags { get; set; } = new();

    /// <summary>
    /// The URL of the link to go to the page
    /// </summary>
    public string Url { get; set; }

    public Guid SubmittedById { get; set; }

    public bool IsDeleted { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTimeOffset DateDeleted { get; set; }

    public Guid DeletedById { get; set; }

    private LinkEntity()
    {

    }


    public LinkEntity(Guid id, string url, string title, Guid submittedById)
    {
        Id = id;
        SubmittedById = submittedById;
        Title = title;
        Url = url;

        Tags = new List<LinkTagEntity>();

        LikesCount = 0;
        SavesCount = 0;
        DateCreated = DateTimeOffset.UtcNow;
        DateUpdated = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a NEW LinkEntity (one that doesn't already exist)
    /// </summary>
    /// <param name="url">The url to the page</param>
    /// <param name="title">The title of the page</param>
    /// <param name="submittedById">The id of the user who submitted the link</param>
    /// <param name="tags">The optional tags that are associated with the link</param>
    /// <returns></returns>
    public static LinkEntity Create(string url, string title, Guid submittedById, string[]? tags)
    {
        var now = DateTimeOffset.UtcNow;

        return new LinkEntity
        {
            IsActive = true,
            SubmittedById = submittedById,
            Tags = tags?.Select(x => new LinkTagEntity(x)).ToList() ?? new List<LinkTagEntity>(),
            Title = title,
            Url = url,

            DateCreated = now,
            DateUpdated = now
        };
    }
}