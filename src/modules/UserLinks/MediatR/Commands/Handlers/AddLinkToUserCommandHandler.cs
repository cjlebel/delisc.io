using Deliscio.Modules.UserLinks.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.UserLinks.MediatR.Commands.Handlers;

public class AddLinkToUserCommandHandler : IRequestHandler<AddLinkToUserCommand, Guid>
{
    private readonly IUserLinksService _service;

    public AddLinkToUserCommandHandler(IUserLinksService service)
    {
        _service = service;
    }

    public Task<Guid> Handle(AddLinkToUserCommand command, CancellationToken cancellationToken)
    {
        // NOTE: Seeing if we can get away with not awaiting this task here.
        //       The caller themselves will await this task, so we don't need to.
        var task = _service.AddAsync(command.UserId, command.LinkId, command.Title, command.Tags, command.IsPrivate, cancellationToken);

        return task;
    }
}