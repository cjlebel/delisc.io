using System.Security.Cryptography;
using System.Text;
using Deliscio.Core.Configuration;
using Deliscio.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Deliscio.Tests.Functional.Web.Mvc;

public class BaseWebFunctionalTests
{
    private readonly WebApplicationFactory<Program> _factory;

    protected const string API_VERSION = "v1";
    protected const int DEFAULT_PAGE_NO = 1;
    protected const int DEFAULT_PAGE_SIZE = 25;
    protected const int DEFAULT_TAG_COUNT = 50;

    protected readonly HttpClient HttpClient;
    protected readonly IConfiguration Config;
    protected BaseWebFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        HttpClient = _factory.CreateClient();

        //var builder = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json")
        //    .AddJsonFile("appsettings.development.json");

        //builder.AddEnvironmentVariables();
        //builder.Build();

        Config = ConfigSettingsManager.GetConfigs();
        var apiKey = Config.GetValue<string>("ApiKey");

        HttpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
    }

    protected static string GetUserId(string username)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = MD5.HashData(Encoding.Default.GetBytes(username));

        return new Guid(data).ToString();
    }
}