using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.QueuedLinks.Common.Enums;
using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MassTransit.Commands;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.QueuedLinks.MassTransit.Consumers;

public class AddNewQueuedLinkConsumer : IConsumer<AddNewQueuedLinkCommand>
{
    private readonly ILogger<AddNewQueuedLinkConsumer> _logger;
    private readonly IMediator _mediator;
    private readonly IQueuedLinksService _queuedLinksService;

    private const string ERROR_COULD_NOT_APPROVE = "{time}: The Link '{Url}' could not be approved";

    public AddNewQueuedLinkConsumer(IQueuedLinksService queuedLinksService, IMediator mediator, ILogger<AddNewQueuedLinkConsumer> logger)
    {
        _logger = logger;
        _mediator = mediator;
        _queuedLinksService = queuedLinksService;
    }

    //NOTE: Must be public.
    public async Task Consume(ConsumeContext<AddNewQueuedLinkCommand> context)
    {
        var command = context.Message;

        var queuedLink = command.Link;

        var result = await _queuedLinksService.ProcessNewLinkAsync(queuedLink);

        if (!result.IsSuccess)
        {
            _logger.LogWarning(ERROR_COULD_NOT_APPROVE, DateTimeOffset.Now, queuedLink.Url);
        }

        if (queuedLink.State == QueuedStates.Finished || queuedLink.State == QueuedStates.Exists)
        {
            var existingLinkId = queuedLink.State == QueuedStates.Exists ? new Guid(result.Message) : Guid.Empty;
            Link? existingLink = null;

            if (queuedLink.State == QueuedStates.Finished)
            {
                var queryAdd = new AddLinkCommand(queuedLink.Url, queuedLink.Title, queuedLink.SubmittedById, queuedLink.Tags);
                existingLinkId = await _mediator.Send(queryAdd);

                if (existingLinkId != Guid.Empty)
                {
                    var queryGet = new GetLinkByIdQuery(existingLinkId);
                    existingLink = await _mediator.Send(queryGet);

                    if (existingLink != null)
                    {
                        // Update with the rest of the details
                    }
                }
            }
            else if (queuedLink.State == QueuedStates.Exists)
            {
                var queryGet = new GetLinkByIdQuery(existingLinkId);
                existingLink = await _mediator.Send(queryGet);
            }

            // It must now exist, so associate the link with the user
        }


        // Optional: Acknowledge the message to remove it from the queue
        await context.ConsumeCompleted;
    }
}
