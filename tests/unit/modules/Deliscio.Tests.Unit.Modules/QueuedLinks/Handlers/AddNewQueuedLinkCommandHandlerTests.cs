using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MediatR.Commands;
using Deliscio.Modules.QueuedLinks.MediatR.Commands.Handlers;
using Moq;

namespace Deliscio.Tests.Unit.Modules.QueuedLinks.Handlers
{
    public class AddNewLinkQueueCommandHandlerTests
    {
        private readonly AddNewLinkQueueCommandHandler _testClass;
        private readonly Mock<IQueuedLinksService> _queueService;

        public AddNewLinkQueueCommandHandlerTests()
        {
            _queueService = new Mock<IQueuedLinksService>();
            _testClass = new AddNewLinkQueueCommandHandler(_queueService.Object);
        }

        [Fact]
        public void Can_Construct()
        {
            // Act
            var instance = new AddNewLinkQueueCommandHandler(_queueService.Object);

            // Assert
            Assert.NotNull(instance);
        }

        [Fact]
        public void Cannot_Construct_WithNull_QueueService()
        {
            Assert.Throws<ArgumentNullException>(() => new AddNewLinkQueueCommandHandler(default));
        }

        [Fact]
        public async Task Can_Call_HandleAsync()
        {
            // Arrange
            var command = new AddNewLinkQueueCommand(new QueuedLink());
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _testClass.Handle(command, cancellationToken);

            // Assert
            throw new NotImplementedException("Create or modify test");
        }

        [Fact]
        public async Task Cannot_Call_Handle_WithNull_Command()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.Handle(default, CancellationToken.None));
        }
    }
}