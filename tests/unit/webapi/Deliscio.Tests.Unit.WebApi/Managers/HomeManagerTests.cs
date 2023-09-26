using Deliscio.Apis.WebApi.Managers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Deliscio.Tests.Unit.WebApi.Managers;

public class HomeManagerTests
{
    private HomeManager _testClass;
    private Mock<IMediator> _mediator;
    private Mock<IBusControl> _bus;
    private Mock<ILogger<HomeManager>> _logger;

    public HomeManagerTests()
    {
        _mediator = new Mock<IMediator>();
        _bus = new Mock<IBusControl>();
        _logger = new Mock<ILogger<HomeManager>>();
        _testClass = new HomeManager(_mediator.Object, _bus.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new HomeManager(_mediator.Object, _bus.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Mediator()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeManager(default(IMediator), _bus.Object, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Bus()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeManager(_mediator.Object, default(IBusControl), _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeManager(_mediator.Object, _bus.Object, default(ILogger<HomeManager>)));
    }
}