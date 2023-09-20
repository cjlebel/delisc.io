using Deliscio.Apis.WebApi.Api.Common.Interfaces;
using Structurizr.Annotations;

namespace Deliscio.Apis.WebApi.Managers;

[CodeElement("LinksManager", Description = "Facilitates dealing with the centrally saved Links")]
[UsedByContainer("Web API")]
public class LinksManager : ILinksManager
{
    //private readonly ILogger<LinksManager> _logger;
    //private readonly ILinksService _linksService;

    //public LinksManager(ILinksService linksService, ILogger<LinksManager> logger)
    //{
    //    _logger = logger;
    //    _linksService = linksService;
    //}

    //public async Task<Link> GetAsync(string id, CancellationToken token = default)
    //{
    //    var result = await _linksService.GetAsync(id, token);
    //    return result;
    //}

    //public Task<IEnumerable<Link>> GetAsync(int pageNo = 1, int pageSize = 25, string userId = "", CancellationToken token = default)
    //{
    //    throw new NotImplementedException();
    //}
}