using Deliscio.Modules.QueuedLinks.Common.Interfaces;

namespace Deliscio.Modules.QueuedLinks.Harvester;

public interface IHarvesterProcessor : IProcessor
{
    ValueTask<(bool IsSuccess, string Message)> ExecuteAsync(HarvestedLink link, CancellationToken token = default);
}