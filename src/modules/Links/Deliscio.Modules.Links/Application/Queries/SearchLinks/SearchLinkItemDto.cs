using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Domain.LinkTags;

namespace Deliscio.Modules.Links.Application.Queries.SearchLinks;

public sealed record SearchLinkItemDto
{
    public string Id { get; init; }

    public string Description { get; init; }

    public string Domain { get; init; }

    public string ImageUrl { get; init; }

    public IReadOnlyCollection<LinkTagDto> TagCollection { get; set; }

    public string Title { get; init; }

    public string Url { get; init; }


    public int Likes { get; init; }

    public int Saves { get; init; }


    public bool IsActive { get; init; }

    public bool IsFlagged { get; init; }

    public bool IsDeleted { get; init; }


    public DateTimeOffset DateCreated { get; init; }

    public string CreatedById { get; init; }

    public DateTimeOffset? DateUpdated { get; set; }
    
    public string UpdatedById { get; set; }

    public SearchLinkItemDto(LinkId id, 
        LinkUrl url, 
        LinkTitle title, 
        LinkDescription description, 
        LinkDomain domain, 
        LinkImageUrl imageUrl, 
        LinkTagCollection tags, 
        int likes, 
        int saves, 
        bool isActive, 
        bool isFlagged, 
        bool isDeleted,
        DateTimeOffset dateCreated,
        CreatedById createdById,
        DateTimeOffset? dateUpdated,
        UpdatedById? updatedById)
    {
        Id = id.Value.ToString();
        Url = url.Value;
        Title = title.Value;
        Description = description.Value;
        Domain = domain.Value;
        ImageUrl = imageUrl.Value;
        TagCollection = tags.Tags.Select(t=>new LinkTagDto(t.Name, t.Count, t.Weight)).ToList();

        Likes = likes;
        Saves = saves;

        IsActive = isActive;
        IsFlagged = isFlagged;
        IsDeleted = isDeleted;

        DateCreated = dateCreated;
        CreatedById = createdById?.Value.ToString() ?? string.Empty;

        DateUpdated = dateUpdated;
        UpdatedById = updatedById?.Value.ToString() ?? string.Empty;
    }

    public static explicit operator SearchLinkItemDto(Link link)
    {
        if (link == null)
        {
            throw new ArgumentNullException(nameof(link));
        }

        return new SearchLinkItemDto
        (
            link.Id,
            link.Url,
            link.Title,
            link.Description,
            link.Domain,
            link.ImageUrl,
            link.TagsCollection,

            link.LikesCount,
            link.SavesCount,

            link.IsActive,
            link.IsFlagged,
            link.IsDeleted,

            link.DateCreated,
            link.CreatedById,

            link.DateUpdated,
            link.UpdatedById
        );
    }
}