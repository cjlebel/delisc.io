using Deliscio.Modules.Links.Application.Contracts;
using MediatR;
using MongoDB.Driver;

namespace Deliscio.Modules.Links;

public class LinksModule(IMediator mediator) : ILinksModule
{
    public async Task<TResult> ExecuteAsync<TResult>(ILinksCommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task ExecuteAsync(ILinksCommand command, CancellationToken cancellationToken = default)
    {
        await mediator.Send(command, cancellationToken);
    }

    public async Task<TResult> ExecuteAsync<TResult>(ILinksQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return  await mediator.Send(query, cancellationToken);
    }
}