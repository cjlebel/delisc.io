namespace Deliscio.Modules.Links.Common.Models.Requests;

public class LinkEditRequest
{
    public required string Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string[]? Tags { get; set; } = Array.Empty<string>();

    public bool IsActive { get; set; }

    public bool? IsFlagged { get; set; }

    public bool? IsDeleted { get; set; }

    public LinkEditRequest() { }

    public LinkEditRequest(string id, string title, string description, bool isActive = false, string[]? tags = default)
    {
        Id = id;
        IsActive = isActive;
        Title = title;
        Description = description;
        Tags = tags;
    }
}