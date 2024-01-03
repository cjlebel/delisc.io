using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Web.Mvc.Managers;
using Deliscio.Web.Mvc.ViewModels.Home;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Tests.Unit.Web.Managers;

public class HomePagePageManagerTests
{
    private readonly HomePagePageManager _testClass;
    private readonly IFixture _fixture;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<HomePagePageManager>> _logger;

    public HomePagePageManagerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mediator = _fixture.Freeze<Mock<IMediator>>();
        _logger = _fixture.Freeze<Mock<ILogger<HomePagePageManager>>>();
        _testClass = new HomePagePageManager(_mediator.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new HomePagePageManager(_mediator.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Mediator()
    {
        Assert.Throws<ArgumentNullException>(() => new HomePagePageManager(default(IMediator), _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new HomePagePageManager(_mediator.Object, default(ILogger<HomePagePageManager>)));
    }

    [Fact]
    public async Task CanCall_GetHomePageViewModelAsync()
    {
        // Arrange
        var token = _fixture.Create<CancellationToken>();

        var expected = _fixture.Create<PagedResults<LinkItem>>();

        _mediator.Setup(mock => mock.Send(It.IsAny<IRequest<PagedResults<LinkItem>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        var result = await _testClass.GetHomePageViewModelAsync(token);

        _mediator.Verify(mock => mock.Send(It.IsAny<IRequest<PagedResults<LinkItem>>>(), It.IsAny<CancellationToken>()));

        // Assert
        Assert.NotNull(result);

        Assert.Equal("https://deliscio.com", result.CanonicalUrl);
        Assert.Equal("Deliscio - Home", result.PageTitle);
        Assert.Equal("Deliscio - Home", result.PageDescription);

        Assert.NotNull(result.Results);
    }
}