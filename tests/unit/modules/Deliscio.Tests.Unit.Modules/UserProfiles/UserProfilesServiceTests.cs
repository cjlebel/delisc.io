using System.Runtime.InteropServices;
using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.UserProfiles;
using Deliscio.Modules.UserProfiles.Common.Models.Requests;
using Deliscio.Modules.UserProfiles.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;

namespace Deliscio.Tests.Unit.Modules.UserProfiles;

public class UserProfilesServiceTests
{
    private UserProfilesService _testClass;
    private IFixture _fixture;
    private Mock<IUserProfilesRepository> _repository;
    private Mock<ILogger<UserProfilesService>> _logger;

    public UserProfilesServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repository = _fixture.Freeze<Mock<IUserProfilesRepository>>();
        _logger = _fixture.Freeze<Mock<ILogger<UserProfilesService>>>();
        _testClass = _fixture.Create<UserProfilesService>();
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new UserProfilesService(_repository.Object, _logger.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Cannot_Construct_WithNull_Repository()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new UserProfilesService(default(IUserProfilesRepository), _logger.Object));
    }

    [Fact]
    public void Cannot_Construct_WithNull_Logger()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new UserProfilesService(_repository.Object, default(ILogger<UserProfilesService>)));
    }

    [Fact]
    public async Task CanCall_AddAsync()
    {
        // Arrange
        var fixtureRequest = _fixture.Create<CreateUserProfileRequest>();

        // Needed to do this because fixture creates a Guid for the Id property
        var request = new CreateUserProfileRequest(
            ObjectId.GenerateNewId().ToString(),
            fixtureRequest.DisplayName,
            fixtureRequest.Email,
            fixtureRequest.DateRegistered
        );

        var token = _fixture.Create<CancellationToken>();

        // Act
        var result = await _testClass.AddAsync(request, token);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CannotCall_AddAsync_WithNull_Request()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _testClass.AddAsync(default(CreateUserProfileRequest), _fixture.Create<CancellationToken>()));
    }

    [Fact]
    public async Task CanCall_GetAsync()
    {
        // Arrange
        var userId = ObjectId.GenerateNewId().ToString();
        var token = _fixture.Create<CancellationToken>();

        var expectedProfileEntity = CreateProfileEntity();

        _repository.Setup(mock =>
            mock.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedProfileEntity);

        // Act
        var result = await _testClass.GetAsync(userId, token);

        // Assert
        _repository.Verify(mock => mock.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);

        var actualProfile = result.Value;

        Assert.NotNull(actualProfile);
        Assert.Equal(expectedProfileEntity.Id.ToString(), actualProfile.Id);
        Assert.Equal(expectedProfileEntity.Email, actualProfile.Email);
        Assert.Equal(expectedProfileEntity.DisplayName, actualProfile.DisplayName);
        Assert.Equal(expectedProfileEntity.DateRegistered, actualProfile.DateRegistered);
        Assert.Equal(expectedProfileEntity.FirstName, actualProfile.FirstName);
        Assert.Equal(expectedProfileEntity.LastName, actualProfile.LastName);
        Assert.Equal(expectedProfileEntity.ImageUrl, actualProfile.ImageUrl);
        Assert.Equal(expectedProfileEntity.Location, actualProfile.Location);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CannotCall_GetAsync_WithInvalid_UserId(string? value)
    {
        if (value is null)
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _testClass.GetAsync(value, _fixture.Create<CancellationToken>()));
        else
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _testClass.GetAsync(value, _fixture.Create<CancellationToken>()));
    }

    [Fact]
    public async Task CanCall_SearchAsync()
    {
        // Arrange
        var displayName = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var pageNo = 3;
        var pageSize = 25;
        var totalResults = 1000;
        var token = _fixture.Create<CancellationToken>();

        var expectedProfileEntities = new List<UserProfileEntity>();

        for (var i = 0; i < 25; i++)
        {
            expectedProfileEntities.Add(CreateProfileEntity());
        }

        var expectedResult = (expectedProfileEntities,
                pageNo,
                totalResults
            );

        _repository.Setup(mock =>
                mock.SearchAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _testClass.SearchAsync(displayName, email, pageNo, pageSize, token);

        // Assert
        _repository.Verify(mock => mock.SearchAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )
        );

        Assert.NotNull(result);
        Assert.Equal(pageNo, result.PageNumber);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(40, result.TotalPages);
        Assert.Equal(totalResults, result.TotalResults);
        Assert.Equal(expectedProfileEntities.Count, result.Items.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CannotCall_SearchAsync_WithInvalid_PageNo(int value)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _testClass.SearchAsync(
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                value,
                _fixture.Create<int>(),
                _fixture.Create<CancellationToken>())
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CannotCall_SearchAsync_WithInvalid_PageSize(int value)
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _testClass.SearchAsync(
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<int>(),
                value,
                _fixture.Create<CancellationToken>())
        );
    }

    private UserProfileEntity CreateProfileEntity()
    {
        var userProfileEntity = new UserProfileEntity(
            ObjectId.GenerateNewId().ToString(),
            $"{_fixture.Create<string>()}@delisc.io",
            _fixture.Create<string>(),
            _fixture.Create<DateTime>()
        )
        {
            FirstName = _fixture.Create<string>(),
            LastName = _fixture.Create<string>(),
            ImageUrl = _fixture.Create<string>(),
            Location = _fixture.Create<string>(),
        };

        return userProfileEntity;
    }
}