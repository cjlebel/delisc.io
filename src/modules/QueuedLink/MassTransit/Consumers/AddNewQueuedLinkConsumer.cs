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
        throw new NotImplementedException("MAssTransit/RabbitMQ aren't working at the moment");

        var command = context.Message;

        if (command.Link != null!)
        {
            if (command.Link != null!)
            {
                var queuedLink = command.Link;

                var result = await _queuedLinksService.ProcessNewLinkAsync(queuedLink);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning(ERROR_COULD_NOT_APPROVE, DateTimeOffset.Now, queuedLink.Url);
                }

                if (queuedLink.State == QueuedStates.Finished || queuedLink.State == QueuedStates.Exists)
                {
                    var existingLinkId = queuedLink.State == QueuedStates.Exists ? queuedLink.LinkId : string.Empty;
                    Link? link;

                    if (queuedLink.State == QueuedStates.Finished)
                    {
                        link = Link.Create(queuedLink.Url, queuedLink.SubmittedById.ToString(), queuedLink.Title, queuedLink.MetaData?.Description ?? string.Empty, queuedLink.Tags);
                        link.Keywords = queuedLink.MetaData?.Keywords?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

                        var queryAdd = new AddLinkCommand(link);
                        existingLinkId = await _mediator.Send(queryAdd);
                    }
                    // If link already existed, then get it to associate it with the user
                    else if (queuedLink.State == QueuedStates.Exists)
                    {
                        var queryGet = new GetLinkByIdQuery(existingLinkId);
                        link = await _mediator.Send(queryGet);
                    }

                    // It must now exist, so associate the link with the user

                    //var associateCommand = new AssociateLinkWithUserCommand(existingLinkId, queuedLink.SubmittedById.ToString());
                }
            }
        }



        // Optional: Acknowledge the message to remove it from the queue
        await context.ConsumeCompleted;
    }
}
