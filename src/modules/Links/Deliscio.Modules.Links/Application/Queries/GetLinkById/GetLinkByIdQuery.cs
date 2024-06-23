using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Contracts;
using Deliscio.Modules.Links.Application.Dtos;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Queries.GetLinkById;

public class GetLinkByIdQuery : ILinksQuery<Result<LinkDto>>
{
    public string Id { get; }

    public GetLinkByIdQuery(string id)
    {
        Guard.Against.NullOrWhiteSpace(id, nameof(id));

        Id = id;
    }
}
