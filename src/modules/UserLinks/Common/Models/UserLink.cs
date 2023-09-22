namespace Deliscio.Modules.UserLinks.Common.Models;

public sealed record UserLink
{
    public string Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public bool IsPrivate { get; set; } = false;

    public UserLinkTag[] Tags { get; set; } = Array.Empty<UserLinkTag>();

    public string Title { get; set; }

    public string Url { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset DateUpdated { get; set; }

    public UserLink(string id, string url, string title, string description, DateTimeOffset dateCreated, DateTimeOffset dateUpdated, UserLinkTag[]? tags = default, bool isPrivate = false)
    {
        Id = id;
        Url = url;
        Title = title;
        Description = description;
        Tags = tags ?? Array.Empty<UserLinkTag>();
        IsPrivate = isPrivate;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
    }
}