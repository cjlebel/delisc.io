using MediatR;

namespace Deliscio.Modules.Links.Application.Contracts;

public interface ILinksQuery<out TResult> : IRequest<TResult> { }