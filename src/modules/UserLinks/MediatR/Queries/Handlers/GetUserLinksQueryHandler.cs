using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.UserLinks.Common.Interfaces;
using Deliscio.Modules.UserLinks.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class GetUserLinksQueryHandler : IRequestHandler<GetUserLinksQuery, PagedResults<UserLink>>
{
    private readonly IUserLinksService _linksService;

    public GetUserLinksQueryHandler(IUserLinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<PagedResults<UserLink>> Handle(GetUserLinksQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetAsync(command.UserId, command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}