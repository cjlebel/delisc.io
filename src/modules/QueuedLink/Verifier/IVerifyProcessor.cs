using Deliscio.Modules.QueuedLinks.Common.Interfaces;
using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.Verifier;

public interface IVerifyProcessor : IQueuedLinkProcessor
{
    ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> ExecuteAsync(QueuedLink link, CancellationToken token = default);
}