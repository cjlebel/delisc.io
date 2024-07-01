using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Commands;

/// <summary>
/// Represents a command to save the link to the central repository
/// </summary>
public sealed record AddLinkCommand : IRequest<string>
{
    public LinkDto Link { get; }

    public AddLinkCommand(LinkDto link)
    {
        Link = link;
    }
}

/// <summary>
/// Represents a MediatR command handler that adds a new link to the central repository.
/// This differs from SubmitLink, as this saves the Link to the central repo, whereas Submit adds it to be verified prior to adding.
/// </summary>
public class AddLinkCommandHandler : IRequestHandler<AddLinkCommand, string>
{
    private readonly ILinksService _service;

    public AddLinkCommandHandler(ILinksService service)
    {
        _service = service;
    }

    public async Task<string> Handle(AddLinkCommand command, CancellationToken cancellationToken)
    {
        var linkId = await _service.AddAsync(command.Link, cancellationToken);

        return linkId;
    }
}