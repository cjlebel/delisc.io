namespace Deliscio.Apis.WebApi.Common.Requests;

public sealed record AddUserLinkRequest
{
    public string LinkId { get; set; }

    public bool IsPrivate { get; set; }

    public string[] Tags { get; set; } = Array.Empty<string>();

    public string Title { get; set; } = string.Empty;
}