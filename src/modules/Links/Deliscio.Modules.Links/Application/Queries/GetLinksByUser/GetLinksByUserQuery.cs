using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Contracts;
using Deliscio.Modules.Links.Application.Dtos;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Queries.GetLinksByUser;

public class GetLinksByUserQuery : ILinksQuery<Result<IEnumerable<LinkDto>>>
{
    public string UserId { get; }

    public GetLinksByUserQuery(string userId)
    {
        Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

        UserId = userId;
    }
}
