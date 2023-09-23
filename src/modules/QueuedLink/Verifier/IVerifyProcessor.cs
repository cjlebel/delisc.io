using Deliscio.Modules.QueuedLinks.Common.Interfaces;
using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.Verifier;

public interface IVerifyProcessor : IProcessor
{
    ValueTask<(bool IsSuccess, string Message)> ExecuteAsync(QueuedLink link, CancellationToken token = default);
}