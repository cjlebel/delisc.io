using Ardalis.GuardClauses;
using Deliscio.Modules.UserLinks.Common.Models;
using MediatR;

namespace Deliscio.Modules.UserLinks.MediatR.Queries;

public sealed record GetUserLinkByIdQuery : IRequest<UserLink?>
{
    public Guid UserId { get; init; }

    public Guid LinkId { get; init; }

    public GetUserLinkByIdQuery(string userId, string linkId)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        Guard.Against.NullOrWhiteSpace(linkId);

        var newUserId = Guid.Parse(userId);
        var newLinkId = Guid.Parse(linkId);

        Guard.Against.NullOrEmpty(newUserId);
        Guard.Against.NullOrEmpty(newLinkId);

        UserId = newUserId;
        LinkId = newLinkId;
    }
}