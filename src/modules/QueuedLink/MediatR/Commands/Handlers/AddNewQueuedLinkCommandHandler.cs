using Ardalis.GuardClauses;
using Deliscio.Modules.QueuedLinks.Interfaces;
using MediatR;

namespace Deliscio.Modules.QueuedLinks.MediatR.Commands.Handlers;

/// <summary>
/// Adds a link to the processing queue, marked as "new"
/// </summary>
public class AddNewLinkQueueCommandHandler : IRequestHandler<AddNewLinkQueueCommand, bool>
{
    private readonly IQueuedLinksService _queueService;

    public AddNewLinkQueueCommandHandler(IQueuedLinksService queueService)
    {
        Guard.Against.Null(queueService);

        _queueService = queueService;
    }

    public async Task<bool> Handle(AddNewLinkQueueCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command);

        //await _queueService.AddLinkAsync(command.Url, command.SubmittedById, command.UsersTitle, command.UsersDescription, command.Tags, cancellationToken);

        return true;
    }
}