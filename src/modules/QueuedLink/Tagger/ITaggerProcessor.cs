using Deliscio.Modules.QueuedLinks.Common.Interfaces;
using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.Tagger;

public interface ITaggerProcessor : IQueuedLinkProcessor
{
    ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> ExecuteAsync(QueuedLink link, CancellationToken token = default);
}