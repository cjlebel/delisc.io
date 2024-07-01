namespace Deliscio.Modules.Links.Application.Dtos;

public class RelatedLinkDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Domain { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsFlagged { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public DateTimeOffset? DateUpdated { get; set; }

    public RelatedLinkDto(string id, string title, string domain, bool isActive, bool isDeleted, bool isFlagged, DateTimeOffset dateCreated, DateTimeOffset? dateUpdated)
    {
        Id = id;
        Title = title;
        Domain = domain;
        IsActive = isActive;
        IsDeleted = isDeleted;
        IsFlagged = isFlagged;
        DateCreated = dateCreated;
        DateUpdated = dateUpdated;
    }
}