using Deliscio.Web.Site.Controllers;
using Deliscio.Web.Site.Managers;
using Deliscio.Web.Site.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deliscio.Tests.Unit.Web.Controllers;

public class HomeControllerTests
{
    private readonly HomeController _testClass;
    private readonly IFixture _fixture;
    private readonly Mock<IHomePageManager> _manager;
    private readonly Mock<ILogger<HomeController>> _logger;

    public HomeControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _manager = _fixture.Freeze<Mock<IHomePageManager>>();
        _logger = _fixture.Freeze<Mock<ILogger<HomeController>>>();

        _testClass = new HomeController(_manager.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new HomeController(_manager.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Manager()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeController(default(IHomePageManager), _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeController(_manager.Object, default(ILogger<HomeController>)));
    }

    [Fact]
    public async Task CanCall_Index()
    {
        // Arrange
        var token = _fixture.Create<CancellationToken>();
        var expected = _fixture.Create<HomePageViewModel>();

        _manager.Setup(mock => mock.GetHomePageViewModelAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        // Act
        var result = await _testClass.Index(token);

        // Assert
        _manager.Verify(mock => mock.GetHomePageViewModelAsync(It.IsAny<CancellationToken>()));

        Assert.NotNull(result);

        var viewResult = Assert.IsType<ViewResult>(result);

        var model = Assert.IsAssignableFrom<HomePageViewModel>(viewResult.Model);

        Assert.NotNull(model);
        Assert.Equal(expected, model);
    }

    [Fact]
    public void CanCall_Privacy()
    {
        // Act
        var result = _testClass.Privacy();

        // Assert
        Assert.NotNull(result);
    }

    [Fact(Skip = "Need a HttpContext")]
    public void CanCall_Error()
    {
        // Act
        var result = _testClass.Error();

        // Assert
        Assert.NotNull(result);
    }
}