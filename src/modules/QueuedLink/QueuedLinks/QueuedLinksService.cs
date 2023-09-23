using Ardalis.GuardClauses;
using Deliscio.Core.Abstracts;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.QueuedLinks.Common.Enums;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.Verifier;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.QueuedLinks;

/// <summary>
/// Represents a service that processes links that are in the queue awaiting to be verified and added to the central links repository.
/// </summary>
/// <remarks>
/// This is not an ideal implementation. Quick and dirty for now.
/// </remarks>
public class QueuedLinksService : ServiceBase, IQueuedLinksService
{
    private readonly ILogger<QueuedLinksService> _logger;
    private readonly IVerifyProcessor _verifier;

    // Best practice is to use a constant for the message format string. This way it will be reused, instead of recreated for each message
    private const string PROCESSOR_ERROR_IMPROPER_STATE = "Link is not in the correct state to be processed: {name}";
    public QueuedLinksService(IVerifyProcessor verifier, ILogger<QueuedLinksService> logger)
    {
        _logger = logger;
        _verifier = verifier;
    }

    public async ValueTask<(bool IsSuccess, string Message)> ProcessNewLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        Guard.Against.Null(link);
        Guard.Against.NullOrWhiteSpace(link.Url);

        (bool IsSuccess, string Message) result = (false, string.Empty);

        if (link.State.Id != QueuedStates.New.Id)
        {
            _logger.LogWarning(PROCESSOR_ERROR_IMPROPER_STATE, link.State.Name);
            link.State = QueuedStates.Error;

            result = (false, $"Improper State - Expected {QueuedStates.New.Name}");
        }

        var verifyResult = await VerifyLinkAsync(link, token);

        if (!verifyResult.IsSuccess)
        {
            if (link.State == QueuedStates.Exists)
            {
                return (true, verifyResult.Message);
            }

            result = verifyResult;
        }



        return result;
    }

    private async Task<(bool IsSuccess, string Message)> VerifyLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        (bool IsSuccess, string Message) result = (false, "Could not verify the link");

        try
        {
            result = await _verifier.ExecuteAsync(link, token);
        }
        // Probably should create a custom exception, to differentiate between different cases so that they can be handle appropriately
        catch (ArgumentNullException ex)
        {
            return (false, ex.Message);
        }

        if (!result.IsSuccess)
        {
            // Save this somewhere so that it can be looked at later?
            // Have specific rejection states (eg:InvalidDomain)?

            return result;
        }

        if (link.State == QueuedStates.VerifyingCompleted || link.State == QueuedStates.Exists)
        {
            return result;
        }

        if (link.State == QueuedStates.Error)
        {
            // Save this somewhere so that it can be looked at later

            return result;
        }

        return result;
    }
}
