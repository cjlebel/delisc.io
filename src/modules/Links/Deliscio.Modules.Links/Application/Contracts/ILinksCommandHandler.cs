using MediatR;

namespace Deliscio.Modules.Links.Application.Contracts;

public interface ILinksCommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ILinksCommand
{
}

public interface ILinksCommandHandler<in TCommand, TResult> :
    IRequestHandler<TCommand, TResult>
    where TCommand : ILinksCommand<TResult>
{
}