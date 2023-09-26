using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.MediatR.Commands;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands.Handlers;

/// <summary>
/// Represents a MediatR command handler that adds a new link to the central repository.
/// </summary>
public class AddLinkCommandHandler : IRequestHandler<AddLinkCommand, Guid>
{
    private readonly ILinksService _linksService;

    public AddLinkCommandHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<Guid> Handle(AddLinkCommand command, CancellationToken cancellationToken)
    {
        var linkId = await _linksService.AddAsync(command.Link, cancellationToken);

        return linkId;
    }
}