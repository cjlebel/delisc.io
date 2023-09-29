using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.Interfaces;

public interface IQueuedLinksService
{
    ValueTask<(bool IsSuccess, string Message, QueuedLink? Link)> ProcessNewLinkAsync(QueuedLink link, CancellationToken token = default);
}