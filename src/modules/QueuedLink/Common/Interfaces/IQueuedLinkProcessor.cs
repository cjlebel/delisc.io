using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.Common.Interfaces;

public interface IQueuedLinkProcessor
{
    ValueTask<(bool IsSuccess, string Message, QueuedLink? Link)> ExecuteAsync(QueuedLink link, CancellationToken token = default);
}