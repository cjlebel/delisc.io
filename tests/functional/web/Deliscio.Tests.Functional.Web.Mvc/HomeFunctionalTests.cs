using Deliscio.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Web.Mvc;

public class HomeFunctionalTests : BaseWebFunctionalTests, IClassFixture<WebApplicationFactory<Program>>
{
    protected HomeFunctionalTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Web_Home_Index_OkResult()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/");

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("Hello, world!", responseString);
    }
}