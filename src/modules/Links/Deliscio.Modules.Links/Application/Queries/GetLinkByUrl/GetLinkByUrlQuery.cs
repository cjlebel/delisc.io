using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Contracts;
using Deliscio.Modules.Links.Application.Dtos;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Queries.GetLinkByUrl;

public class GetLinkByUrlQuery : ILinksQuery<Result<LinkDto>>
{
    public string Id { get; set; }

    private GetLinkByUrlQuery(string id)
    {
        Id = id;
    }

    public static GetLinkByUrlQuery Create(string id)
    {
        Guard.Against.NullOrWhiteSpace(id, nameof(id));

        return new GetLinkByUrlQuery(id);
    }
}
