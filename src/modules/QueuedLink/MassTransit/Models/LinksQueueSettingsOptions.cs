using Deliscio.Core.SettingsOptions;

namespace Deliscio.Modules.QueuedLinks.MassTransit.Models;

/// <summary>
/// RabbitMQ settings that are specific to the Links Queue.
/// </summary>
public sealed class LinksQueueSettingsOptions : RabbitMqSettingsOptions
{
    public LinksQueueSettingsOptions() : base("LinksQueueOptions")
    {
        QueueName = "links-queue";
        MaxConcurrentCalls = 1;
        MaxDequeueCount = 5;
        MaxRetryCount = 5;
        VisibilityTimeout = 60;
    }
}