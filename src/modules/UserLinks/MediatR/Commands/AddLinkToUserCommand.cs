using Ardalis.GuardClauses;
using MediatR;

namespace Deliscio.Modules.UserLinks.MediatR.Commands;

/// <summary>
/// Represents a MediatR command that adds an existing link to a user's collection.
/// </summary>
public class AddLinkToUserCommand : IRequest<Guid>
{
    // Debating whether or not to make the Ids here strings, and let the handler parse them.
    // This would mean the callers would not have to parse and validate them,
    // but instead they would be done in one place, the Handler.
    public Guid UserId { get; set; }

    public Guid LinkId { get; init; }

    public string[] Tags { get; init; }
    public string Title { get; init; }

    public bool IsPrivate { get; init; }


    public AddLinkToUserCommand(string userId, string linkId, string title = "", string[]? tags = default, bool isPrivate = false)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        Guard.Against.NullOrWhiteSpace(linkId);

        UserId = Guid.Parse(userId);
        LinkId = Guid.Parse(linkId);

        IsPrivate = isPrivate;

        Title = title;
        Tags = tags ?? Array.Empty<string>();
    }
}