using Deliscio.Web.Site;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Web.Site;

public class HomeFunctionalTests : BaseWebFunctionalTests, IClassFixture<WebApplicationFactory<Program>>
{
    public HomeFunctionalTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Web_Home_Index_ReturnsOk()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/");

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("<title>Deliscio - Home", responseString);
        Assert.Contains("<meta name=\"description\" content=\"Deliscio - Home\"", responseString);
        Assert.Contains("<link rel=\"canonical\" href=\"https://deliscio.com\"", responseString);
    }
}