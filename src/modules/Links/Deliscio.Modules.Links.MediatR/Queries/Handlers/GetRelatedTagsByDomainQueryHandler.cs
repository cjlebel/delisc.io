using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

public class GetRelatedTagsByDomainQueryHandler : IRequestHandler<GetRelatedTagsByDomainQuery, LinkTagDto[]>
{
    private readonly ILinksService _linksService;

    public GetRelatedTagsByDomainQueryHandler(ILinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<LinkTagDto[]> Handle(GetRelatedTagsByDomainQuery command, CancellationToken cancellationToken)
    {
        var results = (await _linksService.GetRelatedTagsByDomainAsync(command.Domain, command.Count.GetValueOrDefault(), token: cancellationToken)).ToArray();

        return results;
    }
}