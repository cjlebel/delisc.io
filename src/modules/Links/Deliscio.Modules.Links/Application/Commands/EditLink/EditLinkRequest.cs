namespace Deliscio.Modules.Links.Application.Commands.EditLink;

public class EditLinkRequest
{
    public required string LinkId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string[]? Tags { get; set; } = Array.Empty<string>();

    public EditLinkRequest() { }

    public EditLinkRequest(string linkId, string title, string description, string[]? tags = default)
    {
        LinkId = linkId;
        Title = title;
        Description = description;
        Tags = tags;
    }
}