using Deliscio.Core.SettingsOptions;

namespace Deliscio.Modules.QueuedLinks.MassTransit.Models;

/// <summary>
/// RabbitMQ settings that are specific to the Links Queue.
/// </summary>
public sealed class LinksQueueSettingsOptions : RabbitMqSettingsOptions
{
    public static string SectionName = "LinksQueueSettings";

    public override string Host { get; set; } = string.Empty;

    public override string QueueName { get; set; }

    public override int MaxConcurrentCalls { get; set; }

    public override int MaxDequeueCount { get; set; }

    public override int MaxRetryCount { get; set; }

    public override int VisibilityTimeout { get; set; }

    public override string Username { get; set; } = string.Empty;

    public override string Password { get; set; } = string.Empty;

    public string[] BannedSites { get; set; } = Array.Empty<string>();

    public LinksQueueSettingsOptions()
    {
        QueueName = "submitted-links";
        MaxConcurrentCalls = 1;
        MaxDequeueCount = 5;
        MaxRetryCount = 5;
        VisibilityTimeout = 60;
    }
}