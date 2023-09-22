namespace Deliscio.Modules.QueuedLinks.Common.Models;

public sealed class LinksQueueOptions
{
    /// <summary>
    /// The name of the section in the config file that contains the MongoDb settings.
    /// </summary>
    public static string SectionName => "LinksQueueOptions";

    public string Host { get; set; } = string.Empty;

    public string QueueName { get; set; } = string.Empty;

    public int MaxConcurrentCalls { get; set; } = 1;

    public int MaxDequeueCount { get; set; } = 5;

    public int MaxRetryCount { get; set; } = 5;

    public int VisibilityTimeout { get; set; } = 60;
}