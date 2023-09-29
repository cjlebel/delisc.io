using System.Security.Cryptography;
using System.Text;
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

    protected readonly HttpClient HttpClient;

    protected BaseApiFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        HttpClient = _factory.CreateClient();
    }

    protected static string GetUserId(string username)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = MD5.HashData(Encoding.Default.GetBytes(username));

        return new Guid(data).ToString();
    }
}