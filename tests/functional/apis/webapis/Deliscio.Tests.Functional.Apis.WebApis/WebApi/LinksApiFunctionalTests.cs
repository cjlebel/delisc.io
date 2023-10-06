using System.Net;
using System.Text.Json;
using Deliscio.Apis.WebApi;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Apis.WebApis.WebApi;

// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public class LinksApiFunctionalTests : BaseApiFunctionalTests, IClassFixture<WebApplicationFactory<Program>>
{
    private const string GET_LINK_ENDPOINT = "/{0}/link/{1}";

    private const string GET_LINKS_ENDPOINT = "/{0}/links";
    private const string GET_LINKS_ENDPOINT_WITH_PAGENO = "/{0}/links?page={1}";
    private const string GET_LINKS_ENDPOINT_WITH_PAGENO_SIZE = "/{0}/links?page={1}&size={2}";
    private const string GET_LINKS_ENDPOINT_WITH_TAGS = "/{0}/links?tags={1}";
    private const string GET_LINKS_ENDPOINT_WITH_TAGS_PAGENO = "/{0}/links?tags={1}&page={2}";
    private const string GET_LINKS_ENDPOINT_WITH_ALL = "/{0}/links?page={1}&size={2}&search={3}&tags={4}";

    private const string GET_LINKS_RELATED_TAGS_ENDPOINT = "/{0}/links/tags?tags={1}";
    private const string GET_LINKS_RELATED_TAGS_WITH_COUNT_ENDPOINT = "/{0}/links/tags?tags={1}&count={2}";

    public LinksApiFunctionalTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    #region - GetLinkById -
    [Fact]
    public async Task WebApi_GetLinkById_ValidId_OkResult()
    {
        // Arrange
        var id = new Guid("7ea52a6c-e590-4cb5-8c2b-b41c05522e62");
        var expectedJson = "{\"id\": \"7ea52a6c-e590-4cb5-8c2b-b41c05522e62\",\"description\": \"The first 100 of you can use code SCHOOL2022 for 20% off courses and bundles at https://dometrain.comBecome a Patreon and get source code access: https://www...\",\"domain\": \"youtube.com\",\"imageUrl\": \"https://i.ytimg.com/vi/zCKwlgtVLnQ/maxresdefault.jpg\",\"keywords\": [],\"tags\": [  {    \"name\": \"csharp\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"dotnet\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"youtube\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"video\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"c#\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"technology\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"software\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"development\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"microsoft\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"nick chapsas\",    \"count\": 1,    \"weight\": 0  },  {    \"name\": \"software development\",    \"count\": 1,    \"weight\": 0  }],\"title\": \"The INSANE performance boost of LINQ in .NET 7 - YouTube\",\"url\": \"https://www.youtube.com/watch?v\\u003dzCKwlgtVLnQ\",\"submittedById\": \"48263056-61ad-b4a3-05e0-712025051842\",\"dateCreated\": \"2023-10-01T21:54:44.172+00:00\",\"dateUpdated\": \"2023-10-01T21:54:44.172+00:00\"\r\n    }";

        var expectedLink = JsonSerializer.Deserialize<Link>(expectedJson)!;

        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, id.ToString()));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var link = JsonSerializer.Deserialize<Link>(content);

        Assert.NotNull(link);
        Assert.Equal(expectedLink.Id, link.Id);
    }

    [Theory]
    [InlineData("fa431c01-992b-4773-a504-05b9b672a3b9")]
    public async Task WebApi_GetLinkById_ValidId_NotFound_OkResult(string invalidId)
    {
        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, invalidId)); // Replace with your endpoint URL

        // Assert
        // - If a link isn't found, an Ok is still returned (the endpoint itself exists).
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        // - However, the deserialized LinkItem will be null.
        var content = await response.Content.ReadAsStringAsync();
        Assert.Empty(content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task WebApi_GetLinkById_NullOrEmptyId_NotFoundResult(string invalidId)
    {
        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, invalidId)); // Replace with your endpoint URL

        // Assert
        // - This will automatically return a NotFound, without even hitting the endpoint.
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task WebApi_GetLinkById_GuidEmptyId_BadRequestResult(string emptyId)
    {
        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, emptyId)); // Replace with your endpoint URL

        // Assert
        // - This differs from WebApi_GetLinkById_NullOrEmptyId_NotFoundResult as it's a valid route, but not a valid Id
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task WebApi_GetLinkById_InvalidId_BadRequestResult()
    {
        // Arrange
        //var client = _factory.CreateClient();

        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, string.Empty));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    #endregion

    [Fact]
    public async Task WebApi_GetLinks_OkResult()
    {
        // Arrange
        var url = string.Format(GET_LINKS_ENDPOINT, API_VERSION);

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var actual = JsonSerializer.Deserialize<PagedResults<LinkItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(actual);
        Assert.Equal(DEFAULT_PAGE_NO, actual.PageNumber);
        Assert.Equal(DEFAULT_PAGE_SIZE, actual.PageSize);
        Assert.True(actual.IsSuccess);
        Assert.False(actual.IsError);

        Assert.NotNull(actual.Results);
        var links = actual.Results.ToList();

        Assert.Equal(DEFAULT_PAGE_SIZE, links.Count);
        Assert.Equal(actual.PageSize, links.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task WebApi_GetLinks_With_PageNo_OkResult(int? pageNo)
    {
        // Arrange
        var url = string.Format(GET_LINKS_ENDPOINT_WITH_PAGENO, API_VERSION, pageNo.ToString());

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var actual = JsonSerializer.Deserialize<PagedResults<LinkItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(actual);
        Assert.Equal(pageNo, actual.PageNumber);
        Assert.Equal(DEFAULT_PAGE_SIZE, actual.PageSize);
        Assert.True(actual.IsSuccess);
        Assert.False(actual.IsError);

        Assert.NotNull(actual.Results);
        var links = actual.Results.ToList();

        Assert.Equal(DEFAULT_PAGE_SIZE, links.Count);
        Assert.Equal(actual.PageSize, links.Count);
    }

    [Theory]
    [InlineData("recipes")]
    [InlineData("toronto,nba,raptors")]
    [InlineData("nick chapsas")]
    [InlineData("github, react")]
    public async Task WebApi_GetLinks_With_Tags_OkResult(string values)
    {
        // Arrange
        var tags = string.Join(',', values.Split(',').Select(x => x.Trim()).ToList());

        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINKS_ENDPOINT_WITH_TAGS, API_VERSION, tags));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var actual = JsonSerializer.Deserialize<PagedResults<LinkItem>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(actual);
        Assert.Equal(DEFAULT_PAGE_NO, actual.PageNumber);
        Assert.Equal(DEFAULT_PAGE_SIZE, actual.PageSize);
        Assert.True(actual.IsSuccess);
        Assert.False(actual.IsError);

        Assert.NotNull(actual.Results);
        var links = actual.Results.ToList();

        var expectedTags = tags.Split(',');
        foreach (var link in links)
        {
            var linkTags = link.Tags.Select(x => x.Name).ToArray();

            foreach (var expectedTag in expectedTags)
            {
                Assert.Contains(expectedTag, linkTags);
            }
        }
    }

    [Theory]
    [InlineData("dotnet, csharp")]
    [InlineData("github, react")]
    [InlineData("csharp")]
    public async Task WebApi_GetRelatedTags_OkResult(string values)
    {
        // Arrange
        var tags = string.Join(',', values.Split(',').Select(x => x.Trim()).ToList());

        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINKS_RELATED_TAGS_ENDPOINT, API_VERSION, tags));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var actuals = JsonSerializer.Deserialize<LinkTag[]>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(actuals);
        Assert.NotEmpty(actuals);

        decimal totalWeight = 0m;

        foreach (var actual in actuals)
        {
            Assert.True(actual.Count > decimal.Zero);

            Assert.True(actual.Weight > decimal.Zero);

            totalWeight += actual.Weight;
        }

        var delta = 0.00001m;
        Assert.True(decimal.One - totalWeight <= delta);
    }

    [Theory]
    [InlineData("dotnet, csharp", 16)]
    [InlineData("github, react", 23)]
    [InlineData("csharp", 2)]
    public async Task WebApi_GetRelatedTags_WithCount_OkResult(string values, int? count)
    {
        // Arrange
        var tags = string.Join(',', values.Split(',').Select(x => x.Trim()).ToList());

        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINKS_RELATED_TAGS_WITH_COUNT_ENDPOINT, API_VERSION, tags, count));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var actuals = JsonSerializer.Deserialize<LinkTag[]>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(actuals);
        Assert.NotEmpty(actuals);
        Assert.True(actuals.Length <= count);
        decimal totalWeight = 0m;

        foreach (var actual in actuals)
        {
            Assert.True(actual.Count > decimal.Zero);

            Assert.True(actual.Weight > decimal.Zero);

            totalWeight += actual.Weight;
        }

        var delta = 0.00001m;
        Assert.True(decimal.One - totalWeight <= delta);
    }

    [Theory]
    [InlineData("dotnet, csharp", 0)]
    [InlineData("github, react", -1)]
    [InlineData("csharp", null)]
    public async Task WebApi_GetRelatedTags_With_InvalidCount_BadRequestResult(string values, int? count)
    {
        // Arrange
        var tags = string.Join(',', values.Split(',').Select(x => x.Trim()).ToList());

        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_LINKS_RELATED_TAGS_WITH_COUNT_ENDPOINT, API_VERSION, tags, count));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}