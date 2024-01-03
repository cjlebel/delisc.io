using System.Diagnostics;
using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Common.Abstracts;

public abstract class ManagerBase<T>
{
    private readonly IBusControl? _bus;

    protected ILogger<T> Logger { get; }

    protected ManagerBase(ILogger<T> logger)
    {
        Logger = logger;
    }

    protected ManagerBase(IBusControl bus, ILogger<T> logger)
    {
        _bus = bus;
        Logger = logger;
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
        Guard.Against.Null(_bus, message: "The message bus was not provided");
        Guard.Against.Null(message);

        try
        {
            await _bus.Publish(message, token);
        }
        // Token ran out of time
        catch (OperationCanceledException e)
        {
            Logger.LogError(e, "Operation was cancelled");
            throw;
        }
        // Couldn't reach the queue's endpoint
        catch (UnreachableException e)
        {
            Logger.LogError(e, "Could not reach the Queue");
            throw;
        }
        // Everything else
        catch (Exception e)
        {
            Logger.LogError(e, "An error occurred while trying to submit a new link");
            throw;
        }
    }
}