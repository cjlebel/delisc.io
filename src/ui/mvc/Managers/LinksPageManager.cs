using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Clients;
using Deliscio.Common.Extensions;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Web.Mvc.ViewModels.Links;
using MediatR;

namespace Deliscio.Web.Mvc.Managers;

public interface ILinksPageManager
{
    int DefaultPageSize { get; }

    /// <summary>
    /// Gets the View Model for the Links Index page
    /// </summary>
    /// <param name="pageNo">The page number of results (min 1)</param>
    /// <param name="pageSize">The size of the results for the page (min 1, default 50)</param>
    /// <param name="skip">The number of items to skip when applicable (default 0)</param>
    /// <param name="tags">A string of tags to filter by</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<LinksPageViewModel?> GetLinksPageViewModelAsync(int? pageNo = 1, int? skip = 0, string? tags = default, CancellationToken token = default);

    Task<LinksDetailsPageViewModel?> GetLinksDetailsPageViewModelAsync(Guid linkId, CancellationToken token = default);
}

public class LinksPageManager : BasePageManager, ILinksPageManager
{
    public int DefaultPageSize => 50;

    public LinksPageManager(IMediator mediator, ILogger<LinksPageManager>? logger) : base(mediator, logger) { }

    public async Task<LinksPageViewModel?> GetLinksPageViewModelAsync(int? pageNo = 1, int? skip = 0, string? tags = default, CancellationToken token = default)
    {
        var page = pageNo.GetValueOrDefault(1);
        var skipCount = Math.Max(skip.GetValueOrDefault(0), 0);

        Guard.Against.NegativeOrZero(page);
        Guard.Against.Negative(skipCount);

        tags = tags?.Trim() ?? string.Empty;

        var tagsArr = tags.GetArrayOrEmpty(',').OrderBy(t => t).ToArray();

        IRequest<PagedResults<LinkItem>> query = tags?.Length > 0 ?
            new GetLinksByTagsQuery(page, DefaultPageSize, tags) :
            new GetLinksQuery(page, DefaultPageSize);

        var results = await MediatR!.Send(query, token);

        var queryString = GetLinksQueryString(pageNo, skipCount, tagsArr);

        var pageTitle = tagsArr.Length > 0 ?
            $"Links for {string.Join(", ", tagsArr)} - Page {results.PageNumber} of {results.TotalPages}" :
            $"Links - Page {results.PageNumber} of {results.TotalPages}";

        var model = new LinksPageViewModel(results)
        {
            PageTitle = pageTitle,
            PageDescription = "Links to useful resources",
            CanonicalUrl = $"https://deliscio.com/links{queryString}",

            Tags = tagsArr!,
        };

        return model;
    }

    public async Task<LinksDetailsPageViewModel?> GetLinksDetailsPageViewModelAsync(Guid linkId, CancellationToken token = default)
    {
        var query = new GetLinkByIdQuery(linkId);

        var result = await MediatR!.Send(query, token);

        if (result is null)
            return default;

        var model = new LinksDetailsPageViewModel
        {
            PageTitle = result?.Title ?? string.Empty,
            PageDescription = result?.Description ?? string.Empty,
            CanonicalUrl = $"https://deliscio.com/links/{linkId}",

            Title = result.Title,
            Description = result.Description,
            ImageUrl = result.ImageUrl,
            Url = result.Url,
            Tags = result.Tags,
        };

        return model;
    }

    /// <summary>
    /// Builds the query string for the links page with the page number, skip count, and tags
    /// </summary>
    /// <returns>A string with any and all of the populated parameters</returns>
    private static string GetLinksQueryString(int? pageNo, int? skip, string[]? tags)
    {
        var queryString = string.Empty;

        if (tags?.Length > 0)
            queryString += $"{(string.IsNullOrEmpty(queryString) ? string.Empty : "&")}t={string.Join(',', tags).Replace(" ", "+")}";

        if (pageNo > 1)
            queryString += $"{(string.IsNullOrEmpty(queryString) ? string.Empty : "&")}p={pageNo}";

        if (skip > 0)
            queryString += $"{(string.IsNullOrEmpty(queryString) ? string.Empty : "&")}s={skip}";

        queryString = string.IsNullOrEmpty(queryString) ? string.Empty : $"?{queryString}";

        return queryString;
    }
}