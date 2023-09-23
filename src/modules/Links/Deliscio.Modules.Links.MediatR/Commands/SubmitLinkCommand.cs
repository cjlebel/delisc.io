using Ardalis.GuardClauses;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands;

/// <summary>
/// Represents a command to submit a link for verification, before attempting to add to the central repository
/// </summary>
public sealed record SubmitLinkCommand : IRequest<Guid>
{
    public string[] Tags { get; }

    public string Url { get; }

    public Guid SubmittedById { get; }

    public SubmitLinkCommand(string url, string submittedById, string[]? tags = default) : this(url, new Guid(submittedById), tags) { }

    public SubmitLinkCommand(string url, Guid submittedById, string[]? tags = default)
    {
        Guard.Against.NullOrEmpty(url);
        Guard.Against.NullOrEmpty(submittedById);

        SubmittedById = submittedById;
        Tags = tags ?? Array.Empty<string>();
        Url = url;
    }
}