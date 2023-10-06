using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinksByIdsQuery : IRequest<IEnumerable<LinkItem>>
{
    public Guid[] Ids { get; init; }

    public GetLinksByIdsQuery(Guid[] ids)
    {
        Guard.Against.NullOrEmpty(ids);

        Ids = ids;
    }

    public GetLinksByIdsQuery(string[] ids)
    {
        Guard.Against.NullOrEmpty(ids);

        Ids = ids.Select(Guid.Parse).ToArray();
    }
}