using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands.Handlers;

/// <summary>
/// Represents a MediatR command handler that adds a new link to the central repository.
/// </summary>
public class AddLinkCommandHandler : IRequestHandler<AddLinkCommand, Guid>
{
    private readonly ILinksService _service;

    public AddLinkCommandHandler(ILinksService service)
    {
        _service = service;
    }

    public async Task<Guid> Handle(AddLinkCommand command, CancellationToken cancellationToken)
    {
        var linkId = await _service.AddAsync(command.Link, cancellationToken);

        return linkId;
    }
}