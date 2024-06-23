using Ardalis.GuardClauses;
using Deliscio.Modules.UserLinks.Common.Models;
using MediatR;

namespace Deliscio.Modules.UserLinks.MediatR.Queries;

public sealed record GetUserLinkByIdQuery : IRequest<UserLink?>
{
    public string UserId { get; init; }

    public string LinkId { get; init; }

    public GetUserLinkByIdQuery(string userId, string linkId)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        Guard.Against.NullOrWhiteSpace(linkId);

        UserId = userId;
        LinkId = linkId;
    }
}