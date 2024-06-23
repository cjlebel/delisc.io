using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Models.Requests;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Web.Site.ViewModels.Home;
using MediatR;

namespace Deliscio.Web.Site.Managers;

public interface IHomePageManager
{
    Task<HomePageViewModel> GetHomePageViewModelAsync(CancellationToken token = default);

    Task<LinkTag[]> GetTagsPageViewModelAsync(string? tags = default, CancellationToken token = default);
}

public class HomePagePageManager : BasePageManager, IHomePageManager
{
    private readonly int _defaultPageSize;
    private readonly int _defaultTagsSize;

    public HomePagePageManager(IMediator mediator, ILogger<HomePagePageManager>? logger) : base(mediator, logger)
    {
        // This can come from a settings file
        _defaultPageSize = 50;
        _defaultTagsSize = 100;
    }

    /// <summary>
    /// Gets the Home Page's View Model with its meta data and data to populate the page.
    /// This will evolve over time to possibly include groups of data.
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>A view model with the page's title, description, canonical url, and data</returns>
    public async Task<HomePageViewModel> GetHomePageViewModelAsync(CancellationToken token = default)
    {
        var request = new FindLinksRequest(1, string.Empty, string.Empty, Array.Empty<string>(), _defaultPageSize, 0);
        var query = new FindLinksQuery(request);
        //GetLinksQuery(1, _defaultPageSize);

        var results = await MediatR!.Send(query, token);

        //var results = await WebClient.GetLinksSearchResultsAsync(page: 1, pageSize: _defaultPageSize, token: token);

        if (results is null)
        {
            Logger.LogError("No results were found in {Name}", this.GetType().Name);

            return null;
        }

        var model = new HomePageViewModel(results)
        {
            CanonicalUrl = "https://deliscio.com",
            PageTitle = "Deliscio - Home",
            PageDescription = "Deliscio - Home"
        };

        return model;
    }

    public async Task<LinkTag[]> GetTagsPageViewModelAsync(string? tags = default, CancellationToken token = default)
    {
        var query = new GetRelatedTagsByTagsQuery(tags, _defaultTagsSize);

        var results = await MediatR!.Send(query, token);

        //var results = (await WebClient.GetRelatedTagsByTagsAsync(tags, _defaultTagsSize, token));

        return results?.ToArray() ?? Array.Empty<LinkTag>();
    }
}