namespace Deliscio.Modules.Links.Common.Models;

public sealed record LinkItem
{
    public string Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Domain { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public int Likes { get; set; }

    public int Saves { get; set; }

    public List<LinkTag> Tags { get; set; } = new();

    public string Title { get; set; }

    public string Url { get; set; }

    // These "Is" properties are temporary. Needs to refactor services/models to accommodate Admin
    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsFlagged { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset? DateUpdated { get; set; }

    // Needed for deserialization
    public LinkItem() { }

    //public LinkItem(Guid id, string url, string title, string description, string domain, string imageUrl,
    //    IEnumerable<LinkTag>? tags, DateTimeOffset dateCreated, DateTimeOffset dateUpdated) : this(id.ToString(), url, title, description, domain,
    //    imageUrl, tags, dateCreated, dateUpdated)
    //{ }

    public LinkItem(string id, string url, string title, string description, string domain, string imageUrl, IEnumerable<LinkTag>? tags, DateTimeOffset dateCreated, DateTimeOffset? dateUpdated)
    {
        Id = id;
        Description = description;
        Domain = domain;
        ImageUrl = imageUrl;
        Tags = tags?.ToList() ?? new List<LinkTag>();
        Title = title;
        Url = url;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
    }
}