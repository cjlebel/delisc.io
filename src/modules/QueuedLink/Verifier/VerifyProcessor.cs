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

    // Simple list for now. Should be read from somewhere, where that list can be updated.
    private readonly List<string> _invalidDomains = new() { "127.0.0.1", "192.168.", "localhost", "mail." };

    public VerifyProcessor(IMediator mediator, ILogger<VerifyProcessor> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

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
            return (false, existingLink.Id);
        }

        if (_invalidDomains.Exists(x => link.Url.Contains(x)))
        {
            _logger.LogInformation(VERIFYING_ERROR_MESSAGE, DateTimeOffset.Now, link.Url, "Invalid domain");
            link.State = QueuedStates.Rejected;

            return (false, "Invalid domain");
        }

        link.State = QueuedStates.VerifyingCompleted;

        _logger.LogInformation(VERIFYING_COMPLETED_MESSAGE, DateTimeOffset.Now, link.Url);

        return (true, "Verifying Complete");
    }
}