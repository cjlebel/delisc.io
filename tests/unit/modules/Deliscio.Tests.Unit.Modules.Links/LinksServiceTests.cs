using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Data.Entities;
using Deliscio.Modules.Links.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deliscio.Tests.Unit.Modules.Links
{
    /// <summary>
    /// Unit tests for the type <see cref="LinksService"/>.
    /// </summary>
    public class LinksServiceTests
    {
        private LinksService _testClass;
        private Mock<ILinksRepository> _linksRepository;
        private Mock<ILogger<LinksService>> _logger;

        /// <summary>
        /// Sets up the dependencies required for the tests for <see cref="LinksService"/>.
        /// </summary>
        public LinksServiceTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _linksRepository = fixture.Freeze<Mock<ILinksRepository>>();
            _logger = fixture.Freeze<Mock<ILogger<LinksService>>>();
            _testClass = fixture.Create<LinksService>();
        }

        /// <summary>
        /// Checks that instance construction works.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new LinksService(_linksRepository.Object, _logger.Object);

            // Assert
            Assert.NotNull(instance);
        }

        /// <summary>
        /// Checks that instance construction throws when the linksRepository parameter is null.
        /// </summary>
        [Fact]
        public void CannotConstruct_WithNull_LinksRepository()
        {
            Assert.Throws<ArgumentNullException>(() => new LinksService(default(ILinksRepository), _logger.Object));
        }

        /// <summary>
        /// Checks that instance construction throws when the logger parameter is null.
        /// </summary>
        [Fact]
        public void CannotConstruct_WithNull_Logger()
        {
            Assert.Throws<ArgumentNullException>(() => new LinksService(_linksRepository.Object, default(ILogger<LinksService>)));
        }

        /// <summary>
        /// Checks that the GetAsync method functions correctly.
        /// </summary>
        /// <returns>A task that represents the running test.</returns>
        [Fact]
        public async Task GetAsyncWithIdAndToken_CanCall()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var id = fixture.Create<string>();
            var token = fixture.Create<CancellationToken>();

            // Act
            var result = await _testClass.GetAsync(id, token);

            // Assert
            throw new NotImplementedException("Create or modify test");
        }

        /// <summary>
        /// Checks that the GetAsync method throws when the id parameter is null, empty or white space.
        /// </summary>
        /// <param name="value">The parameter that receives the test case values.</param>
        /// <returns>A task that represents the running test.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetAsyncWithIdAndToken_CannotCall_WithInvalid_Id(string value)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetAsync(value, fixture.Create<CancellationToken>()));
        }

        /// <summary>
        /// Checks that the GetAsync maps values from the input to the returned instance.
        /// </summary>
        /// <returns>A task that represents the running test.</returns>
        [Fact]
        public async Task GetAsyncWithIdAndToken_PerformsMapping()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var id = fixture.Create<string>();
            var token = fixture.Create<CancellationToken>();

            // Act
            var result = await _testClass.GetAsync(id, token);

            // Assert
            Assert.Same(id, result.Id);
        }

        /// <summary>
        /// Checks that the GetAsync method functions correctly.
        /// </summary>
        /// <returns>A task that represents the running test.</returns>
        [Fact]
        public async Task GetAsyncWithPageNoAndPageSizeAndToken_CanCall()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var pageNo = fixture.Create<int>();
            var pageSize = fixture.Create<int>();
            var token = fixture.Create<CancellationToken>();

            // Act
            var result = await _testClass.GetAsync(pageNo, pageSize, token);

            // Assert
            throw new NotImplementedException("Create or modify test");
        }

        /// <summary>
        /// Checks that the GetByDomain method functions correctly.
        /// </summary>
        /// <returns>A task that represents the running test.</returns>
        [Fact]
        public async Task GetByDomain_CanCall()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var domain = fixture.Create<string>();
            var pageNo = fixture.Create<int>();
            var pageSize = fixture.Create<int>();
            var token = fixture.Create<CancellationToken>();

            _linksRepository.Setup(mock => mock.GetByDomainAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(fixture.Create<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)>());

            // Act
            var result = await _testClass.GetByDomain(domain, pageNo, pageSize, token);

            // Assert
            _linksRepository.Verify(mock => mock.GetByDomainAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

            throw new NotImplementedException("Create or modify test");
        }

        /// <summary>
        /// Checks that the GetByDomain method throws when the domain parameter is null, empty or white space.
        /// </summary>
        /// <param name="value">The parameter that receives the test case values.</param>
        /// <returns>A task that represents the running test.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetByDomain_CannotCall_WithInvalid_Domain(string value)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetByDomain(value, fixture.Create<int>(), fixture.Create<int>(), fixture.Create<CancellationToken>()));
        }

        /// <summary>
        /// Checks that the GetByTags method functions correctly.
        /// </summary>
        /// <returns>A task that represents the running test.</returns>
        [Fact]
        public async Task GetByTags_CanCall()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var tags = fixture.Create<IEnumerable<string>>();
            var pageNo = fixture.Create<int>();
            var pageSize = fixture.Create<int>();
            var token = fixture.Create<CancellationToken>();

            // Act
            var result = await _testClass.GetByTags(tags, pageNo, pageSize, token);

            // Assert
            throw new NotImplementedException("Create or modify test");
        }

        /// <summary>
        /// Checks that the GetByTags method throws when the tags parameter is null.
        /// </summary>
        /// <returns>A task that represents the running test.</returns>
        [Fact]
        public async Task GetByTags_CannotCall_WithNull_Tags()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.GetByTags(default(IEnumerable<string>), fixture.Create<int>(), fixture.Create<int>(), fixture.Create<CancellationToken>()));
        }
    }
}