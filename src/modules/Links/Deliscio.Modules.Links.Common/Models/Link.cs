using System.Collections.ObjectModel;

namespace Deliscio.Modules.Links.Common.Models;

public class Link
{
    public string Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image for the site/page that the link belongs to.
    /// </summary>
    /// <value>
    /// The image.
    /// </value>
    public string ImageUrl { get; set; } = string.Empty;

    public string[] Keywords { get; set; } = Array.Empty<string>();

    public ReadOnlyCollection<LinkTag> Tags { get; set; }

    public string Title { get; set; }

    public string Url { get; set; }

    public string SubmittedById { get; set; } = Guid.Empty.ToString();

    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    public Link(string id, string url, string submittedById)
    {
        Id = id;
        Title = string.Empty;
        Url = url;
        SubmittedById = submittedById;
        Tags = new ReadOnlyCollection<LinkTag>(new List<LinkTag>());
    }

    public Link(Guid id, string url, Guid submittedById)
    {
        Id = id.ToString();
        Title = string.Empty;
        Url = url;
        SubmittedById = submittedById.ToString();
        Tags = new ReadOnlyCollection<LinkTag>(new List<LinkTag>());
    }

    public Link(Guid id, string url, string title, string description, IEnumerable<LinkTag>? tags)
        : this(id.ToString(), url, title, description, tags)
    {
    }

    public Link(string id, string url, string title, string description, IEnumerable<LinkTag>? tags)
    {
        Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
        Title = title;
        Description = description;
        Url = url;
        Tags = new ReadOnlyCollection<LinkTag>((tags?.ToList() ?? new List<LinkTag>()));
    }

    public static Link Create(string url, string submittedById)
    {
        return Create(url, submittedById, string.Empty, string.Empty, Enumerable.Empty<string>());
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
    public static Link Create(string url, string submittedById, string title, string description, IEnumerable<string>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        var arrTags = tags as string[] ?? Array.Empty<string>();

        return new Link(Guid.Empty, url, title, description, (arrTags.Select(LinkTag.Create)))
        {
            SubmittedById = submittedById,
            DateCreated = DateTimeOffset.UtcNow,
            DateUpdated = DateTimeOffset.UtcNow,
        };
    }
}