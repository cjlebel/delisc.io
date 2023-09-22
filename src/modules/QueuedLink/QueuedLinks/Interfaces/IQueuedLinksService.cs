namespace Deliscio.Modules.QueuedLinks.Interfaces;

public interface IQueuedLinksService
{
    Task<Guid> AddLinkAsync(string url, Guid submittedByUserId, string usersTitle = "", string usersDescription = "", string[]? tags = default, CancellationToken token = default);
}