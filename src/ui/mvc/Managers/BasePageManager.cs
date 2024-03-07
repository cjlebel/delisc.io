using Deliscio.Apis.WebApi.Common.Clients;
using MediatR;

namespace Deliscio.Web.Mvc.Managers;

public abstract class BasePageManager
{
    protected readonly ILogger? Logger;
    protected readonly IMediator? MediatR;

    protected readonly WebApiClient WebClient;

    protected BasePageManager(WebApiClient webClient, IMediator mediator) : this(webClient, mediator, null) { }

    protected BasePageManager(WebApiClient webClient, IMediator mediator, ILogger? logger)
    {
        Logger = logger;
        MediatR = mediator;
        WebClient = webClient;
    }
}