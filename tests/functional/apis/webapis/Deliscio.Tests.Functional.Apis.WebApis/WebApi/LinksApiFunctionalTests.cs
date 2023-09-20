using System.Net;
using Deliscio.Apis.WebApi;
using Deliscio.Modules.Links.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Apis.WebApis.WebApi;

// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public class LinksApiFunctionalTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    private const string API_VERSION = "v1";
    private const string GET_LINK_ENDPOINT = "/{0}/links/{1}";

    public LinksApiFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task WebApi_GetLink_ValidId_Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, Guid.NewGuid())); // Replace with your endpoint URL

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var link = System.Text.Json.JsonSerializer.Deserialize<Link>(content);

        Assert.NotNull(link);
    }

    [Fact]
    public async Task WebApi_GetLink_NotFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, Guid.NewGuid())); // Replace with your endpoint URL

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task WebApi_GetLink_InvalidId_BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, string.Empty)); // Replace with your endpoint URL

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}