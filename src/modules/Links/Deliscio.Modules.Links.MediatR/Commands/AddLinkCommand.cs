using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands;

/// <summary>
/// Represents a command to save the link to the central repository
/// </summary>
public sealed record AddLinkCommand : IRequest<Guid>
{
    public string Url { get; }
    public string Title { get; }
    public Guid SubmittedById { get; }
    public string[] Tags { get; }

    public AddLinkCommand(string url, string title, Guid submittedById, string[]? tags = default)
    {
        Url = url;
        Title = title;
        SubmittedById = submittedById;
        Tags = tags ?? Array.Empty<string>();
    }
}