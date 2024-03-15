namespace Deliscio.Apis.WebApi.Common.Requests;

public sealed record AddUserLinkRequest
{
    public string LinkId { get; set; } = string.Empty;

    public bool IsPrivate { get; set; }

    public string[] Tags { get; set; } = [];

    public string Title { get; set; } = string.Empty;
}