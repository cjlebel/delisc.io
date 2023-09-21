using Ardalis.GuardClauses;
using MediatR;

namespace Deliscio.Modules.Links.Common.Commands;

public sealed record SubmitLinkCommand : IRequest<bool>
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