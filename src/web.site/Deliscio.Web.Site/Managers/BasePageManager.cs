using Deliscio.Apis.WebApi.Common.Clients;
using MediatR;

namespace Deliscio.Web.Site.Managers;

public abstract class BasePageManager
{
    protected readonly ILogger? Logger;
    protected readonly IMediator? MediatR;

    //protected readonly WebApiClient WebClient;

    protected BasePageManager(IMediator mediator) : this(mediator, null) { }

    //protected BasePageManager(WebApiClient webClient, IMediator mediator) : this(webClient, mediator, null) { }

    protected BasePageManager(IMediator mediator, ILogger? logger)
    {
        Logger = logger;
        MediatR = mediator;
        //WebClient = webClient;
    }

    //protected BasePageManager(WebApiClient webClient, IMediator mediator, ILogger? logger)
    //{
    //    Logger = logger;
    //    MediatR = mediator;
    //    WebClient = webClient;
    //}
}