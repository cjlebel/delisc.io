using Deliscio.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Deliscio.Apis.WebApi.Common.Abstracts;

public class BaseApiClient
{
    protected readonly HttpClient ApiClient;

    public BaseApiClient(HttpClient httpClient, IOptions<WebApiSettings> apiSettings)
    {
        ApiClient = httpClient;

        ApiClient.BaseAddress = new Uri(apiSettings.Value.BaseUrl);

        var apiKey = apiSettings.Value.ApiKey;

        ApiClient.DefaultRequestHeaders.Add("deliscio-api-key", apiKey);
        ApiClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Deliscio Web Api Client");
    }
}