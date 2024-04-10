using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Models.Requests;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record FindLinksQuery : IRequest<PagedResults<LinkItem>>
{
    public FindLinksRequest Request { get; init; }


    public FindLinksQuery(FindLinksRequest request)
    {
        Request = request;
    }
}

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class FindLinksQueryHandler : IRequestHandler<FindLinksQuery, PagedResults<LinkItem>>
{
    private readonly ILinksService _linksService;

    public FindLinksQueryHandler(ILinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<PagedResults<LinkItem>> Handle(FindLinksQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.FindAsync(command.Request, token: cancellationToken);

        return results;
    }
}

public sealed record FindLinksAdminQuery : IRequest<PagedResults<LinkItem>>
{
    public FindLinksAdminRequest Request { get; init; }

    public FindLinksAdminQuery(FindLinksAdminRequest request)
    {
        Request = request;
    }
}

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class FindLinksAdminQueryHandler : IRequestHandler<FindLinksAdminQuery, PagedResults<LinkItem>>
{
    private readonly ILinksAdminService _linksService;

    public FindLinksAdminQueryHandler(ILinksAdminService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<PagedResults<LinkItem>> Handle(FindLinksAdminQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.FindAsync(command.Request, token: cancellationToken);

        if (results is null)
            return new PagedResults<LinkItem>();

        return results;
    }
}
