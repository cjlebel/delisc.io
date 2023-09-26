using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Harvester;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Deliscio.Tests.Unit.Modules.QueuedLinks;

public class HarvesterProcessorTests
{
    private HarvesterProcessor _testClass;
    private Mock<IMediator> _mediator;
    private HttpClient _httpClient;
    private Mock<ILogger<HarvesterProcessor>> _logger;

    public HarvesterProcessorTests()
    {
        _mediator = new Mock<IMediator>();
        _httpClient = new HttpClient();
        _logger = new Mock<ILogger<HarvesterProcessor>>();
        _testClass = new HarvesterProcessor(_mediator.Object, _httpClient, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new HarvesterProcessor(_mediator.Object, _httpClient, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Mediator()
    {
        Assert.Throws<ArgumentNullException>(() => new HarvesterProcessor(default, _httpClient, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_HttpClient()
    {
        Assert.Throws<ArgumentNullException>(() => new HarvesterProcessor(_mediator.Object, default, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new HarvesterProcessor(_mediator.Object, _httpClient, default));
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
