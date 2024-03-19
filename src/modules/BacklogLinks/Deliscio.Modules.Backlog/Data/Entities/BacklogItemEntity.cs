using System.ComponentModel.DataAnnotations.Schema;

using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;
using Deliscio.Core.Data;


namespace Deliscio.Modules.BackLog.Data.Entities;

/// <summary>
/// Represents a link that has been submitted to the system, but not yet processed.
/// </summary>
/// <seealso cref="EntityBase" />
[Table("Backlog")]
[BsonCollection("backlinks")]
public sealed class BacklogItemEntity : MongoEntityBase
{
    /// <summary>
    /// Gets or sets a value indicating whether this record has been processed after it's been saved.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is processed; otherwise, <c>false</c>.
    /// </value>
    public bool IsProcessed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this Link is excluded from results.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is excluded; otherwise, <c>false</c>.
    /// </value>
    public bool IsExcluded { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this Link is flagged for review.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is flagged; otherwise, <c>false</c>.
    /// </value>
    public bool IsFlagged { get; set; }

    /// <summary>
    /// Gets or sets the page title for the link.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the URL for the link.
    /// </summary>
    /// <value>
    /// The URL.
    /// </value>
    public string Url { get; set; }

    public BacklogItemEntity(string url, string title, string createById)
    {
        Title = title;
        Url = url;
        CreatedById = Guid.Parse(createById);
    }

    //public BackLinkEntity(string title, string url, string createById, bool isProcessed, IEnumerable<string>? tags)
    public BacklogItemEntity(string id, string url, string title, string createById)
    {
        Id = string.IsNullOrWhiteSpace(id) ? Guid.Empty : Guid.Parse(id);
        Title = title;
        Url = url;
        CreatedById = Guid.Parse(createById);
    }

    public static BacklogItemEntity Create(string url, string title, string createById)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        if (string.IsNullOrWhiteSpace(createById))
            throw new ArgumentNullException(nameof(createById));

        return new BacklogItemEntity(Guid.NewGuid().ToString(), url, title, createById)
        {
            IsProcessed = false,
            DateCreated = DateTimeOffset.UtcNow,
            DateUpdated = DateTimeOffset.UtcNow,
        };
    }
}