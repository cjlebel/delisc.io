using Ardalis.GuardClauses;
using Deliscio.Modules.QueuedLinks.Common.Models;
using MediatR;

namespace Deliscio.Modules.QueuedLinks.MediatR.Commands;

public class ProcessNewQueuedLinkCommand : IRequest<bool>
{
    QueuedLink QueuedLink { get; }

    public ProcessNewQueuedLinkCommand(QueuedLink newQueuedLink)
    {
        Guard.Against.Null(newQueuedLink);

        QueuedLink = newQueuedLink;
    }
}