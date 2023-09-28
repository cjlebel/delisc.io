using Deliscio.Apis.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Apis.WebApis.WebApi;

public class BaseApiFunctionalTests
{
    private readonly WebApplicationFactory<Program> _factory;

    protected const string API_VERSION = "v1";
    protected const int DEFAULT_PAGE_NO = 1;
    protected const int DEFAULT_PAGE_SIZE = 25;
    protected const int DEFAULT_TAG_COUNT = 50;
    protected const string DEFAULT_USER_ID = "48263056-61ad-b4a3-05e0-712025051842";

    protected readonly HttpClient HttpClient;

    protected BaseApiFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        HttpClient = _factory.CreateClient();
    }
}