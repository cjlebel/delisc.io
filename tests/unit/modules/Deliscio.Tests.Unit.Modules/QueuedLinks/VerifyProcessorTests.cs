using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.QueuedLinks.Common.Enums;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Verifier;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Deliscio.Tests.Unit.Modules.QueuedLinks;

public class VerifyProcessorTests
{
    private VerifyProcessor _testClass;
    private Mock<IMediator> _mediator;
    private Mock<ILogger<VerifyProcessor>> _logger;
    private Mock<IOptions<QueuedLinksSettingsOptions>> _options;

    public VerifyProcessorTests()
    {
        _mediator = new Mock<IMediator>();
        _logger = new Mock<ILogger<VerifyProcessor>>();
        _options = new Mock<IOptions<QueuedLinksSettingsOptions>>();

        _testClass = new VerifyProcessor(_options.Object, _mediator.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new VerifyProcessor(_options.Object, _mediator.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Mediator()
    {
        Assert.Throws<ArgumentNullException>(() => new VerifyProcessor(_options.Object, default, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new VerifyProcessor(_options.Object, _mediator.Object, default));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Options()
    {
        Assert.Throws<ArgumentNullException>(() => new VerifyProcessor(default, _mediator.Object, _logger.Object));
    }

    [Fact]
    public async Task Can_Call_ExecuteAsync()
    {
        // Arrange
        var link = new QueuedLink();
        var token = CancellationToken.None;

        // Act
        var result = await _testClass.ExecuteAsync(link, token);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Cannot_Call_ExecuteAsync_WithNull_Link()
    {
        // Arrange
        Link? link = null;

        // Act
        var actual = await _testClass.ExecuteAsync(default);

        // Assert
        Assert.False(actual.IsSuccess);
        Assert.True(actual.Message.Contains("cannot be null"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ExecuteAsync_Cannot_Call_With_Invalid_Url(string url)
    {
        var link = new QueuedLink { Url = url };

        var actual = await _testClass.ExecuteAsync(link, CancellationToken.None);

        Assert.False(actual.IsSuccess);
        Assert.Equal(QueuedStates.Error, actual.Link.State);
    }
}