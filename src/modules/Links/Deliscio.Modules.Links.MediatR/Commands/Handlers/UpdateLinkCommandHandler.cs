using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands.Handlers;

/// <summary>
/// Represents a MediatR command handler that updates an existing Link, based on the information from a Queued Link
/// </summary>
public class UpdateLinkCommandHandler : IRequestHandler<AddLinkCommand, Guid>
{
    private readonly ILinksService _service;

    public UpdateLinkCommandHandler(ILinksService service)
    {
        _service = service;
    }

    public async Task<Guid> Handle(AddLinkCommand command, CancellationToken cancellationToken)
    {
        var linkId = await _service.AddAsync(command.Link, cancellationToken);

        return linkId;
    }
}