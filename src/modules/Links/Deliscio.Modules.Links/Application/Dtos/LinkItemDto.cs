using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Domain.LinkTags;

namespace Deliscio.Modules.Links.Application.Dtos;

public sealed record LinkItemDto
{
    public string Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Domain { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public int TotalLikes { get; set; }

    public int TotalSaves { get; set; }

    public List<LinkTagDto> Tags { get; set; } = [];

    public string Title { get; set; }

    public string Url { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsFlagged { get; set; }

    public string CreatedByUserId { get; set; } = Guid.Empty.ToString();

    public DateTimeOffset DateCreated { get; set; }

    public string UpdatedByUserId { get; set; } = Guid.Empty.ToString();

    public DateTimeOffset? DateUpdated { get; set; }

    // Needed for deserialization
    public LinkItemDto() { }

    public LinkItemDto(LinkId id,
        LinkTitle title,
        LinkDescription description,
        LinkDomain domain,
        LinkImageUrl imageUrl,
        LinkTagCollection tagsCollection,
        LinkUrl url,
        IReadOnlyCollection<string> keywords,
        int totalLikes,
        int totalSaves,
        bool isActive,
        bool isFlagged,
        bool isDeleted,
        DateTimeOffset dateCreated,
        CreatedById createdByUserId,
        DateTimeOffset? dateUpdated = null,
        UpdatedById? updateByUserId = null,
        DateTimeOffset? dateDeleted = null,
        DeletedById? deletedByUserId = null)
    {
        Id = id.Value.ToString();
        Title = title.Value;
        Description = description.Value;
        Domain = domain.Value;
        ImageUrl = imageUrl.Value;
        Tags = tagsCollection.Tags.Select(t => (LinkTagDto)t).ToList();
        Url = url.Value;
        TotalLikes = totalLikes;
        TotalSaves = totalSaves;
        IsActive = isActive;
        IsFlagged = isFlagged;
        IsDeleted = isDeleted;
        DateCreated = dateCreated;
        CreatedByUserId = createdByUserId.Value.ToString();
        DateUpdated = dateUpdated;
        UpdatedByUserId = updateByUserId?.Value.ToString() ?? string.Empty;
    }

    public static explicit operator LinkItemDto(Link link)
    {
        if (link is null)
            throw new ArgumentNullException(nameof(link));

        return new LinkItemDto
        (
            link.Id,
            link.Title,
            link.Description,
            link.Domain,
            link.ImageUrl,
            link.TagsCollection,
            link.Url,
            link.Keywords,
            link.LikesCount,
            link.SavesCount,
            link.IsActive,
            link.IsFlagged,
            link.IsDeleted,
            link.DateCreated,
            link.CreatedById,
            link.DateUpdated,
            link.UpdatedById,
            link.DateDeleted,
            link.DeletedById
        );
    }
}