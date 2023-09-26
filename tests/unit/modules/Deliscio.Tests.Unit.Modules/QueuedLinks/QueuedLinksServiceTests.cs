using Deliscio.Modules.QueuedLinks;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Harvester;
using Deliscio.Modules.QueuedLinks.Tagger;
using Deliscio.Modules.QueuedLinks.Verifier;

using Microsoft.Extensions.Logging;

using Moq;

namespace Deliscio.Tests.Unit.Modules.QueuedLinks;

public class QueuedLinksServiceTests
{
    private QueuedLinksService _testClass;
    private Mock<IVerifyProcessor> _verifier;
    private Mock<IHarvesterProcessor> _harvester;
    private Mock<ITaggerProcessor> _tagger;
    private Mock<ILogger<QueuedLinksService>> _logger;

    public QueuedLinksServiceTests()
    {
        _verifier = new Mock<IVerifyProcessor>();
        _harvester = new Mock<IHarvesterProcessor>();
        _tagger = new Mock<ITaggerProcessor>();
        _logger = new Mock<ILogger<QueuedLinksService>>();
        _testClass = new QueuedLinksService(_verifier.Object, _harvester.Object, _tagger.Object, _logger.Object);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new QueuedLinksService(_verifier.Object, _harvester.Object, _tagger.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Verifier()
    {
        Assert.Throws<ArgumentNullException>(() => new QueuedLinksService(default, _harvester.Object, _tagger.Object, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Harvester()
    {
        Assert.Throws<ArgumentNullException>(() => new QueuedLinksService(_verifier.Object, default, _tagger.Object, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Tagger()
    {
        Assert.Throws<ArgumentNullException>(() => new QueuedLinksService(_verifier.Object, _harvester.Object, default, _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() => new QueuedLinksService(_verifier.Object, _harvester.Object, _tagger.Object, default));
    }

    [Fact]
    public async Task ProcessNewLinkAsync_Can_Call()
    {
        // Arrange
        var link = new QueuedLink();
        var token = CancellationToken.None;

        _harvester.Setup(mock => mock.ExecuteAsync(It.IsAny<QueuedLink>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<(bool IsSuccess, string Message, QueuedLink Link)>());
        _tagger.Setup(mock => mock.ExecuteAsync(It.IsAny<QueuedLink>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<(bool IsSuccess, string Message, QueuedLink Link)>());
        _verifier.Setup(mock => mock.ExecuteAsync(It.IsAny<QueuedLink>(), It.IsAny<CancellationToken>())).Returns(new ValueTask<(bool IsSuccess, string Message, QueuedLink Link)>());

        // Act
        var result = await _testClass.ProcessNewLinkAsync(link, token);

        // Assert
        _harvester.Verify(mock => mock.ExecuteAsync(It.IsAny<QueuedLink>(), It.IsAny<CancellationToken>()));
        _tagger.Verify(mock => mock.ExecuteAsync(It.IsAny<QueuedLink>(), It.IsAny<CancellationToken>()));
        _verifier.Verify(mock => mock.ExecuteAsync(It.IsAny<QueuedLink>(), It.IsAny<CancellationToken>()));

        throw new NotImplementedException("Create or modify test");
    }

    [Fact]
    public async Task Cannot_Call_ProcessNewLinkAsync_WithNull_LinkAsync()
    {
        //await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.ProcessNewLinkAsync(default(QueuedLink), CancellationToken.None));
    }
}