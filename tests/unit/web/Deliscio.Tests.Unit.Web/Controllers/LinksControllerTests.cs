using System.Net;
using Deliscio.Web.Mvc.Controllers;
using Deliscio.Web.Mvc.Managers;
using Deliscio.Web.Mvc.ViewModels.Links;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deliscio.Tests.Unit.Web.Controllers;

public class LinksControllerTests
{
    private readonly LinksController _testClass;
    private readonly IFixture _fixture;
    private readonly Mock<ILinksPageManager> _linksManager;
    private readonly Mock<ILogger<LinksController>> _logger;

    public LinksControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _linksManager = _fixture.Freeze<Mock<ILinksPageManager>>();
        _linksManager.Setup(mock => mock.DefaultPageSize).Returns(50);

        _logger = _fixture.Freeze<Mock<ILogger<LinksController>>>();
        _testClass = new LinksController(_linksManager.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinksController(_linksManager.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_LinksManager()
    {
        Assert.Throws<ArgumentNullException>(() => new LinksController(default(ILinksPageManager), _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new LinksController(_linksManager.Object, default(ILogger<LinksController>)));
    }

    [Theory]
    [InlineData(1, 10, 0, "tag1, tag5, tag2, tag6")]
    [InlineData(default, 10, 0, "tag1, tag5, tag2, tag6")]
    [InlineData(2, default, 0, "tag1, tag5, tag2, tag6")]
    [InlineData(3, 10, 10, "tag1, tag5, tag2, tag6")]
    [InlineData(3, 10, default, "tag1, tag5, tag2, tag6")]
    [InlineData(1, 10, 0, default)]
    [InlineData(default, default, default, default)]
    public async Task CanCall_Index(int? page, int? count, int? skip, string? tags)
    {
        // Arrange
        var token = _fixture.Create<CancellationToken>();

        var expected = _fixture.Create<LinksPageViewModel>();

        _linksManager.Setup(mock => mock.GetLinksPageViewModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        var result = await _testClass.Index(page, skip, tags, token);

        // Assert
        _linksManager.Verify(mock => mock.GetLinksPageViewModelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        Assert.IsType<ViewResult>(result);

        var model = Assert.IsAssignableFrom<LinksPageViewModel>(((ViewResult)result).Model);
        Assert.NotNull(model);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99)]
    public async Task CannotCall_Index_WithInvalid_PageNo(int value)
    {
        var result = await _testClass.Index(value, _fixture.Create<int?>(), _fixture.Create<string?>(), _fixture.Create<CancellationToken>());

        Assert.NotNull(result);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal((int)HttpStatusCode.BadRequest, badRequest.StatusCode.Value);
        Assert.Equal("Page number must be greater than 0.", badRequest.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-99)]
    public async Task CannotCall_Index_WithInvalid_Skip(int value)
    {
        var result = await _testClass.Index(_fixture.Create<int?>(), value, _fixture.Create<string?>(), _fixture.Create<CancellationToken>());

        Assert.NotNull(result);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal((int)HttpStatusCode.BadRequest, badRequest.StatusCode.Value);
        Assert.Equal("Skip must be greater or equal to 0.", badRequest.Value);
    }
}