using Deliscio.Modules.QueuedLinks.Common.Interfaces;
using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.Harvester;

public interface IHarvesterProcessor : IProcessor
{
    ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> ExecuteAsync(QueuedLink link, CancellationToken token = default);
}