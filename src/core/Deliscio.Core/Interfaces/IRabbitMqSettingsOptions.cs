namespace Deliscio.Core.Interfaces;

public interface IRabbitMqSettingsOptions
{
    string Host { get; set; }

    string QueueName { get; set; }

    int MaxConcurrentCalls { get; set; }

    int MaxDequeueCount { get; set; }

    int MaxRetryCount { get; set; }

    int VisibilityTimeout { get; set; }

    string Username { get; set; }

    string Password { get; set; }
}