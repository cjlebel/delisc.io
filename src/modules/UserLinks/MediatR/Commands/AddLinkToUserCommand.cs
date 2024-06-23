using Ardalis.GuardClauses;
using Deliscio.Modules.UserLinks.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.UserLinks.MediatR.Commands;

/// <summary>
/// Represents a MediatR command that adds an existing link to a user's collection.
/// </summary>
public class AddLinkToUserCommand : IRequest<string>
{
    // Debating whether or not to make the Ids here strings, and let the handler parse them.
    // This would mean the callers would not have to parse and validate them,
    // but instead they would be done in one place, the Handler.
    public string UserId { get; set; }

    public string LinkId { get; init; }

    public string[] Tags { get; init; }
    public string Title { get; init; }

    public bool IsPrivate { get; init; }


    public AddLinkToUserCommand(string userId, string linkId, string title = "", string[]? tags = default, bool isPrivate = false)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        Guard.Against.NullOrWhiteSpace(linkId);

        UserId = userId;
        LinkId = linkId;

        IsPrivate = isPrivate;

        Title = title;
        Tags = tags ?? Array.Empty<string>();
    }
}

public class AddLinkToUserCommandHandler : IRequestHandler<AddLinkToUserCommand, string>
{
    private readonly IUserLinksService _service;

    public AddLinkToUserCommandHandler(IUserLinksService service)
    {
        _service = service;
    }

    public Task<string> Handle(AddLinkToUserCommand command, CancellationToken cancellationToken)
    {
        // NOTE: Seeing if we can get away with not awaiting this task here.
        //       The caller themselves will await this task, so we don't need to.
        var task = _service.AddAsync(command.UserId, command.LinkId, command.Title, command.Tags, command.IsPrivate, cancellationToken);

        return task;
    }
}