using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.QueuedLinks.Common.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Structurizr.Annotations;

namespace Deliscio.Apis.WebApi.Managers;

[CodeElement("LinksManager", Description = "Facilitates dealing with the centrally saved Links")]
[UsedByContainer("Web API")]
public sealed class LinksManager : ManagerBase<LinksManager>, ILinksManager
{
    private readonly ILogger<LinksManager> _logger;
    private readonly IMediator _mediator;

    public LinksManager(IMediator mediator, IBusControl bus, ILogger<LinksManager> logger) : base(bus, logger)
    {
        _logger = logger;
        _mediator = mediator;
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
            var newLink = new QueuedLink(url, submittedByUserId, usersTitle, usersDescription, tagsToAdd);

            await Publish(newLink, token);
        }
        catch (UriFormatException e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return string.Empty;
    }
}