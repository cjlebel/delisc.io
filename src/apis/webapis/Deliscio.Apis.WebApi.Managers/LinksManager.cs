using System.Diagnostics;
using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Models.Requests;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.QueuedLinks.Common.Enums;
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
    private readonly IMediator _mediator;
    private readonly IQueuedLinksService _queueService;

    private const string ERROR_COULD_NOT_APPROVE = "{time}: The Link '{Url}' could not be approved";

    public LinksManager(IMediator mediator, IBusControl bus, IQueuedLinksService queueService, ILogger<LinksManager> logger) : base(bus, logger)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(bus);
        Guard.Against.Null(queueService);
        Guard.Against.Null(logger);

        _bus = bus;
        _mediator = mediator;
        _queueService = queueService;
    }

    public Task<PagedResults<LinkItem>> FindAsync(string search = "", string tags = "", int pageNo = 1, int pageSize = 50, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(search);
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var request = new FindLinksRequest(pageNo, search, "", Array.Empty<string>(), pageSize, 0);

        var query = new FindLinksQuery(request);

        return _mediator.Send(query, token);
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

        var query = new GetLinkByIdQuery(new Guid(id));

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
    [Obsolete("Use FindAsync instead")]
    public Task<PagedResults<LinkItem>> GetLinksAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var query = new GetLinksQuery(pageNo, pageSize);

        return _mediator.Send(query, token);
    }

    public async Task<PagedResults<LinkItem>> GetLinksByDomainAsync(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        var query = new GetLinksByDomainQuery(domain, pageNo, pageSize);

        return await _mediator.Send(query, token);
    }

    public async ValueTask<IEnumerable<LinkItem>> GetLinksByIdsAsync(string[] ids, CancellationToken token = default)
    {
        if (ids.Length == 0)
            return Enumerable.Empty<LinkItem>();

        var query = new GetLinksByIdsQuery(ids);

        return await _mediator.Send(query, token);
    }

    /// <summary>
    /// Gets a page of links where each link contains all of the links that were given.
    /// </summary>
    /// <param name="tags">The collection of tags to use to get the links</param>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <param name="token"></param>
    /// <returns>
    /// A page of links where each link contains all of the links that were given.
    /// </returns>
    public async ValueTask<PagedResults<LinkItem>> GetLinksByTagsAsync(string tags = "", int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        if (tags.Length == 0)
            return new PagedResults<LinkItem>();

        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        var query = new GetLinksByTagsQuery(pageNo, pageSize, tags);

        return await _mediator.Send(query, token);
    }

    public async ValueTask<LinkItem[]> GetRelatedLinksAsync(string id, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(id);

        var query = new GetLinkRelatedLinksQuery(new Guid(id));

        return await _mediator.Send(query, token);
    }

    /// <summary>
    /// Gets a collection of tags that are related to the given tags.
    /// This is used to drill down into the links.
    /// </summary>
    /// <param name="tags">
    /// The initial set of tags, that the results are based on.
    /// If no tags are provided, then a collection of the top tags are returned
    /// </param>
    /// <param name="count">The number of tags to return</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<LinkTag[]> GetTagsRelatedToTagsAsync(string tags, int? count = default, CancellationToken token = default)
    {
        var query = new GetRelatedTagsByTagsQuery(tags, count);

        return _mediator.Send(query, token);
    }

    public async Task<string> SubmitLinkAsync(string url, string submittedByUserId, string usersTitle = "", string usersDescription = "", string[]? tags = default, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(url);
        Guard.Against.NullOrEmpty(submittedByUserId);

        // Placing this here so that I can have access to it in the exception block
        (bool IsSuccess, string Message, QueuedLink? Link) result = (false, string.Empty, null);

        var tagsToAdd = tags ?? Array.Empty<string>();

        try
        {
            url = url.Trim('"');
            var newLink = QueuedLink.Create(new Uri(url), submittedByUserId, UsersData.Create(usersTitle, usersDescription, tagsToAdd));

            // var addToQueueCommand = new AddNewLinkQueueCommand(newLink);
            // await Publish(newLink, token);
            try
            {
                //_mediator.Send(newLink, token);
                //await _bus.Publish(newLink, token);

                Link? link = null;
                // Short circuiting the queue for now, as it's not working.
                // This will allow me to test the rest of the system.

                result = await _queueService.ProcessNewLinkAsync(newLink, token);

                if (result.Link != null)
                {
                    var queuedLink = result.Link;

                    if (!result.IsSuccess)
                    {
                        Logger.LogWarning(ERROR_COULD_NOT_APPROVE, DateTimeOffset.Now, queuedLink.Url);
                    }

                    // Success
                    if (queuedLink.State == QueuedStates.Finished || queuedLink.State == QueuedStates.Exists)
                    {
                        // If state is Exists, then get the existing id.
                        var existingLinkId = queuedLink.State == QueuedStates.Exists ? queuedLink.LinkId : Guid.Empty;

                        if (queuedLink.State == QueuedStates.Finished)
                        {
                            link = Link.Create(queuedLink.Url, queuedLink.SubmittedById.ToString(), queuedLink.Title, queuedLink.MetaData?.Description ?? string.Empty, queuedLink.Tags);
                            link.Domain = queuedLink.Domain;
                            link.Keywords = queuedLink.MetaData?.Keywords?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                            link.ImageUrl = queuedLink.MetaData?.OgImage ?? string.Empty;

                            // Add the link and get the id
                            var queryAdd = new AddLinkCommand(link);
                            existingLinkId = await _mediator.Send(queryAdd, token);
                        }
                        // If link already existed, then get it to associate it with the user
                        else if (queuedLink.State == QueuedStates.Exists)
                        {
                            var queryGet = new GetLinkByIdQuery(existingLinkId);
                            link = await _mediator.Send(queryGet, token);

                            //if (link != null)
                            //{
                            //    var newKeywords = queuedLink.MetaData?.Keywords?.Split(',', StringSplitOptions.RemoveEmptyEntries)?.ToArray() ?? Array.Empty<string>();
                            //    link.Keywords = (link.Keywords.Union(newKeywords)).Distinct().ToArray();

                            //    link.ImageUrl = !string.IsNullOrWhiteSpace(queuedLink.MetaData?.OgImage) ?
                            //        queuedLink.MetaData?.OgImage :
                            //        link.ImageUrl;

                            //    if (queuedLink.Tags != null && queuedLink.Tags.Any())
                            //    {
                            //        foreach (var tag in queuedLink.Tags)
                            //        {
                            //            if (!string.IsNullOrWhiteSpace(tag))
                            //            {
                            //                var existingTag = link.Tags.FirstOrDefaultAsync(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

                            //                if (existingTag == null)
                            //                {
                            //                    link.Tags.Add(LinkTag.Create(tag));
                            //                }
                            //            }

                            //        }
                            //    }


                            //}
                        }
                    }
                }



                // Check for null again after all is said and done. If it exists, then associate with the current user
                if (link != null)
                {
                    // It must now exist, so associate the link with the user

                    //var associateCommand = new AssociateLinkWithUserCommand(existingLinkId, queuedLink.SubmittedById.ToString());
                }

                return result.Message;
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
        catch (UriFormatException e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            throw;
        }

        return string.Empty;
    }
}