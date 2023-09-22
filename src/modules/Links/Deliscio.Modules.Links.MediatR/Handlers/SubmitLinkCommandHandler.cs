using Deliscio.Modules.Links.Common.Commands;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Handlers;

public class SubmitLinkCommandHandler : IRequestHandler<SubmitLinkCommand, Guid>
{
    private readonly ILinksService _linksService;

    public SubmitLinkCommandHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<Guid> Handle(SubmitLinkCommand command, CancellationToken cancellationToken)
    {
        var link = await _linksService.SubmitLinkAsync(command.Url, command.SubmittedById, command.Tags, cancellationToken);

        return link;
    }
}