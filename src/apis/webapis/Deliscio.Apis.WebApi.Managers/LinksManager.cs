using System.Diagnostics;
using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Structurizr.Annotations;

namespace Deliscio.Apis.WebApi.Managers;

[CodeElement("LinksManager", Description = "Facilitates dealing with the centrally saved Links")]
[UsedByContainer("Web API")]
public sealed class LinksManager : ManagerBase<LinksManager>, ILinksManager
{
    private readonly IBusControl _bus;
    private readonly ILogger<LinksManager> _logger;
    private readonly IMediator _mediator;
    private readonly IQueuedLinksService _queueService;

    public LinksManager(IMediator mediator, IBusControl bus, IQueuedLinksService queueService, ILogger<LinksManager> logger) : base(bus, logger)
    {
        _bus = bus;
        _mediator = mediator;
        _queueService = queueService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a single Link by its Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns>A single link if found, if not, then null</returns>
    /// <remarks>
    /// No async/await is used here, because it's not needed.
    /// </remarks>
    public Task<Link?> GetLinkAsync(string id, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(id);

        var query = new GetLinkByIdQuery(id);

        return _mediator.Send(query, token);
    }

    /// <summary>
    /// Gets a page of links
    /// </summary>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <param name="token"></param>
    /// <returns>A page of links.</returns>
    /// <remarks>
    /// No async/await is used here, because it's not needed.
    /// </remarks>
    public Task<PagedResults<Link>> GetLinksAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        var query = new GetLinksQuery(pageNo, pageSize);

        return _mediator.Send(query, token);
    }

    public async Task<string> SubmitLinkAsync(string url, string submittedByUserId, string usersTitle = "", string usersDescription = "", string[]? tags = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);
        Guard.Against.NullOrEmpty(submittedByUserId);

        //if (token == default)
        //   token = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

        var tagsToAdd = tags ?? Array.Empty<string>();

        try
        {
            var newLink = QueuedLink.Create(new Uri(url), submittedByUserId, UsersData.Create(usersDescription, usersTitle, tagsToAdd));

            //await Publish(newLink, token);
            try
            {
                //_mediator.Send(newLink, token);
                //await _bus.Publish(newLink, token);

                // Short circuiting the queue for now, as it's not working.
                // This will allow me to test the rest of the system.
                var result = await _queueService.ProcessNewLinkAsync(newLink, token);

                return result.Message;
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
        catch (UriFormatException e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }

        return string.Empty;
    }
}