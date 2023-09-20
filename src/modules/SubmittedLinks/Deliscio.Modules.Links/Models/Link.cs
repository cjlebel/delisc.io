using System.Collections.ObjectModel;

namespace Deliscio.Modules.Links.Models;

public class Link
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
    /// Gets or sets a value indicating whether this Link is excluded from results.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is excluded; otherwise, <c>false</c>.
    /// </value>
    public bool IsExcluded { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this Link is flagged.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is flagged; otherwise, <c>false</c>.
    /// </value>
    public bool IsFlagged { get; set; } = false;

    public ReadOnlyCollection<Tag> Tags { get; set; }

    public string Title { get; set; }

    public string Url { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    public Link(string id, string url, string createdById)
    {
        Id = id;
        Title = string.Empty;
        Url = url;
        CreatedById = createdById;
        Tags = new ReadOnlyCollection<Tag>(new List<Tag>());
    }

    public Link(Guid id, string url, Guid createdById)
    {
        Id = id.ToString();
        Title = string.Empty;
        Url = url;
        CreatedById = createdById.ToString();
        Tags = new ReadOnlyCollection<Tag>(new List<Tag>());
    }

    public Link(Guid id, string url, string title, string description, IEnumerable<Tag>? tags)
        : this(id.ToString(), url, title, description, tags)
    {
    }

    public Link(string id, string url, string title, string description, IEnumerable<Tag>? tags)
    {
        Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
        Title = title;
        Description = description;
        Url = url;
        Tags = new ReadOnlyCollection<Tag>((IList<Tag>)(tags ?? Enumerable.Empty<Tag>()));
    }

    public Link Create(string url)
    {
        return Create(url, string.Empty, string.Empty, Enumerable.Empty<string>());
    }

    /// <summary>
    /// Creates a instance of a NEW Link.
    /// </summary>
    /// <param name="url">The URL that the Link points to.</param>
    /// <param name="title">The title of the page for the Link.</param>
    /// <param name="description">The description of the Link.</param>
    /// <param name="tags">The tags that are associated with the Link.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">
    /// url
    /// or
    /// title
    /// or
    /// tags
    /// </exception>
    public Link Create(string url, string title, string description, IEnumerable<string>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        var arrTags = tags as string[] ?? Array.Empty<string>();

        return new Link(Guid.Empty, url, title, description, (arrTags.Select(Tag.Create)))
        {
            DateCreated = DateTimeOffset.UtcNow,
            DateUpdated = DateTimeOffset.UtcNow,
        };
    }
}