using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinkByIdQuery : IRequest<Link?>
{
    public Guid Id { get; init; }

    public GetLinkByIdQuery(Guid id)
    {
        Guard.Against.NullOrEmpty(id);

        Id = id;
    }
}