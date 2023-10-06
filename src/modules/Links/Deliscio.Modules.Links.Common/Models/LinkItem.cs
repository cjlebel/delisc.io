namespace Deliscio.Modules.Links.Common.Models;

public sealed record LinkItem
{
    public string Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Domain { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public List<LinkTag> Tags { get; set; } = new List<LinkTag>();

    public string Title { get; set; }

    public string Url { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    // Needed for deserialization
    public LinkItem() { }

    public LinkItem(Guid id, string url, string title, string description, string domain, string imageUrl,
        IEnumerable<LinkTag>? tags, DateTimeOffset dateCreated) : this(id.ToString(), url, title, description, domain,
        imageUrl, tags, dateCreated)
    { }

    public LinkItem(string id, string url, string title, string description, string domain, string imageUrl, IEnumerable<LinkTag>? tags, DateTimeOffset dateCreated)
    {
        Id = id;
        Description = description;
        Domain = domain;
        ImageUrl = imageUrl;
        Tags = tags?.ToList() ?? new List<LinkTag>();
        Title = title;
        Url = url;
        DateCreated = dateCreated;
    }
}