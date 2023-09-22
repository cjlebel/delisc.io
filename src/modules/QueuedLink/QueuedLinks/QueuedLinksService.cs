using Deliscio.Core.Abstracts;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Interfaces;

namespace Deliscio.Modules.QueuedLinks;

public class QueuedLinksService : ServiceBase, IQueuedLinksService
{
    public Task<Guid> AddLinkAsync(string url, Guid submittedByUserId, string usersTitle = "", string usersDescription = "", string[]? tags = default, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ProcessNewLinkAsync(QueuedLink link, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
