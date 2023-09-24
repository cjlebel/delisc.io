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

    public async ValueTask<(bool IsSuccess, string Message)> ProcessNewLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        Guard.Against.Null(link);
        Guard.Against.NullOrWhiteSpace(link.Url);

        if (link.State.Id != QueuedStates.New.Id)
        {
            _logger.LogWarning(PROCESSOR_ERROR_IMPROPER_STATE, link.State.Name);
            link.State = QueuedStates.Error;

            return (false, $"Improper State - Expected {QueuedStates.New.Name}");
        }

        var verifyResult = await VerifyLinkAsync(link, token);

        if (!verifyResult.IsSuccess)
        {
            return verifyResult;
        }

        //var harvestResult = await HarvestLinkAsync(link, token);

        //if (!harvestResult.IsSuccess)
        //{
        //    return harvestResult;
        //}

        return verifyResult;
    }

    private async ValueTask<(bool IsSuccess, string Message)> VerifyLinkAsync(QueuedLink link, CancellationToken token = default)
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

        // Redundant.
        if (!result.IsSuccess || link.State == QueuedStates.Error)
        {
            // Save this somewhere so that it can be looked at later?
            // Have specific rejection states (eg:InvalidDomain)?
            return result;
        }

        if (link.State == QueuedStates.VerifyingCompleted || link.State == QueuedStates.Exists)
        {
            return result;
        }

        return result;
    }

    private async ValueTask<(bool IsSuccess, string Message, HarvestedLink Link)> HarvestLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        var harvestedLink = (HarvestedLink)link;

        try
        {
            var result = await _harvester.ExecuteAsync(harvestedLink, token);

            if (result.IsSuccess)
            {
                harvestedLink.State = QueuedStates.FetchingMetaCompleted;
                return (true, "Successfully harvested the link", harvestedLink);
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message, new HarvestedLink());
        }

        return (false, "Could not harvest the link", new HarvestedLink());

    }
}
