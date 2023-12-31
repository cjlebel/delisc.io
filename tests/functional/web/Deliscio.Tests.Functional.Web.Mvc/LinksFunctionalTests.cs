using Deliscio.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Web.Mvc;

public class LinksFunctionalTests : BaseWebFunctionalTests, IClassFixture<WebApplicationFactory<Program>>
{
    public LinksFunctionalTests(WebApplicationFactory<Program> factory) : base(factory) { }

    /// <summary>
    /// Calls the default /links route and checks that the page is returned correctly.
    /// - Title must contain "Links - Page 1 of ". Do not know how many pages there are, so cannot check that.
    /// - Description must contain "Links to useful resources".
    /// - Canonical must be the base for the /links.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Web_Links_Default_ReturnsOk()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/links");

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("<title>Links - Page 1 of ", responseString);
        Assert.Contains("<meta name=\"description\" content=\"Links to useful resources\"", responseString);
        Assert.Contains("<link rel=\"canonical\" href=\"https://deliscio.com/links\"", responseString);
    }

    /// <summary>
    /// Calls the /links route with a PAGE NUMBER and checks that the page is returned correctly.
    /// - Title must contain "Links - Page 2 of ". Do not know how many pages there are, so cannot check that.
    /// - Description must contain "Links to useful resources".
    /// - Canonical must be the base for the /links with the page query parameter set to 2.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Web_Links_WithPageNo_ReturnsOk()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/links?p=2");

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("<title>Links - Page 2 of ", responseString);
        Assert.Contains("<meta name=\"description\" content=\"Links to useful resources\"", responseString);
        Assert.Contains("<link rel=\"canonical\" href=\"https://deliscio.com/links?p=2\"", responseString);
    }

    /// <summary>
    /// Calls the /links route with TAGS and checks that the page is returned correctly.
    /// - Title must contain "Links for [INSERT TAGS] - Page 1 of ". Do not know how many pages there are, so cannot check that.
    /// - Description must contain "Links to useful resources".
    /// - Canonical must be the base for the /links with the tags parameter set and its values in alphabetical order.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Web_Links_WithTags_ReturnsOk()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/links?t=clean+architecture,software+development");

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("<title>Links for clean architecture, software development - Page 1 of", responseString);
        Assert.Contains("<meta name=\"description\" content=\"Links to useful resources\"", responseString);
        Assert.Contains("<link rel=\"canonical\" href=\"https://deliscio.com/links?t=clean+architecture,software+development\"", responseString);
    }

    /// <summary>
    /// Calls the /links route with a PAGE NUMBER and TAGS and checks that the page is returned correctly.
    /// - Title must contain "Links for [INSERT TAGS] - Page 2 of ". Do not know how many pages there are, so cannot check that.
    /// - Description must contain "Links to useful resources".
    /// - Canonical must be the base for the /links with the tags parameter (first) set and its values in alphabetical order, and then the page number parameter (second).
    /// </summary>
    /// <remarks>
    /// Notice how the page parameter in the request is first and the tags are not alphabetical.
    /// The site should set these properly regardless of the order they are passed in.
    /// </remarks>
    /// <returns></returns>
    [Fact]
    public async Task Web_Links_WithTagsAndPageNo_ReturnsOk()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/links?p=2&t=software+development,clean+architecture");

        // Act
        var response = await HttpClient.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("<title>Links for clean architecture, software development - Page 2 of ", responseString);
        Assert.Contains("<meta name=\"description\" content=\"Links to useful resources\"", responseString);
        Assert.Contains("<link rel=\"canonical\" href=\"https://deliscio.com/links?t=clean+architecture,software+development&p=2\"", responseString);
    }
}