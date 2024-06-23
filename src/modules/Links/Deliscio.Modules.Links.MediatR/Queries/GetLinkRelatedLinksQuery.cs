using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinkRelatedLinksQuery : IRequest<LinkItemDto[]>
{
    public string Id { get; init; }

    public GetLinkRelatedLinksQuery(string id)
    {
        Guard.Against.NullOrEmpty(id);

        Id = id;
    }
}