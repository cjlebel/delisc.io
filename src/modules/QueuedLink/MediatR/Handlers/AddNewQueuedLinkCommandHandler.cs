using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MediatR.Commands;
using MediatR;

namespace Deliscio.Modules.QueuedLinks.MediatR.Handlers;

/// <summary>
/// Adds a link to the processing queue, marked as "new"
/// </summary>
public class AddNewLinkQueueCommandHandler : IRequestHandler<AddNewLinkQueueCommand, bool>
{
    private readonly IQueuedLinksService _queueService;

    public AddNewLinkQueueCommandHandler(IQueuedLinksService queueService)
    {
        _queueService = queueService;
    }

    public async Task<bool> Handle(AddNewLinkQueueCommand command, CancellationToken cancellationToken)
    {
        //await _queueService.AddLinkAsync(command.Url, command.SubmittedById, command.UsersTitle, command.UsersDescription, command.Tags, cancellationToken);

        return true;
    }
}