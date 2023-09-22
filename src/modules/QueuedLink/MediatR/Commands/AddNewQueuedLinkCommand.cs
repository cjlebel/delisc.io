using Ardalis.GuardClauses;
using Deliscio.Modules.QueuedLinks.Common.Models;
using MediatR;

namespace Deliscio.Modules.QueuedLinks.MediatR.Commands;

/// <summary>
/// Represents a MediatR command that adds a brand spanking new link to the queue.
/// </summary>
public record AddNewLinkQueueCommand : IRequest<bool>
{
    QueuedLink NewQueuedLink { get; }

    public AddNewLinkQueueCommand(QueuedLink newQueuedLink)
    {
        Guard.Against.Null(newQueuedLink);

        NewQueuedLink = newQueuedLink;
    }
}