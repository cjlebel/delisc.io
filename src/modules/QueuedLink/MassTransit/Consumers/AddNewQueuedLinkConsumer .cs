using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MassTransit.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.QueuedLinks.MassTransit.Consumers;

public sealed class AddNewQueuedLinkConsumer : IConsumer<AddNewQueuedLinkCommand>
{
    private readonly ILogger<AddNewQueuedLinkConsumer> _logger;
    private readonly IQueuedLinksService _queueLinksService;
    protected AddNewQueuedLinkConsumer(IQueuedLinksService queueLinksService, ILogger<AddNewQueuedLinkConsumer> logger)
    {
        _logger = logger;
        _queueLinksService = queueLinksService;
    }

    public async Task Consume(ConsumeContext<AddNewQueuedLinkCommand> context)
    {
        var command = context.Message;

        await context.Publish(command);
    }
}
