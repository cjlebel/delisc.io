using AutoFixture;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.Requests;
using Microsoft.Extensions.Logging;
using Moq;

namespace Deliscio.Tests.Unit.Modules;

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
    public async Task Can_Call_GetAsync_With_StringId_And_CancellationTokenAsync()
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
        CompareEntityToModel(linkEntity, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Cannot_Call_GetAsyncWithStringAndCancellationToken_WithInvalid_IdAsync(string value)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetAsync(value, CancellationToken.None));
    }

    [Fact]
    public async Task GetAsyncWithStringAndCancellationToken_PerformsMappingAsync()
    {
        // Arrange
        var id = "TestValue138292954";
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.GetAsync(id, token);

        // Assert
        Assert.Same(id, result.Id);
    }

    [Fact]
    public async Task Can_Call_GetAsyncWithGuidAndCancellationTokenAsync()
    {
        // Arrange
        var id = new Guid("8ef3f69e-8a82-42ac-b967-f18fc8455dbc");
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.GetAsync(id, token);

        // Assert
        throw new NotImplementedException("Create or modify test");
    }

    [Fact]
    public async Task GetAsyncWithGuidAndCancellationToken_PerformsMappingAsync()
    {
        // Arrange
        var id = new Guid("8f0afc88-4603-4741-b716-d57c899e7954");
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.GetAsync(id, token);

        // Assert
        Assert.Same(id, result.Id);
    }

    [Fact]
    public async Task Can_Call_GetAsyncWithPageNoAndPageSizeAndTokenAsync()
    {
        // Arrange
        var pageNo = 1303834936;
        var pageSize = 639612657;
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.GetAsync(pageNo, pageSize, token);

        // Assert
        throw new NotImplementedException("Create or modify test");
    }

    [Fact]
    public async Task GetAsyncWithPageNoAndPageSizeAndToken_PerformsMappingAsync()
    {
        // Arrange
        var pageNo = 499458953;
        var pageSize = 58751778;
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.GetAsync(pageNo, pageSize, token);

        // Assert
        Assert.Equal(pageSize, result.PageSize);
    }

    [Fact]
    public async Task Can_Call_GetByDomainAsync()
    {
        // Arrange
        var domain = "TestValue111279897";
        var pageNo = 567586492;
        var pageSize = 1390309897;
        var token = CancellationToken.None;

        //_linksRepository.Setup(mock => mock.GetByDomainAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new (IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)());

        // Act
        var result = await _testClass.GetByDomain(domain, pageNo, pageSize, token);

        // Assert
        _linksRepository.Verify(mock => mock.GetByDomainAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        throw new NotImplementedException("Create or modify test");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Cannot_Call_GetByDomain_WithInvalid_DomainAsync(string value)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetByDomain(value, 1995334241, 501827622, CancellationToken.None));
    }

    [Fact]
    public async Task GetByDomain_PerformsMappingAsync()
    {
        // Arrange
        var domain = "TestValue13672750";
        var pageNo = 1213946541;
        var pageSize = 1823502591;
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.GetByDomain(domain, pageNo, pageSize, token);

        // Assert
        Assert.Equal(pageSize, result.PageSize);
    }

    [Fact]
    public async Task Can_Call_GetByTagsAsync()
    {
        // Arrange
        var tags = new[] { "TestValue275093999", "TestValue1977799120", "TestValue752175942" };
        var pageNo = 766041297;
        var pageSize = 532585192;
        var token = CancellationToken.None;

        //_linksRepository.Setup(mock => mock.GetByTagsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new (IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)());

        // Act
        var result = await _testClass.GetByTags(tags, pageNo, pageSize, token);

        // Assert
        _linksRepository.Verify(mock => mock.GetByTagsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        throw new NotImplementedException("Create or modify test");
    }

    [Fact]
    public async Task Cannot_Call_GetByTags_WithNull_TagsAsync()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetByTags(default(IEnumerable<string>), 283271778, 1594713334, CancellationToken.None));
    }

    [Fact]
    public async Task GetByTags_PerformsMappingAsync()
    {
        // Arrange
        var tags = new[] { "TestValue1176806255", "TestValue1842352881", "TestValue586457334" };
        var pageNo = 83420035;
        var pageSize = 180700701;
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.GetByTags(tags, pageNo, pageSize, token);

        // Assert
        Assert.Equal(pageSize, result.PageSize);
    }

    [Fact]
    public async Task Can_Call_SubmitLinkAsyncWithUrlAndSubmittedByUserIdAndTagsAndTokenAsync()
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
    public async Task Cannot_Call_SubmitLinkAsyncWithUrlAndSubmittedByUserIdAndTagsAndToken_WithInvalid_UrlAsync(string value)
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

    private void CompareEntityToModel(LinkEntity entity, Link link)
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