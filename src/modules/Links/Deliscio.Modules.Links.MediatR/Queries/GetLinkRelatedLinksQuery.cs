using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinkRelatedLinksQuery : IRequest<LinkItem[]>
{
    public Guid Id { get; init; }

    public GetLinkRelatedLinksQuery(Guid id)
    {
        Guard.Against.NullOrEmpty(id);

        Id = id;
    }
}