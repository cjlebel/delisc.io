using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Contracts;
using Deliscio.Modules.Links.Application.Dtos;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Queries.GetRelatedLinks;

public class GetRelatedLinksQuery : ILinksQuery<Result<RelatedLinkDto[]>>
{
    public string LinkId { get; }

    public int? Count { get; }

    public bool IsAdmin { get; }

    public GetRelatedLinksQuery(string linkId, int? count = default, bool isAdmin = false)
    {
        Guard.Against.NullOrWhiteSpace(linkId);

        LinkId = linkId;
        Count = count;
        IsAdmin = isAdmin;
    }
}