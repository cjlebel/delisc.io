using Deliscio.Modules.SubmittedLinks.Abstractions.Models;

namespace Deliscio.Modules.SubmittedLinks.Abstractions.Interfaces;

public interface ISubmittedLinksService
{
    Task<SubmittedLink?> GetAsync(string id, CancellationToken token = default);

    Task<IEnumerable<SubmittedLink>> GetAsync(int pageNo = 1, int pageSize = 25, CancellationToken token = default);
}