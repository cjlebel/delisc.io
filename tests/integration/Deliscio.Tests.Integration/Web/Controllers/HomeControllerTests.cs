using Deliscio.Web.Mvc.Controllers;
using Deliscio.Web.Mvc.Managers;
using Deliscio.Web.Mvc.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deliscio.Tests.Integration.Web.Controllers;

public class HomeControllerTests : BaseControllerTests
{
    private readonly HomeController _controller;
    private readonly IHomePageManager _homePageManager;
    private readonly ILogger<HomeController> _logger;

    public HomeControllerTests() : base()
    {
        _logger = new Logger<HomeController>(new LoggerFactory());
        _homePageManager = new HomePagePageManager(MediatR, default);
        _controller = new HomeController(_homePageManager, _logger);
    }

    /// <summary>
    /// Tests the ViewResult of the Index action (and NOT the HTML that would get rendered)
    /// </summary>
    [Fact]
    public async Task HomeController_Index_Returns_ViewModel()
    {
        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);

        // ViewData
        Assert.NotNull(viewResult.ViewData);
        Assert.NotNull(viewResult.ViewData.Values);
        Assert.Equal(1, viewResult.ViewData.Values.Count);

        // Breadcrumbs
        var breadCrumbs = viewResult.ViewData["Breadcrumbs"] as Dictionary<string, string>;
        Assert.NotNull(breadCrumbs);
        Assert.Single(breadCrumbs);
        Assert.Equal("Home", breadCrumbs.Keys.First());
        Assert.Equal("/", breadCrumbs.Values.First());

        // Model
        var model = Assert.IsAssignableFrom<HomePageViewModel>(viewResult.Model);
        Assert.NotNull(model);
        Assert.NotEqual(string.Empty, model.PageDescription);
        Assert.NotEqual(string.Empty, model.PageTitle);

        var links = model.Results;

        Assert.Equal(1, links.PageNumber);
        Assert.Equal(50, links.PageSize);
        Assert.NotNull(links.Results);
        Assert.True(links.Results.Count <= 50);
        Assert.True(links.TotalResults > 50);
    }
}