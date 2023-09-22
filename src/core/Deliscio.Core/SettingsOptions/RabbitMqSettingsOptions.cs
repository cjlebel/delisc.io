namespace Deliscio.Core.SettingsOptions;

public abstract class RabbitMqSettingsOptions : SettingsOptions
{
    protected RabbitMqSettingsOptions(string sectionName) : base(sectionName)
    {
    }
    public string Host { get; set; } = string.Empty;

    public string QueueName { get; set; } = string.Empty;

    public int MaxConcurrentCalls { get; set; } = 1;

    public int MaxDequeueCount { get; set; } = 5;

    public int MaxRetryCount { get; set; } = 5;

    public int VisibilityTimeout { get; set; } = 60;
}