using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinksByIdsQuery : IRequest<IEnumerable<LinkItemDto>>
{
    public string[] Ids { get; init; }

    public GetLinksByIdsQuery(string[] ids)
    {
        Guard.Against.NullOrEmpty(ids);

        Ids = ids;
    }
}