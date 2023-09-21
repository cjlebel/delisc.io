using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;

namespace Deliscio.Modules.Links.Data.Entities;

/// <summary>
/// Represents a link that has been submitted to the system, but not yet processed.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("Link")]
[BsonCollection("links")]
public class LinkEntity : MongoEntityBase
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

    public bool IsActive { get; set; } = false;

    public bool IsFlagged { get; set; } = false;

    public bool IsProcessed { get; set; } = false;

    /// <summary>
    /// The number of times this link has been saved.
    /// </summary>
    public int SavesCount { get; set; } = 0;

    /// <summary>
    /// The number of times this link has been liked.
    /// </summary>
    public int LikesCount { get; set; } = 0;

    /// <summary>
    /// Represents all of the tags that are associated with this link, as well as the number of times they've been used.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public List<TagEntity> Tags { get; set; } = new List<TagEntity>();

    /// <summary>
    /// The URL of the link to go to the page
    /// </summary>
    public string Url { get; set; }

    public Guid CreatedByUserId { get; set; }

    private LinkEntity() { }

    public LinkEntity(string id, string url, string title) : this(Guid.Parse(id), url, title) { }

    public LinkEntity(Guid id, string url, string title)
    {
        Id = id;
        Title = title;
        Url = url;

        Tags = new List<TagEntity>();

        LikesCount = 0;
        SavesCount = 0;
        DateCreated = DateTimeOffset.UtcNow;
        DateUpdated = DateTimeOffset.UtcNow;
    }

    public LinkEntity(string url, string title, string description, string domain, bool hasScreenshot, string imageUrl, int savesCount, int likesCount, bool isProcessed, DateTimeOffset dateCreated, DateTimeOffset dateUpdated)
    {
        Title = title;
        Description = description;
        Domain = domain;
        HasScreenshot = hasScreenshot;
        ImageUrl = imageUrl;
        SavesCount = savesCount;
        LikesCount = likesCount;
        //Tags = tags;
        Url = url;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
    }

    /// <summary>
    /// Creates a NEW LinkEntity (one that doesn't already exist)
    /// </summary>
    /// <param name="url">The url to the page</param>
    /// <param name="title">The title of the page</param>
    /// <param name="createdById">The id of the user who submitted the link</param>
    /// <param name="tags">The optional tags that are associated with the link</param>
    /// <returns></returns>
    public static LinkEntity Create(string url, string title, Guid createdById, string[]? tags)
    {
        return new LinkEntity()
        {

            Tags = tags?.Select(x => new TagEntity(x)).ToList() ?? new List<TagEntity>(),
        };
    }
}