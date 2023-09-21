using Deliscio.Apis.WebApi.Api.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Structurizr.Annotations;

namespace Deliscio.Apis.WebApi.Managers;

[CodeElement("LinksManager", Description = "Facilitates dealing with the centrally saved Links")]
[UsedByContainer("Web API")]
public class LinksManager : ILinksManager
{
    private readonly ILogger<LinksManager> _logger;
    private readonly IMediator _mediator;

    public LinksManager(IMediator mediator, ILogger<LinksManager> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    //public async Task<IEnumerable<Link>> GetLink(string id)
    //{
    //    // Create a query request and dispatch it using MediatR
    //    var query = new GetU { UserId = userId };
    //    var result = await _mediator.Send(query);

    //    return result;
    //}
}