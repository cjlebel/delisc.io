using MediatR;

namespace Deliscio.Modules.Links.Application.Contracts;

public interface ILinksCommand<out TResult> : IRequest<TResult> // ICommand
{
    Guid Id { get; }
}

public interface ILinksCommand : IRequest
{
    Guid Id { get; }
}