using MediatR;

namespace Deliscio.Web.Mvc.Managers;

public abstract class BasePageManager
{
    protected IMediator? MediatR;

    protected BasePageManager(IMediator mediator)
    {
        MediatR = mediator;
    }
}