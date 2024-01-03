using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Web.Mvc.ViewModels.Home;
using MediatR;

namespace Deliscio.Web.Mvc.Managers;

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
        Guard.Against.Null(mediator);

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
        var query = new GetLinksQuery(1, _defaultPageSize);

        var results = await MediatR!.Send(query, token);

        var model = new HomePageViewModel
        {
            CanonicalUrl = "https://deliscio.com",
            PageTitle = "Deliscio - Home",
            PageDescription = "Deliscio - Home",

            Results = results
        };

        return model;
    }

    public async Task<LinkTag[]> GetTagsPageViewModelAsync(string? tags = default, CancellationToken token = default)
    {
        var tagsArr = !string.IsNullOrWhiteSpace(tags) ?
            tags.Split(',').OrderBy(t => t).ToArray() :
            Array.Empty<string>();

        var query = new GetRelatedTagsByTagsQuery(tagsArr, _defaultTagsSize);

        var results = await MediatR!.Send(query, token);

        return results;
    }
}