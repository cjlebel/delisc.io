using System.Diagnostics;
using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Common.Abstracts;

public abstract class ManagerBase<T>
{
    private readonly IBusControl _bus;
    private readonly ILogger<T> _logger;

    protected ManagerBase(IBusControl bus, ILogger<T> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    /// <summary>
    /// Helper method to publish a message to the queue via MassTransit.
    /// Handles catching errors and logging them
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="message"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    protected async Task Publish<TMessage>(TMessage message, CancellationToken token = default)
    {
        Guard.Against.Null(message);

        try
        {
            await _bus.Publish(message, token);
        }
        // Token ran out of time
        catch (OperationCanceledException e)
        {
            _logger.LogError(e, "Operation was cancelled");
            throw;
        }
        // Couldn't reach the queue's endpoint
        catch (UnreachableException e)
        {
            _logger.LogError(e, "Could not reach the Queue");
            throw;
        }
        // Everything else
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while trying to submit a new link");
            throw;
        }
    }
}