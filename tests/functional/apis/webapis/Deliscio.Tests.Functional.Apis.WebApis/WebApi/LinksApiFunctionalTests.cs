using System.Net;
using System.Text.Json;
using Deliscio.Apis.WebApi;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Apis.WebApis.WebApi;

// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public class LinksApiFunctionalTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    private const string API_VERSION = "v1";
    private const string GET_LINK_ENDPOINT = "/{0}/link/{1}";
    private const string GET_LINKS_ENDPOINT = "/{0}/links";
    private const string GET_LINKS_WITH_PAGENO_ENDPOINT = "/{0}/links/{1}";
    private const string GET_LINKS_WITH_PAGENO_PAGESIZE_ENDPOINT = "/{0}/links/{1}/{2}";
    private const string GET_LINKS_WITH_TAGS_ENDPOINT = "/{0}/links/{1}";
    private const string GET_LINKS_RELATED_TAGS_ENDPOINT = "/{0}/links/tags/{1}";

    private const int DEFAULT_PAGE_NO = 1;
    private const int DEFAULT_PAGE_SIZE = 25;

    public LinksApiFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task WebApi_GetLinkById_ValidId_OkResult()
    {
        // Arrange
        var id = new Guid("fa431c01-992b-4773-a504-05b9b672a3b2");
        var expectedJson =
            "{\"id\":\"fa431c01-992b-4773-a504-05b9b672a3b2\",\"description\":\"Offered by Duke University. Welcome to the second course in the Building Cloud Computing Solutions at Scale Specialization! In this course, ... Enroll for free.\",\"domain\":\"coursera.org\",\"imageUrl\":\"https://s3.amazonaws.com/coursera_assets/meta_images/generated/XDP/XDP~COURSE!~cloud-virtualization-containers-api-duke/XDP~COURSE!~cloud-virtualization-containers-api-duke.jpeg\",\"keywords\":[],\"tags\":[{\"name\":\"api\",\"count\":1,\"weight\":0},{\"name\":\"web services\",\"count\":1,\"weight\":0},{\"name\":\"integration\",\"count\":1,\"weight\":0},{\"name\":\"cloud computing\",\"count\":1,\"weight\":0},{\"name\":\"scalability\",\"count\":1,\"weight\":0},{\"name\":\"aws\",\"count\":1,\"weight\":0},{\"name\":\"azure\",\"count\":1,\"weight\":0}],\"title\":\"Cloud Virtualization, Containers and APIs | Coursera\",\"url\":\"https://www.coursera.org/learn/cloud-virtualization-containers-api-duke\",\"submittedById\":\"48263056-61ad-b4a3-05e0-712025051842\",\"dateCreated\":\"2023-09-26T01:24:45.185+00:00\",\"dateUpdated\":\"2023-09-26T01:24:45.185+00:00\"}";
        var expectedLink = JsonSerializer.Deserialize<Link>(expectedJson)!;


        // Act
        var response = await _client.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, id));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var link = JsonSerializer.Deserialize<Link>(content);

        Assert.NotNull(link);
        Assert.Equal(expectedLink.Id, link.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("fa431c01-992b-4773-a504-05b9b672a3b9")]
    public async Task WebApi_GetLinkById_NotFoundResult(string invalidId)
    {
        // Act
        var response = await _client.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, invalidId)); // Replace with your endpoint URL

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task WebApi_GetLinkById_InvalidId_BadRequestResult()
    {
        // Arrange
        //var client = _factory.CreateClient();

        // Act
        var response = await _client.GetAsync(string.Format(GET_LINK_ENDPOINT, API_VERSION, string.Empty));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(1, 10)]
    [InlineData(2, 16)]
    public async Task WebApi_GetLinks_OkResult(int? pageNo, int? pageSize)
    {
        // Arrange
        var url = pageNo != null && pageSize != null ? string.Format(GET_LINKS_WITH_PAGENO_PAGESIZE_ENDPOINT, API_VERSION, pageNo, pageSize) :
                pageNo != null ? string.Format(GET_LINKS_WITH_PAGENO_ENDPOINT, API_VERSION, pageNo) :
                string.Format(GET_LINKS_ENDPOINT, API_VERSION);

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var actual = JsonSerializer.Deserialize<PagedResults<Link>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(actual);
        Assert.Equal(pageNo ?? DEFAULT_PAGE_NO, actual.PageNumber);
        Assert.Equal(pageSize ?? DEFAULT_PAGE_SIZE, actual.PageSize);

        Assert.NotNull(actual.Results);
        var links = actual.Results.ToList();

        Assert.Equal(pageSize ?? DEFAULT_PAGE_SIZE, links.Count);
    }

    [Theory]
    [InlineData("nick chapsas")]
    [InlineData("github, react")]
    public async Task WebApi_GetLinksByTags_OkResult(string values)
    {
        // Arrange
        var tags = string.Join(',', values.Split(',').Select(x => x.Trim()).ToList());

        // Act
        var response = await _client.GetAsync(string.Format(GET_LINKS_WITH_TAGS_ENDPOINT, API_VERSION, tags));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var actual = JsonSerializer.Deserialize<PagedResults<Link>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(actual);
        Assert.Equal(DEFAULT_PAGE_NO, actual.PageNumber);
        Assert.Equal(DEFAULT_PAGE_SIZE, actual.PageSize);

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
    [InlineData(".net, c#", 16)]
    [InlineData("github, react", 23)]
    [InlineData("csharp", null)]
    public async Task WebApi_GetRelatedTags_OkResult(string values, int? count)
    {
        // Arrange
        var tags = string.Join(',', values.Split(',').Select(x => x.Trim()).ToList());

        // Act
        var response = await _client.GetAsync(string.Format(GET_LINKS_RELATED_TAGS_ENDPOINT, API_VERSION, tags));

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
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(",")]
    public async Task WebApi_GetLinksByTags_With_InvalidTags_OkResult(string values)
    {
        // Arrange
        var tags = string.Join(',', values.Split(',').Select(x => x.Trim()).ToList());

        // Act
        var response = await _client.GetAsync(string.Format(GET_LINKS_WITH_TAGS_ENDPOINT, API_VERSION, tags));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        response.EnsureSuccessStatusCode();
    }
}