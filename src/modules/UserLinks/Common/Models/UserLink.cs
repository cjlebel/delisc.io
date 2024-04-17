namespace Deliscio.Modules.UserLinks.Common.Models;

public sealed record UserLink
{
    /// <summary>
    /// Id is different from the LinkId, even though it's a 1-to-1.
    /// This is actually by design and serves a few purposes
    /// </summary>
    public string Id { get; set; }

    public string UserId { get; set; }

    public string LinkId { get; set; }

    //public string Description { get; set; } = string.Empty;

    public bool IsPrivate { get; set; } = false;

    public UserLinkTag[] Tags { get; set; } = Array.Empty<UserLinkTag>();

    public string Title { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset? DateUpdated { get; set; }

    public UserLink() { }

    /// <summary>
    /// Creates an instance of the UserLink class, based on existing data.
    /// For new UserLinks that don't yet exist in the database, use the Create method.
    /// </summary>
    /// <param name="id">The unique id of this user link</param>
    /// <param name="linkId">The unique id of the underlying link that this points to</param>
    /// <param name="userId">The unique id of the user who wants to associate with the underlying link</param>
    /// <param name="title">The title for this link that this used will see. This is optional, and will not change the underlying link's title</param>
    /// <param name="dateCreated">The date when the already existing link was created</param>
    /// <param name="dateUpdated">The date when the already existing link was last updated</param>
    /// <param name="tags">One or more tags that the user would like to associate with their user link. This is optional, and will not change the underlying link's title</param>
    /// <param name="isPrivate">Whether or not the user would like to hide their link from anyone other than themselves</param>
    public UserLink(string id, string linkId, string userId, string title, DateTimeOffset dateCreated, DateTimeOffset? dateUpdated, UserLinkTag[]? tags = default, bool isPrivate = false)
    {
        Id = id;
        UserId = userId;
        LinkId = linkId;
        Title = title;
        //Description = description;
        Tags = tags ?? Array.Empty<UserLinkTag>();
        IsPrivate = isPrivate;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
    }

    public static UserLink Create(string linkId, string userId, string title = "", UserLinkTag[]? tags = default, bool isPrivate = false)
    {
        return new UserLink
        {
            UserId = userId,
            LinkId = linkId,
            Title = title,
            Tags = tags ?? Array.Empty<UserLinkTag>(),
            IsPrivate = isPrivate,
            DateCreated = DateTimeOffset.UtcNow,
            DateUpdated = DateTimeOffset.UtcNow
        };
    }
}