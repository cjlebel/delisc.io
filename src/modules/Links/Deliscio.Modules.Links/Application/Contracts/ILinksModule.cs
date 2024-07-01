namespace Deliscio.Modules.Links.Application.Contracts;

public interface ILinksModule
{
    Task<TResult> ExecuteAsync<TResult>(ILinksCommand<TResult> command, CancellationToken cancellationToken = default);

    Task ExecuteAsync(ILinksCommand command, CancellationToken cancellationToken = default);

    Task<TResult> ExecuteAsync<TResult>(ILinksQuery<TResult> query, CancellationToken cancellationToken = default);
}