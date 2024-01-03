using System.Net;
using System.Net.Http.Json;
using Deliscio.Apis.WebApi;
using Deliscio.Apis.WebApi.Common.Requests;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Apis.WebApis.WebApi;

public class AuthApiFunctionalTests : BaseApiFunctionalTests, IClassFixture<WebApplicationFactory<Program>>
{
    private const string REGISTER_USER_ENDPOINT = "/{0}/auth/register";

    public AuthApiFunctionalTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task AuthApi_RegisterUser_OkResult()
    {
        // Arrange
        var registerRequest = new RegisterRequest() { UserName = "deliscio", Email = "jason@delisc.io", Password = "AbC123!!!" };
        var apiUrl = string.Format(REGISTER_USER_ENDPOINT, API_VERSION);

        // Act
        var response = await HttpClient.PostAsJsonAsync(apiUrl, registerRequest);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Successfully registered!", await response.Content.ReadAsStringAsync());
    }
}