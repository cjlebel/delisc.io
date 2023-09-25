using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.MassTransit.Commands;

public class AddNewQueuedLinkCommand
{
    public QueuedLink Link { get; set; }

    public AddNewQueuedLinkCommand()
    {
        Link = new QueuedLink();
    }
}