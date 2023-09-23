using Deliscio.Modules.QueuedLinks.MassTransit.Commands;
using Deliscio.Modules.QueuedLinks.MassTransit.Models;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Deliscio.Workers.LinksProcessor;

public class Worker : BackgroundService
{
    private readonly IBusControl _busControl;
    private readonly ILogger<Worker> _logger;
    private readonly IOptions<LinksQueueSettingsOptions> _options;
    private readonly string _queueName;

    private const string COULD_NOT_GET_OPTIONS = "Could not retrieve the options";

    public Worker(IOptions<LinksQueueSettingsOptions> options, IBusControl busControl, ILogger<Worker> logger)
    {
        _busControl = busControl;
        _logger = logger;

        if (options.Value == null)
            throw new ArgumentException(COULD_NOT_GET_OPTIONS, nameof(options));

        _queueName = options.Value.QueueName;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _busControl.StartAsync(cancellationToken);

        // Subscribe to the message queue with a handler for Command messages
        await _busControl.Publish<AddNewQueuedLinkCommand>(_queueName, cancellationToken);

        _logger.LogInformation("Submitted Links Worker started.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _busControl.StartAsync(stoppingToken);

            _logger.LogInformation("Submitted Links Worker running at: {time}", DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _busControl.StopAsync(cancellationToken);

        _logger.LogInformation("Submitted Links Worker stopped.");
    }
}
