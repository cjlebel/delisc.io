using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Domain.LinkTags;

namespace Deliscio.Modules.Links.Application.Dtos;

public sealed record LinkDto
{
    public string Id { get; init; }

    public string Url { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public string Domain { get; init; }

    public string ImageUrl { get; init; }

    public IReadOnlyCollection<LinkTagDto> TagCollection { get; set; }

    public IReadOnlyCollection<string> Keywords { get; set; }


    public int TotalLikes { get; init; }

    public int TotalSaves { get; init; }


    public bool IsActive { get; init; }

    public bool IsFlagged { get; init; }

    public bool IsDeleted { get; init; }
    public DateTimeOffset? DateDeleted { get; init; }
    public string DeletedByUserId { get; init; }


    public DateTimeOffset? DateUpdated { get; init; }
    public string UpdatedByUserId { get; init; }

    //?.ToLocalTime().DateTime.ToString(CultureInfo.InvariantCulture)


    public DateTimeOffset DateCreated { get; init; }
    public string CreatedByUserId { get; init; }


    public LinkDto(LinkId id,
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
        TagCollection = tagsCollection.Tags.Select(t=> (LinkTagDto)t).ToList();
        Url = url.Value;
        Keywords = keywords;
        TotalLikes = totalLikes;
        TotalSaves = totalSaves;
        IsActive = isActive;
        IsFlagged = isFlagged;
        IsDeleted = isDeleted;
        DateCreated = dateCreated;
        CreatedByUserId = createdByUserId.Value.ToString();
        DateUpdated = dateUpdated;
        UpdatedByUserId = updateByUserId?.Value.ToString() ?? string.Empty;
        DateDeleted = dateDeleted;
        DeletedByUserId = deletedByUserId?.Value.ToString() ?? string.Empty;
    }

    public static explicit operator LinkDto(Link link)
    {
        if (link is null)
            throw new ArgumentNullException(nameof(link));

        return new LinkDto
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