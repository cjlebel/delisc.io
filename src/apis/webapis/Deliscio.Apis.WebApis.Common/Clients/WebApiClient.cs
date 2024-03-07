using System.Text.Json;
using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Common.Settings;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Deliscio.Apis.WebApi.Common.Clients;

public interface IWebApiClient
{
    Task<Link?> GetLinkAsync(Guid id, CancellationToken token = default);

    Task<PagedResults<LinkItem>?> GetLinksSearchResultsAsync(string? search = default, string? tags = default, int page = 1,
        int pageSize = 10, CancellationToken token = default);

    Task<IEnumerable<LinkTag>> GetRelatedTagsByTagsAsync(string? tags = default, int count = 100,
        CancellationToken token = default);
}

public class WebApiClient : BaseApiClient, IWebApiClient
{
    private const string VERSION = "v1";
    public WebApiClient(HttpClient httpClient, IOptions<WebApiSettings> apiSettings) : base(httpClient, apiSettings) { }

    public virtual async Task<Link?> GetLinkAsync(Guid id, CancellationToken token = default)
    {
        var response = await ApiClient.GetAsync($"{VERSION}/links/{id}", token);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(token);

            return JsonSerializer.Deserialize<Link>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return null;
    }

    public virtual async Task<PagedResults<LinkItem>?> GetLinksSearchResultsAsync(string? search = default, string? tags = default, int page = 1, int pageSize = 10, CancellationToken token = default)
    {
        var queryString = new Dictionary<string, string?>
        {
            { "search", search ?? string.Empty },
            { "tags", tags ?? string.Empty },
            { "page", page.ToString() },
            { "count", pageSize.ToString() }
                                       };

        //var url = $"{VERSION}/links?search={search}&tags={tags}&page={page}&count={pageSize}";
        var url = QueryHelpers.AddQueryString($"{VERSION}/links", queryString);
        var response = await ApiClient.GetAsync(url, token);
        PagedResults<LinkItem>? results = null;

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(token);

            if (!string.IsNullOrWhiteSpace(content))
            {
                results = JsonSerializer.Deserialize<PagedResults<LinkItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        return results ?? new PagedResults<LinkItem>();
    }

    public virtual async Task<IEnumerable<LinkTag>> GetRelatedTagsByTagsAsync(string? tags = default, int count = 100, CancellationToken token = default)
    {
        var url = $"{VERSION}/links/tags?tags={tags ?? string.Empty}&count={count}";
        var response = await ApiClient.GetAsync(url, token);
        IEnumerable<LinkTag>? results = null;

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(token);

            if (!string.IsNullOrWhiteSpace(content))
            {
                results = JsonSerializer.Deserialize<IEnumerable<LinkTag>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        return results ?? Array.Empty<LinkTag>();
    }
}