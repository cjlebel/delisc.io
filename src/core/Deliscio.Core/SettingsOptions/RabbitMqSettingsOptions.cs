using Deliscio.Core.Interfaces;

namespace Deliscio.Core.SettingsOptions;

public abstract class RabbitMqSettingsOptions : SettingsOptions, IRabbitMqSettingsOptions
{
    public virtual string Host { get; set; } = string.Empty;

    public virtual string QueueName { get; set; } = string.Empty;

    public virtual int MaxConcurrentCalls { get; set; } = 1;

    public virtual int MaxDequeueCount { get; set; } = 5;

    public virtual int MaxRetryCount { get; set; } = 5;

    public virtual int VisibilityTimeout { get; set; } = 60;

    public virtual string Username { get; set; } = string.Empty;

    public virtual string Password { get; set; } = string.Empty;
}