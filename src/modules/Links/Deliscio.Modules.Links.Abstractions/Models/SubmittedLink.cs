using System.Collections.ObjectModel;

namespace Deliscio.Modules.SubmittedLinks.Abstractions.Models;

public class SubmittedLink
{
    public string Id { get; set; }

    public string? Description { get; set; }

    public string? Domain { get; set; }

    /// <summary>
    /// Gets or sets the image for the site/page that the link belongs to.
    /// </summary>
    /// <value>
    /// The image.
    /// </value>
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this SubmittedLink is excluded from results.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is excluded; otherwise, <c>false</c>.
    /// </value>
    public bool IsExcluded { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this SubmittedLink is flagged.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is flagged; otherwise, <c>false</c>.
    /// </value>
    public bool IsFlagged { get; set; } = false;

    public ReadOnlyDictionary<string, int> Tags { get; set; }

    public string Title { get; set; }

    public string Url { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    public SubmittedLink(Guid id, string url, string title, string description, IDictionary<string, int> tags)
        : this(id.ToString(), url, title, description, tags)
    {
    }

    public SubmittedLink(string id, string url, string title, string description, IDictionary<string, int> tags)
    {
        Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
        Title = title;
        Description = description ?? string.Empty;
        Url = url;
        Tags = new ReadOnlyDictionary<string, int>(tags);
    }

    /// <summary>
    /// Creates a instance of a NEW SubmittedLink.
    /// </summary>
    /// <param name="url">The URL that the SubmittedLink points to.</param>
    /// <param name="title">The title of the page for the SubmittedLink.</param>
    /// <param name="description">The description of the SubmittedLink.</param>
    /// <param name="tags">The tags that are associated with the SubmittedLink.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">
    /// url
    /// or
    /// title
    /// or
    /// tags
    /// </exception>
    public SubmittedLink Create(string url, string title, string description, IDictionary<string, int> tags)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        if (tags == null)
            throw new ArgumentNullException(nameof(tags));

        return new SubmittedLink(Guid.Empty, url, title, description, tags)
        {
            DateCreated = DateTimeOffset.UtcNow,
            DateUpdated = DateTimeOffset.UtcNow,
        };
    }
}