using AutoFixture;

using Deliscio.Core.Models;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.Requests;

using Microsoft.Extensions.Logging;

using Moq;

namespace Deliscio.Tests.Unit.Modules.Links;

public class LinksServiceTests
{
    private readonly Fixture _fixture = new Fixture();
    private LinksService _testClass;
    private Mock<ILinksRepository> _linksRepository;
    private Mock<ILogger<LinksService>> _logger;

    public LinksServiceTests()
    {
        _linksRepository = new Mock<ILinksRepository>();
        _logger = new Mock<ILogger<LinksService>>();
        _testClass = new LinksService(_linksRepository.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinksService(_linksRepository.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_LinksRepository()
    {
        Assert.Throws<ArgumentNullException>(() => new LinksService(default, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new LinksService(_linksRepository.Object, default));
    }

    [Fact]
    public async Task GetAsync_CanCall_With_StringId_And_CancellationToken()
    {
        // Arrange
        var token = CancellationToken.None;
        var linkEntity = _fixture.Create<LinkEntity>();
        var id = linkEntity.Id;

        _linksRepository.Setup(repo => repo.GetAsync(linkEntity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetAsync(id.ToString(), token);

        // Assert
        Assert.NotNull(actual);
        AssertEntityToModel(linkEntity, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetAsync_CannotCall_With_Invalid_StringId_And_CancellationToken(string value)
    {
        // GetAsync uses Guard, which will throw a ArgumentException is the string is empty, but will throw a ArgumentNullException if the string is null.
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _testClass.GetAsync(value, CancellationToken.None));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _testClass.GetAsync(value, CancellationToken.None));
        });
    }

    [Fact]
    public async Task GetAsync_CanCall_With_StringId()
    {
        // Arrange
        var linkEntity = _fixture.Create<LinkEntity>();
        var id = linkEntity.Id;

        _linksRepository.Setup(repo => repo.GetAsync(linkEntity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetAsync(id.ToString());

        // Assert
        Assert.NotNull(actual);
        AssertEntityToModel(linkEntity, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetAsync_CannotCall_With_Invalid_StringId(string value)
    {
        // GetAsync uses Guard, which will throw a ArgumentException is the string is empty, but will throw a ArgumentNullException if the string is null.
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _testClass.GetAsync(value));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                 _testClass.GetAsync(value));
        });
    }

    [Fact]
    public async Task GetAsync_CanCall_With_GuidId_And_CancellationToken()
    {
        // Arrange
        var token = CancellationToken.None;
        var linkEntity = _fixture.Create<LinkEntity>();
        var id = linkEntity.Id;

        _linksRepository.Setup(repo => repo.GetAsync(linkEntity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetAsync(id, token);

        // Assert
        Assert.NotNull(actual);
        AssertEntityToModel(linkEntity, actual);
    }

    [Fact]
    public async Task GetAsync_Can_Call_With_PageNo_And_PageSize_And_CancellationToken()
    {
        // Arrange
        var pageNo = 1303834936;
        var pageSize = 639612657;
        var totalPages = 34652;
        var totalResults = 3452345;

        var token = CancellationToken.None;
        var linkEntities = _fixture.Create<IEnumerable<LinkEntity>>();

        _linksRepository.Setup(repo => repo.FindAsync(_ => true, pageNo, pageSize, It.IsAny<CancellationToken>())).ReturnsAsync((linkEntities, totalPages, totalResults));

        // Act
        var actuals = await _testClass.GetAsync(pageNo, pageSize, token);

        // Assert
        AssertPageResults(linkEntities, actuals, pageNo, pageSize, totalResults);
    }

    [Fact]
    public async Task GetByDomainAsync_Can_Call()
    {
        // Arrange
        var domain = "TestValue111279897";
        var pageNo = 1303834936;
        var pageSize = 639612657;
        var totalPages = 34652;
        var totalResults = 3452345;

        var token = CancellationToken.None;
        var linkEntities = _fixture.Create<IEnumerable<LinkEntity>>();

        _linksRepository.Setup(repo => repo.GetByDomainAsync(domain, pageNo, pageSize, It.IsAny<CancellationToken>())).ReturnsAsync((linkEntities, totalPages, totalResults));

        // Act
        var actuals = await _testClass.GetByDomainAsync(domain, pageNo, pageSize, token);

        // Assert
        AssertPageResults(linkEntities, actuals, pageNo, pageSize, totalResults);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetByDomain_CannotCall_WithInvalid_Domain(string value)
    {
        // GetByDomainAsync uses Guard, which will throw a ArgumentException is the string is empty, but will throw a ArgumentNullException if the string is null.
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _testClass.GetByDomainAsync(value, 1995334241, 501827622, CancellationToken.None));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _testClass.GetByDomainAsync(value, 1995334241, 501827622, CancellationToken.None));
        });
    }

    [Fact]
    public async Task GetByTagsAsync_CanCall()
    {
        // Arrange
        var tags = new[] { "TestValue275093999", "TestValue1977799120", "TestValue752175942" };
        var pageNo = 1303834936;
        var pageSize = 639612657;
        var totalPages = 34652;
        var totalResults = 3452345;

        var token = CancellationToken.None;
        var linkEntities = _fixture.Create<IEnumerable<LinkEntity>>();

        _linksRepository.Setup(repo => repo.GetByTagsAsync(tags, pageNo, pageSize, It.IsAny<CancellationToken>())).ReturnsAsync((linkEntities, totalPages, totalResults));

        // Act
        var actuals = await _testClass.GetByTagsAsync(tags, pageNo, pageSize, token);

        // Assert
        _linksRepository.Verify(mock => mock.GetByTagsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        AssertPageResults(linkEntities, actuals, pageNo, pageSize, totalResults);
    }

    [Fact]
    public async Task GetByTagsAsync_CannotCall_WithNull_Tags()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _testClass.GetByTagsAsync(default, 283271778, 1594713334, CancellationToken.None));
    }

    [Fact]
    public async Task GetByUrlAsync_CanCall_With_Url()
    {
        // Arrange
        var linkEntity = _fixture.Create<LinkEntity>();
        var url = linkEntity.Url;

        _linksRepository.Setup(repo => repo.GetByUrlAsync(url, It.IsAny<CancellationToken>())).ReturnsAsync(linkEntity);

        // Act
        var actual = await _testClass.GetByUrlAsync(url);

        // Assert
        Assert.NotNull(actual);
        AssertEntityToModel(linkEntity, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetByUrlAsync_Cannot_Call_WithInvalid_Url(string value)
    {
        // GetByDomainAsync uses Guard, which will throw a ArgumentException is the string is empty, but will throw a ArgumentNullException if the string is null.
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _testClass.GetByUrlAsync(value));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _testClass.GetByUrlAsync(value));
        });
    }

    [Fact]
    public async Task SubmitLinkAsync_Can_Call_WithUrl_And_SubmittedByUserId_And_Tags_And_Token()
    {
        // Arrange
        var url = "TestValue615648565";
        var submittedByUserId = new Guid("a00b2194-0006-4cfe-9984-222377caaaae");
        var tags = new[] { "TestValue1509245304", "TestValue1745193086", "TestValue669590334" };
        var token = CancellationToken.None;

        _linksRepository.Setup(mock => mock.GetByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new LinkEntity(Guid.NewGuid(), "TestValue447647022", "TestValue859312934"));

        // Act
        var result = await _testClass.SubmitLinkAsync(url, submittedByUserId, tags, token);

        // Assert
        _linksRepository.Verify(mock => mock.GetByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        throw new NotImplementedException("Create or modify test");
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
    public async Task Can_Call_SubmitLinkAsyncWithRequestAndTokenAsync()
    {
        // Arrange
        var request = new SubmitLinkRequest("TestValue769920427", "TestValue1564293322");
        var token = CancellationToken.None;

        _linksRepository.Setup(mock => mock.GetByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new LinkEntity(Guid.NewGuid(), "TestValue761697808", "TestValue1774356627"));

        // Act
        var result = await _testClass.SubmitLinkAsync(request, token);

        // Assert
        _linksRepository.Verify(mock => mock.GetByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        throw new NotImplementedException("Create or modify test");
    }

    [Fact]
    public async Task Cannot_Call_SubmitLinkAsyncWithRequestAndToken_WithNull_RequestAsync()
    {
        //await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.SubmitLinkAsync(default(SubmitLinkRequest), CancellationToken.None));
    }

    private void AssertPageResults(IEnumerable<LinkEntity>? expecteds, PagedResults<Link> actuals, int expectedPageNo, int expectedPageSize, int expectedTotalResults)
    {
        Assert.NotNull(actuals);
        Assert.NotNull(actuals.Results);
        Assert.Equal(expectedPageNo, actuals.PageNumber);
        Assert.Equal(expectedPageSize, actuals.PageSize);

        var expectedPages = (int)Math.Ceiling((double)expectedTotalResults / expectedPageSize);
        Assert.Equal(expectedPages, actuals.TotalPages);
        Assert.Equal(expectedTotalResults, actuals.TotalResults);

        AssertEntitiesToModels(expecteds, actuals.Results);
    }

    private void AssertEntitiesToModels(IEnumerable<LinkEntity> entities, IEnumerable<Link> links)
    {
        Assert.NotNull(links);

        foreach (var link in links)
        {
            var entity = entities.FirstOrDefault(x => x.Id == new Guid(link.Id));

            Assert.NotNull(entity);

            AssertEntityToModel(entity, link);
        }
    }

    private void AssertEntityToModel(LinkEntity entity, Link link)
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
}