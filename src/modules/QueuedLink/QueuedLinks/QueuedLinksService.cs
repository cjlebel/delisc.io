using Ardalis.GuardClauses;
using Deliscio.Core.Abstracts;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.QueuedLinks.Common.Enums;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Harvester;
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
    private readonly IHarvesterProcessor _harvester;
    private readonly IVerifyProcessor _verifier;
    private readonly ILogger<QueuedLinksService> _logger;

    // Best practice is to use a constant for the message format string. This way it will be reused, instead of recreated for each message
    private const string PROCESSOR_ERROR_IMPROPER_STATE = "Link is not in the correct state to be processed: {name}";
    public QueuedLinksService(IVerifyProcessor verifier, IHarvesterProcessor harvester, ILogger<QueuedLinksService> logger)
    {
        _harvester = harvester;
        _verifier = verifier;

        _logger = logger;
    }

    public async ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> ProcessNewLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        if (link == null!)
            return (false, "Link is null", new QueuedLink());

        if (string.IsNullOrWhiteSpace(link.Url))
            return (false, "Link is missing a URL", new QueuedLink());

        //Guard.Against.Null(link);
        //Guard.Against.NullOrWhiteSpace(link.Url);

        QueuedLink updatedLink;

        if (link.State.Id != QueuedStates.New.Id)
        {
            _logger.LogWarning(PROCESSOR_ERROR_IMPROPER_STATE, link.State.Name);
            updatedLink = link with { State = QueuedStates.Error };

            return (false, $"Improper State - Expected {QueuedStates.New.Name}", updatedLink);
        }

        var verifyResult = await VerifyLinkAsync(link, token);

        if (!verifyResult.IsSuccess)
        {
            return verifyResult;
        }

        updatedLink = verifyResult.Link;

        var harvestResult = await HarvestLinkAsync(updatedLink, token);

        if (!harvestResult.IsSuccess)
        {
            return harvestResult;
        }

        updatedLink = harvestResult.Link;


        return (true, "Link processed successfully", updatedLink);
    }

    private async ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> VerifyLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        (bool IsSuccess, string Message, QueuedLink? Link) result = (false, "Could not verify the link", null);

        try
        {
            result = await _verifier.ExecuteAsync(link, token);
        }
        // Probably should create a custom exception, to differentiate between different cases so that they can be handle appropriately
        catch (ArgumentNullException ex)
        {
            result.Message = ex.Message;

            return result;
        }

        // Redundant.
        if (!result.IsSuccess || link.State == QueuedStates.Error)
        {
            // Save this somewhere so that it can be looked at later?
            // Have specific rejection states (eg:InvalidDomain)?
            return result;
        }

        if (link.State == QueuedStates.VerifyingCompleted || link.State == QueuedStates.Exists)
        {
            // Log
            return result;
        }

        return result;
    }

    private async ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> HarvestLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        (bool IsSuccess, string Message, QueuedLink Link) result;

        try
        {
            result = await _harvester.ExecuteAsync(link, token);

            if (!result.IsSuccess)
            {
                return result;
            }

            return result;
        }
        catch (Exception ex)
        {
            return (false, ex.Message, link);
        }
    }
}
