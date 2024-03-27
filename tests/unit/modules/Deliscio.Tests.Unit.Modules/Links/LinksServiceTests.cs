using AutoFixture;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;

namespace Deliscio.Tests.Unit.Modules.Links;

public class LinksServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly LinksService _testClass;
    private readonly Mock<ILinksRepository> _linksRepository;
    private readonly Mock<ILogger<LinksService>> _logger;
    private readonly Mock<IOptions<MongoDbOptions>> _options;
    //private readonly IOptions<MongoDbOptions> _options;

    public LinksServiceTests()
    {
        _linksRepository = new Mock<ILinksRepository>();
        _logger = new Mock<ILogger<LinksService>>();
        _options = new Mock<IOptions<MongoDbOptions>>();
        //Options.Create(new MongoDbOptions
        //{
        //    ConnectionString = "mongodb://mongo:some-password-string@localhost:12345678",
        //    DatabaseName = "MongoTestDb"
        //});

        //_testClass = new LinksService(_options, _logger.Object);
        _testClass = new LinksService(_linksRepository.Object, _logger.Object);
    }

    [Fact]//(Skip = "Cannot run this as it will try to create an instance of the MongoDb client, which will throw an exception. Only way to avoid is to have a constructor that takes in a repo, and mock that repo. Do not want to change code so that I can test (code should be testable, but shouldn't be changed just to make a test work")]
    public void Can_Construct()
    {
        // Act
        var instance = new LinksService(_linksRepository.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Repository()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new LinksService(default, _logger.Object));

        Assert.Contains("Parameter 'linksRepository')", exception.Message);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new LinksService(_linksRepository.Object, default));

        Assert.Contains("Parameter 'logger')", exception.Message);
    }

    [Fact]
    public async Task GetAsync_CanCall_With_StringId()
    {
        // Arrange
        var linkEntity = _fixture.Create<LinkEntity>();
        linkEntity.Url = $"https://www.{linkEntity.Url}.com";
        linkEntity.ImageUrl = $"https://www.{linkEntity.ImageUrl}.com";

        var id = linkEntity.Id;

        _linksRepository.Setup(repo => repo.GetAsync(linkEntity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetAsync(id.ToString());

        // Assert
        Assert.NotNull(actual);
        AssertEntityToLinkModel(linkEntity, actual);
    }

    [Fact]
    public async Task GetAsync_CanCall_With_StringId_And_CancellationToken()
    {
        // Arrange
        var token = CancellationToken.None;
        var linkEntity = _fixture.Create<LinkEntity>();
        linkEntity.Url = $"https://www.{linkEntity.Url}.com";
        linkEntity.ImageUrl = $"https://www.{linkEntity.ImageUrl}.com";

        var id = linkEntity.Id;

        _linksRepository.Setup(repo => repo.GetAsync(linkEntity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetAsync(id.ToString(), token);

        // Assert
        Assert.NotNull(actual);
        AssertEntityToLinkModel(linkEntity, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetAsync_CannotCall_With_Invalid_StringId(string value)
    {
        // GetAsync uses Guard, which will throw a ArgumentException is the string is empty, but will throw a ArgumentNullException if the string is null.
        async void Checks()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _testClass.GetAsync(value));
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetAsync(value));
        }

        Assert.Multiple(Checks);
    }

    [Fact]
    public async Task GetAsync_CanCall_With_GuidId()
    {
        // Arrange
        var token = CancellationToken.None;
        var linkEntity = _fixture.Create<LinkEntity>();
        linkEntity.Url = $"https://www.{linkEntity.Url}.com";
        linkEntity.ImageUrl = $"https://www.{linkEntity.ImageUrl}.com";

        var id = linkEntity.Id;

        _linksRepository.Setup(repo => repo.GetAsync(linkEntity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetAsync(id.ToGuid(), token);

        // Assert
        Assert.NotNull(actual);
        AssertEntityToLinkModel(linkEntity, actual);
    }

    [Fact]
    public async Task GetAsync_Can_Call_With_PageNo_And_PageSize()
    {
        // Arrange
        var pageNo = 1303834936;
        var pageSize = 639612657;
        var totalPages = 34652;
        var totalResults = 3452345;

        var token = CancellationToken.None;
        var linkEntities = _fixture.Create<List<LinkEntity>>();
        var expected = (linkEntities, totalPages, totalResults);

        _linksRepository.Setup(repo => repo.FindAsync(_ => true, pageNo, pageSize, token)).ReturnsAsync(expected);

        // Act
        var actuals = await _testClass.GetAsync(pageNo, pageSize, token);

        // Assert
        AssertPageResultsOfLinkItems(linkEntities, actuals, pageNo, pageSize, totalResults);
    }

    [Fact]
    public async Task GetLinksByDomainAsync_Can_Call()
    {
        // Arrange
        var domain = "TestValue111279897";
        var pageNo = 1303834936;
        var pageSize = 639612657;
        var totalPages = 34652;
        var totalResults = 3452345;

        var token = CancellationToken.None;
        var linkEntities = _fixture.Create<List<LinkEntity>>();
        var expected = (linkEntities, totalPages, totalResults);

        _linksRepository.Setup(repo => repo.GetLinksByDomainAsync(domain, pageNo, pageSize, token)).ReturnsAsync(expected);

        // Act
        var actuals = await _testClass.GetLinksByDomainAsync(domain, pageNo, pageSize, token);

        // Assert
        AssertPageResultsOfLinkItems(linkEntities, actuals, pageNo, pageSize, totalResults);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetLinksByDomain_CannotCall_WithInvalid_Domain(string value)
    {
        // GetLinksByDomainAsync uses Guard, which will throw a ArgumentException is the string is empty, but will throw a ArgumentNullException if the string is null.
        async void Checks()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _testClass.GetLinksByDomainAsync(value, 1995334241, 501827622, CancellationToken.None));
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _testClass.GetLinksByDomainAsync(value, 1995334241, 501827622, CancellationToken.None));
        }

        Assert.Multiple(Checks);
    }

    [Fact]
    public async Task GetLinksByTagsAsync_CanCall()
    {
        // Arrange
        var tags = new[] { "TestValue275093999", "TestValue1977799120", "TestValue752175942" };
        var pageNo = 13;
        var pageSize = 13;
        var totalPages = 34652;
        var totalResults = 3452345;

        var token = CancellationToken.None;
        var linkEntities = _fixture.Create<List<LinkEntity>>();
        var expected = (linkEntities.AsEnumerable(), totalPages, totalResults);

        _linksRepository.Setup(repo => repo.GetLinksByTagsAsync(It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<int>(), token)).ReturnsAsync(expected);

        // Act
        var actuals = await _testClass.GetLinksByTagsAsync(tags.AsEnumerable(), pageNo, pageSize, token);

        // Assert
        _linksRepository.Verify(repo => repo.GetLinksByTagsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        AssertPageResultsOfLinkItems(linkEntities, actuals, pageNo, pageSize, totalResults);
    }

    [Fact]
    public async Task GetByTagsAsync_CannotCall_WithNull_Tags()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetLinksByTagsAsync(default, 283271778, 1594713334, CancellationToken.None));
    }

    [Fact]
    public async Task GetByUrlAsync_CanCall_With_Url()
    {
        // Arrange
        var linkEntity = _fixture.Create<LinkEntity>();
        linkEntity.Url = $"https://www.{linkEntity.Url}.com";
        linkEntity.ImageUrl = $"https://www.{linkEntity.ImageUrl}.com";

        var url = linkEntity.Url;

        _linksRepository.Setup(repo => repo.GetLinkByUrlAsync(url, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetByUrlAsync(url);

        // Assert
        Assert.NotNull(actual);
        AssertEntityToLinkModel(linkEntity, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetByUrlAsync_Cannot_Call_WithInvalid_Url(string value)
    {
        // GetLinksByDomainAsync uses Guard, which will throw a ArgumentException is the string is empty, but will throw a ArgumentNullException if the string is null.
        async void Checks()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _testClass.GetByUrlAsync(value));
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _testClass.GetByUrlAsync(value));
        }

        Assert.Multiple(Checks);
    }

    [Fact]
    public async Task SubmitLinkAsync_Can_Call_WithUrl_And_SubmittedByUserId_And_Tags_And_Token_LinkDoesNotExist_Add()
    {
        // Arrange
        var url = "TestValue615648565";
        var submittedByUserId = new Guid("a00b2194-0006-4cfe-9984-222377caaaae");
        var tags = new[] { "TestValue1509245304", "TestValue1745193086", "TestValue669590334" };
        var token = CancellationToken.None;

        // Null so that we can attempt to add a new Link
        LinkEntity? expected = default;

        _linksRepository.Setup(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        await Assert.ThrowsAsync<NotImplementedException>(() => _testClass.SubmitLinkAsync(url, submittedByUserId, tags, token));

        //_linksRepository.Verify(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        // Not yet implemented
        //Assert.Throws<NotImplementedException>(() => Assert.Equal(Guid.Empty, result));
    }

    [Fact]
    public async Task SubmitLinkAsync_Can_Call_WithUrl_And_SubmittedByUserId_And_Tags_And_Token_LinkExists_Update()
    {
        // Arrange
        var url = "TestValue615648565";
        var submittedByUserId = new Guid("a00b2194-0006-4cfe-9984-222377caaaae");
        var tags = new[] { "TestTag1509245304", "TestTag1745193086", "TestTag669590334" };
        var token = CancellationToken.None;
        var expected = new LinkEntity(Guid.NewGuid(), "TestUrl447647022", "TestTitle859312934", submittedByUserId);

        _linksRepository.Setup(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // The expected link should not have any tags to start with.
        Assert.Empty(expected.Tags);

        // Act
        var result = await _testClass.SubmitLinkAsync(url, submittedByUserId, tags, token);

        // Assert
        _linksRepository.Verify(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        Assert.Equal(expected.Id.ToGuid(), result);
        Assert.Equal(tags.Length, expected.Tags.Count);

        foreach (var tag in tags)
        {
            Assert.NotNull(expected.Tags.Find(t => t.Name == tag.ToLowerInvariant()));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SubmitLinkAsync_Cannot_Call_WithUrl_And_SubmittedByUserId_And_Tags_And_Token_WithInvalid_Url(string value)
    {
        //await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.SubmitLinkAsync(value, new Guid("ef747e5f-8c9b-4762-a1da-4316cb57c82e"), new[] { "TestValue1208235640", "TestValue1534039209", "TestValue945325384" }, CancellationToken.None));
    }

    [Fact]
    public async Task SubmitLinkAsync_CanCall_With_GuidId_And_Url_Expected_NewGuid()
    {
        // Arrange
        var submittedByUserId = Guid.NewGuid();
        var url = "http://www.fakesite.com";

        var expectedId = Guid.NewGuid();
        var expected = new LinkEntity(expectedId, "TestValue761697808", "TestValue1774356627", submittedByUserId);

        _linksRepository.Setup(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        var actual = await _testClass.SubmitLinkAsync(url, submittedByUserId);

        // Assert
        _linksRepository.Verify(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        Assert.Equal(expectedId, actual);
    }

    [Fact]
    public async Task SubmitLinkAsync_CanCall_With_GuidId_And_Url_Expected_EmptyGuid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var url = "http://www.fakesite.com";

        var expectedId = Guid.Empty;

        _linksRepository.Setup(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(LinkEntity));

        // Act
        await Assert.ThrowsAsync<NotImplementedException>(() => _testClass.SubmitLinkAsync(url, userId));

        // Assert
        //_linksRepository.Verify(mock => mock.GetLinkByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        //Assert.Equal(expectedId, actual);
    }

    [Fact]
    public async Task Cannot_Call_SubmitLinkAsync_With_Request_And_Token_WithNull_Request()
    {
        //await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.SubmitLinkAsync(default(SubmitLinkRequest), CancellationToken.None));
    }

    private void AssertPageResultsOfLinkItems(IEnumerable<LinkEntity>? expecteds, PagedResults<LinkItem> actuals, int expectedPageNo, int expectedPageSize, int expectedTotalResults)
    {
        Assert.NotNull(actuals);
        Assert.NotNull(actuals.Results);
        Assert.Equal(expectedPageNo, actuals.PageNumber);
        Assert.Equal(expectedPageSize, actuals.PageSize);

        var expectedPages = (int)Math.Ceiling((double)expectedTotalResults / expectedPageSize);
        Assert.Equal(expectedPages, actuals.TotalPages);
        Assert.Equal(expectedTotalResults, actuals.TotalResults);

        AssertEntitiesToLinkItemModels(expecteds, actuals.Results);
    }

    private void AssertEntitiesToLinkModels(IEnumerable<LinkEntity> entities, IEnumerable<Link> links)
    {
        Assert.NotNull(links);

        foreach (var link in links)
        {
            var entity = entities.FirstOrDefault(x => x.Id == new ObjectId(link.Id));

            Assert.NotNull(entity);

            AssertEntityToLinkModel(entity, link);
        }
    }

    private void AssertEntitiesToLinkItemModels(IEnumerable<LinkEntity> entities, IEnumerable<LinkItem> links)
    {
        Assert.NotNull(links);

        foreach (var link in links)
        {
            var entity = entities.FirstOrDefault(x => x.Id == new ObjectId(link.Id));

            Assert.NotNull(entity);

            AssertEntityToLinkItemModel(entity, link);
        }
    }

    private void AssertEntityToLinkModel(LinkEntity entity, Link link)
    {
        Assert.Equal(entity.Id.ToString(), link.Id);
        Assert.Equal(entity.SubmittedById.ToString(), link.SubmittedById);

        Assert.Equal(entity.Description, link.Description);
        Assert.Equal(entity.ImageUrl, link.ImageUrl);
        //Assert.Equal(linkEntity.IsE, actual.IsExcluded);

        Assert.Equal(entity.Tags.Count, link.Tags.Count);
        Assert.Equal(entity.Title, link.Title);
        Assert.Equal(entity.Url, link.Url);

        Assert.Equal(entity.DateCreated, link.DateCreated);
        Assert.Equal(entity.DateUpdated, link.DateUpdated);
    }

    private void AssertEntityToLinkItemModel(LinkEntity entity, LinkItem link)
    {
        Assert.Equal(entity.Id.ToString(), link.Id);
        //Assert.Equal(entity.SubmittedById.ToString(), link.SubmittedById);

        Assert.Equal(entity.Description, link.Description);
        Assert.Equal(entity.ImageUrl, link.ImageUrl);
        //Assert.Equal(linkEntity.IsE, actual.IsExcluded);

        Assert.Equal(entity.Tags.Count, link.Tags.Count);
        Assert.Equal(entity.Title, link.Title);
        Assert.Equal(entity.Url, link.Url);

        Assert.Equal(entity.DateCreated, link.DateCreated);
        //Assert.Equal(entity.DateUpdated, link.DateUpdated);
    }
}