using MediatR;

namespace Deliscio.Web.Mvc.Managers;

public abstract class BasePageManager
{
    protected readonly ILogger? Logger;
    protected readonly IMediator? MediatR;

    protected BasePageManager(IMediator mediator) : this(mediator, null) { }

    protected BasePageManager(IMediator mediator, ILogger? logger)
    {
        Logger = logger;
        MediatR = mediator;
    }
}