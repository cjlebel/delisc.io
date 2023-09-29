using Deliscio.Modules.UserLinks.Common.Interfaces;
using Deliscio.Modules.UserLinks.Common.Models;
using MediatR;

namespace Deliscio.Modules.UserLinks.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a single link from the central repository, by the link's id
/// </summary>
public class GetUserLinkByIdQueryHandler : IRequestHandler<GetUserLinkByIdQuery, UserLink?>
{
    private readonly IUserLinksService _service;

    public GetUserLinkByIdQueryHandler(IUserLinksService service)
    {
        _service = service;
    }

    public async Task<UserLink?> Handle(GetUserLinkByIdQuery command, CancellationToken cancellationToken)
    {
        var link = await _service.GetAsync(command.UserId, command.LinkId, cancellationToken);

        return link;
    }
}