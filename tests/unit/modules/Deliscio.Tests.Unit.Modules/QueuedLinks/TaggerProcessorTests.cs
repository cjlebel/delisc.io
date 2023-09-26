using System;
using System.Threading;
using System.Threading.Tasks;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Tagger;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Deliscio.Tests.Unit.Modules.QueuedLinks;

public class TaggerProcessorTests
{
    private TaggerProcessor _testClass;
    private Mock<IMediator> _mediator;
    private Mock<ILogger<TaggerProcessor>> _logger;

    public TaggerProcessorTests()
    {
        _mediator = new Mock<IMediator>();
        _logger = new Mock<ILogger<TaggerProcessor>>();
        _testClass = new TaggerProcessor(_mediator.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new TaggerProcessor(_mediator.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Mediator()
    {
        Assert.Throws<ArgumentNullException>(() => new TaggerProcessor(default, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new TaggerProcessor(_mediator.Object, default));
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
        throw new NotImplementedException("Create or modify test");
    }

    [Fact]
    public async Task Cannot_Call_ExecuteAsync_WithNull_LinkAsync()
    {
        //await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.ExecuteAsync(default(QueuedLink), CancellationToken.None));
    }
}
