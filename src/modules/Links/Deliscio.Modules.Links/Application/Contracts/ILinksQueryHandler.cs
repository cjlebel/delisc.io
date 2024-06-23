using MediatR;

namespace Deliscio.Modules.Links.Application.Contracts;
public interface ILinksQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : ILinksQuery<TResult>
{
}
