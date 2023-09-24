using Ardalis.GuardClauses;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.QueuedLinks.Common.Enums;
using Deliscio.Modules.QueuedLinks.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.QueuedLinks.Verifier;

public class VerifyProcessor : IVerifyProcessor
{
    private readonly ILogger<VerifyProcessor> _logger;
    private readonly IMediator _mediator;

    // Best practice is to use a constant for the message format string. This way it will be reused, instead of recreated for each message
    private const string VERIFYING_STARTED_MESSAGE = "{time}: Verifying Started for: {url}";
    private const string VERIFYING_COMPLETED_MESSAGE = "{time}: Verifying Completed for: {url}";
    private const string VERIFYING_ERROR_MESSAGE = "{time}: Verifying Error for: {url}\nMessage: {message}";
    private const string VERIFYING_LINK_ALREADY_EXISTS_MESSAGE = "{time}: Link already exists for: {url}";
    
    
    // Simple list of invalid domains. Should be read from somewhere, where that list can be updated.
    private readonly string[] _invalidDomains = { "127.0.0.1", "192.168.", "localhost", "mail." };

    // Simple list of valid protocols. Should be read from somewhere, where that list can be updated.
    private readonly string[] _validProtocols = { "http://", "https://" };
    public VerifyProcessor(IMediator mediator, ILogger<VerifyProcessor> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Executes the verifier, which checks to see if the link already exists in the central links repository, or if the domain is invalid. Other validations may occur as well.
    /// If the link already does exist, then IsSuccess = true, and Message = the existing link id. The link's state will be set to Exists.
    /// If the link isn't valid due to the rules, then IsSuccess = false, and Message = the reason why. The link's state will be set to Rejected.
    /// If the link passes the verification, then IsSuccess = true, and Message = "Verifying Complete". The link's state will be set to VerifyingCompleted. 
    /// </summary>
    /// <param name="link">The link object to be verified</param>
    /// <param name="token">The cancellation token to end the request</param>
    /// <returns></returns>
    public async ValueTask<(bool IsSuccess, string Message)> ExecuteAsync(QueuedLink link, CancellationToken token = default)
    {
        Guard.Against.Null(link);
        Guard.Against.NullOrWhiteSpace(link.Url);

        _logger.LogInformation(VERIFYING_STARTED_MESSAGE, DateTimeOffset.Now, link.Url);

        link.State = QueuedStates.Verifying;

        var query = new GetLinkByUrlQuery(link.Url);
        var existingLink = await _mediator.Send(query, token);

        if (existingLink is not null)
        {
            _logger.LogInformation(VERIFYING_LINK_ALREADY_EXISTS_MESSAGE, DateTimeOffset.Now, link.Url);
            link.State = QueuedStates.Exists;

            // Hacky way of returning the existing link id. Should be refactored.
            return (true, existingLink.Id);
        }

        // Verify that the domain is valid
        if (_invalidDomains.Any(x => link.Url.Contains(x, StringComparison.InvariantCultureIgnoreCase)))
        {
            _logger.LogInformation(VERIFYING_ERROR_MESSAGE, DateTimeOffset.Now, link.Url, "Invalid domain");
            link.State = QueuedStates.Rejected;

            return (false, "Invalid domain");
        }

        // Verify that the protocol is valid
        if (!_validProtocols.Any(x => link.Url.StartsWith(x, StringComparison.InvariantCultureIgnoreCase)))
        {
            _logger.LogInformation(VERIFYING_ERROR_MESSAGE, DateTimeOffset.Now, link.Url, "Invalid protocol");
            link.State = QueuedStates.Rejected;

            return (false, "Invalid protocol");
        }
        
        link.State = QueuedStates.VerifyingCompleted;

        _logger.LogInformation(VERIFYING_COMPLETED_MESSAGE, DateTimeOffset.Now, link.Url);

        return (true, "Verifying Complete");
    }
}