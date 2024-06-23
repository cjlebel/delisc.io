using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands;

/// <summary>
/// Represents a command to submit a link for verification, before attempting to add to the central repository
/// </summary>
public sealed record SubmitLinkByUserCommand : IRequest<string>
{
    public string[] Tags { get; }

    public string Url { get; }

    public string SubmittedById { get; }

    public SubmitLinkByUserCommand(string url, string submittedById, string[]? tags = default)
    {
        Guard.Against.NullOrEmpty(url);
        Guard.Against.NullOrEmpty(submittedById);

        SubmittedById = submittedById;
        Tags = tags ?? Array.Empty<string>();
        Url = url;
    }
}

/// <summary>
/// Represents a MediatR command handler that submits a link for verification.
/// </summary>
public sealed class SubmitLinkByUserCommandHandler : IRequestHandler<SubmitLinkByUserCommand, string>
{
    private readonly ILinksService _linksService;

    public SubmitLinkByUserCommandHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<string> Handle(SubmitLinkByUserCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        var linkId = await _linksService.SubmitLinkAsync(command.Url, command.SubmittedById, command.Tags, cancellationToken);

        return linkId;
    }
}