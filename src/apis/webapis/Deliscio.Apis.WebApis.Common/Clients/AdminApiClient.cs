using Deliscio.Common.Settings;
using Microsoft.Extensions.Options;

namespace Deliscio.Apis.WebApi.Common.Clients;

public class AdminApiClient : WebApiClient
{
    public AdminApiClient(HttpClient httpClient, IOptions<WebApiSettings> apiSettings) : base(httpClient, apiSettings)
    {
    }
}