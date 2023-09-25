namespace Deliscio.Modules.BackLog.Models;

/// <summary>
/// Represents a link that has been submitted to be processed.
/// </summary>
public sealed class BacklogItem
{
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this link has been processed.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is processed; otherwise, <c>false</c>.
    /// </value>
    public bool IsProcessed { get; set; } = false;

    /// <summary>
    /// Gets or sets a collection of tags that were either given by the user when they submitted the link, or was generated when the link was processed
    /// </summary>
    /// <value>
    /// The collection of tags asscoiated with this link.
    /// </value>
    ///public IEnumerable<string> Tags { get; set; }
    /// <summary>
    /// Gets or sets the title of the page for this link.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>

    public string Title { get; set; }

    public string Url { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset DateUpdated { get; set; }

    public string CreatedById { get; set; }

    //public BackLink(string title, string url, createdById, IEnumerable<string>? tags, bool isProcessed = false)

    public BacklogItem(string url, string title, string createdById, bool isProcessed = false)
    {
        Title = title;
        Url = url;
        IsProcessed = isProcessed;
        CreatedById = createdById;
    }

    public BacklogItem(Guid id, string url, string title, Guid createdById, DateTimeOffset dateCreated,
        DateTimeOffset dateUpdated, bool isProcessed = false) : this(id.ToString(), url, title, createdById.ToString(),
        dateCreated, dateUpdated, isProcessed)
    {
    }

    public BacklogItem(string id, string url, string title, string createdById, DateTimeOffset dateCreated,
        DateTimeOffset dateUpdated, bool isProcessed = false)
    {
        if (!Guid.TryParse(id, out var newId))
            throw new ArgumentNullException(nameof(id));

        if (!Guid.TryParse(id, out var newCreatedById) && newCreatedById == Guid.Empty)
            throw new ArgumentNullException(nameof(createdById));

        Id = newId.ToString();
        Title = title;
        Url = url;
        CreatedById = createdById;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
    }

    /// <summary>
    /// Creates a instance of a NEW BackLink (Id is 0).
    /// </summary>
    /// <param name="title">The title of the link</param>
    /// <param name="url">The url of the link</param>
    /// <param name="createdById">The id of the user who is creating the link</param>
    /// <returns>A an instance of a New backlog item</returns>
    public static BacklogItem Create(string url, string title, string createdById)
    {
        return Create(url, title, Guid.Parse(createdById));
    }

    /// <summary>
    /// Creates a instance of a NEW BackLink (Id is 0).
    /// </summary>
    /// <param name="title">The title of the link</param>
    /// <param name="url">The url of the link</param>
    /// <param name="createdById">The id of the user who is creating the link</param>
    /// <returns>A an instance of a New backlog item</returns>
    public static BacklogItem Create(string url, string title, Guid createdById)
    {
        return new BacklogItem(Guid.Empty, url, title, createdById, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, false);
    }
}