using System.Net;
using System.Text.Json;
using Deliscio.Apis.WebApi;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Modules.Links.Common.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Deliscio.Tests.Functional.Apis.WebApis.WebApi;

public class UserLinksApiFunctionalTests : BaseApiFunctionalTests, IClassFixture<WebApplicationFactory<Program>>
{
    private const string GET_USERLINK_ENDPOINT = "/{0}/user/{1}/link/{2}";
    private const string GET_USERLINKS_ENDPOINT = "/{0}/users/{1}/links/";
    private const string GET_USERLINKS_WITH_PAGENO_ENDPOINT = "/{0}/users/links/{1}";
    private const string GET_USERLINKS_WITH_PAGENO_PAGESIZE_ENDPOINT = "/{0}/users/links/{1}/{2}";
    private const string POST_USERLINKS_ADD_LINK_ENDPOINT = "/{0}/users/{1}/links/add";

    private const string ID_CANNOT_BE_NULL_OR_WHITESPACE = "Id cannot be null or whitespace";
    private const string LINK_COULD_NOT_BE_FOUND = "The Link for Id {0} could not be found";
    private const string LINKS_COULD_NOT_BE_FOUND = "The Links for Page {0} could not be found";
    private const string PAGE_NO_CANNOT_BE_LESS_THAN_ONE = "PageNo cannot be less than 1";
    private const string PAGE_SIZE_CANNOT_BE_LESS_THAN_ONE = "PageSize cannot be less than 1";
    private const string TAGS_CANNOT_BE_NULL_OR_EMPTY = "Tags cannot be null or empty";
    private const string TAGS_COUNT_CANNOT_BE_LESS_THAN_ONE = "TagsCount cannot be less than 1";

    public UserLinksApiFunctionalTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task WebApi_GetUserLinkById_ValidId_OkResult()
    {
        // Arrange

        var linkId = new Guid("fa431c01-992b-4773-a504-05b9b672a3b2");
        var expectedJson =
            "{\"id\":\"fa431c01-992b-4773-a504-05b9b672a3b2\"," +
            "\"description\":\"Offered by Duke University. Welcome to the second course in the Building Cloud Computing Solutions at Scale Specialization! In this course, ... Enroll for free.\"," +
            "\"domain\":\"coursera.org\"," +
            "\"imageUrl\":\"https://s3.amazonaws.com/coursera_assets/meta_images/generated/XDP/XDP~COURSE!~cloud-virtualization-containers-api-duke/XDP~COURSE!~cloud-virtualization-containers-api-duke.jpeg\"," +
            "\"keywords\":[]," +
            "\"tags\":" +
                "[{\"name\":\"api\",\"count\":1,\"weight\":0}," +
                "{\"name\":\"web services\",\"count\":1,\"weight\":0}," +
                "{\"name\":\"integration\",\"count\":1,\"weight\":0}," +
                "{\"name\":\"cloud computing\",\"count\":1,\"weight\":0}," +
                "{\"name\":\"scalability\",\"count\":1,\"weight\":0}," +
                "{\"name\":\"aws\",\"count\":1,\"weight\":0}," +
                "{\"name\":\"azure\",\"count\":1,\"weight\":0}]," +
            "\"title\":\"Cloud Virtualization, Containers and APIs | Coursera\"," +
            "\"url\":\"https://www.coursera.org/learn/cloud-virtualization-containers-api-duke\"," +
            "\"submittedById\":\"48263056-61ad-b4a3-05e0-712025051842\"," +
            "\"dateCreated\":\"2023-09-26T01:24:45.185+00:00\"," +
            "\"dateUpdated\":\"2023-09-26T01:24:45.185+00:00\"}";

        var expectedLink = JsonSerializer.Deserialize<Link>(expectedJson)!;

        // Act
        var response = await GetUserLinkHttpResponseMessage(DEFAULT_USER_ID, linkId.ToString());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var link = JsonSerializer.Deserialize<Link>(content);

        Assert.NotNull(link);
        Assert.Equal(expectedLink.Id, link.Id);
    }

    [Fact]
    public async Task WebApi_GetUserLinkById_NotFound()
    {
        // Arrange
        var linkId = new Guid("fa431c01-992b-4773-a504-05b9b672a3b0");

        // Act
        var response = await HttpClient.GetAsync(string.Format(GET_USERLINK_ENDPOINT, API_VERSION, DEFAULT_USER_ID, linkId));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task WebApi_AddUserLink_OkResult()
    {
        var request = new AddUserLinkRequest
        {
            LinkId = "257e9c00-9a89-4487-b38a-200fcf75e79e",
            IsPrivate = false,
            Tags = new[] { "github", "azure", "virtualization" },
            Title = $"Updated - {DateTimeOffset.UtcNow}"
        };

        var response = await HttpClient.PostAsync(string.Format(POST_USERLINKS_ADD_LINK_ENDPOINT, API_VERSION, DEFAULT_USER_ID), new StringContent(string.Empty));
    }

    private async Task<HttpResponseMessage> GetUserLinkHttpResponseMessage(string userId, string linkId)
    {
        return await HttpClient.GetAsync(string.Format(GET_USERLINK_ENDPOINT, API_VERSION, userId, linkId));
    }
}