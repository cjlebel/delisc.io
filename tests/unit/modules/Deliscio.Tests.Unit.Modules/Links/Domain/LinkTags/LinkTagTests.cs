using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.LinkTags;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;
using MongoDB.Bson;

namespace Deliscio.Tests.Unit.Modules.Links.Domain.LinkTags;

public class LinkTagTests
{
    private readonly LinkTag _testClass;
    private readonly IFixture _fixture;

    public LinkTagTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _testClass = LinkTag.Create("tag1", 1, 0.01M);
    }

    [Fact]
    public void CanCall_New()
    {
        // Arrange
        var name = _fixture.Create<string>();

        // Act
        var result = LinkTag.New(name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(1, result.Count);
        Assert.Equal(0, result.Weight);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CannotCall_New_WithInvalid_Name(string? value)
    {
        Assert.Throws<ArgumentException>(() => LinkTag.New(value));
    }

    [Fact]
    public void New_PerformsMapping()
    {
        // Arrange
        var name = _fixture.Create<string>();

        // Act
        var result = LinkTag.New(name);

        // Assert
        Assert.Same(name, result.Name);
    }

    [Fact]
    public void CanCall_Create()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var count = _fixture.Create<int>();
        var weight = _fixture.Create<decimal>();

        // Act
        var result = LinkTag.Create(name, count, weight);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(count, result.Count);
        Assert.Equal(weight, result.Weight);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CannotCall_Create_WithInvalid_Name(string? value)
    {
        Assert.Throws<ArgumentException>(() => LinkTag.Create(value, _fixture.Create<int>(), _fixture.Create<decimal>()));
    }

    [Fact]
    public void Create_PerformsMapping()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var count = _fixture.Create<int>();
        var weight = _fixture.Create<decimal>();

        // Act
        var result = LinkTag.Create(name, count, weight);

        // Assert
        Assert.Equal(name, result.Name);
        Assert.Equal(count, result.Count);
        Assert.Equal(weight, result.Weight);
    }

    [Fact]
    public void CanCall_Map()
    {
        // Arrange
        var entity = new LinkTagEntity(_fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<decimal>())
        {
            Id = ObjectId.GenerateNewId(),
            CreatedById = ObjectId.GenerateNewId(),
            DateCreated = DateTimeOffset.UtcNow.AddDays(-5),
            DateDeleted = DateTimeOffset.UtcNow.AddDays(-1),
            DateUpdated = DateTimeOffset.UtcNow.AddDays(-1),
            DeletedById = ObjectId.GenerateNewId(),
            IsDeleted = true,
            UpdatedById = ObjectId.GenerateNewId()
        };

        // Act
        var result = LinkTag.Map(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Name, result.Name);
        Assert.Equal(entity.Count, result.Count);
        Assert.Equal(entity.Weight, result.Weight);
    }

    [Fact]
    public void CannotCall_Map_WithNull_Entity_Returns_Null()
    {
        var result = LinkTag.Map(default);

        Assert.Null(result);
    }

    [Fact]
    public void Map_PerformsMapping()
    {
        // Arrange
        var entity = new LinkTagEntity(_fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<decimal>())
        {
            Id = ObjectId.GenerateNewId(),
            CreatedById = ObjectId.GenerateNewId(),
            DateCreated = DateTimeOffset.UtcNow.AddDays(-5),
            DateDeleted = DateTimeOffset.UtcNow.AddDays(-1),
            DateUpdated = DateTimeOffset.UtcNow.AddDays(-1),
            DeletedById = ObjectId.GenerateNewId(),
            IsDeleted = true,
            UpdatedById = ObjectId.GenerateNewId()
        };

        // Act
        var result = LinkTag.Map(entity);

        // Assert
        Assert.Same(entity.Name, result.Name);
        Assert.Equal(entity.Count, result.Count);
        Assert.Equal(entity.Weight, result.Weight);
    }

    [Fact]
    public void CanCall_IncreaseCount()
    {
        // Arrange
        var newTag = LinkTag.Create("tag1", 10, 0.01M);

        // Act
        newTag.IncreaseCount();

        // Assert
        Assert.Equal(11, newTag.Count);
    }

    [Fact]
    public void CanCall_DecreaseCount()
    {
        // Arrange
        var newTag = LinkTag.Create("tag1", 10, 0.01M);

        // Act
        newTag.DecreaseCount();

        // Assert
        Assert.Equal(9, newTag.Count);
    }

    [Fact]
    public void CanCall_DecreaseCount_Prevent_Negative()
    {
        // Arrange
        var newTag = LinkTag.Create("tag1", 0, 0.01M);

        // Act
        newTag.DecreaseCount();

        // Assert
        Assert.Equal(0, newTag.Count);
    }

    [Theory]
    [InlineData(1, 1.00)]   // 1 / 1 = 1
    [InlineData(2, 0.50)]   // 1 / 2 = 0.5
    [InlineData(3, 0.333)]   // 1 / 3 = 0.333
    [InlineData(5, 0.20)]   // 1 / 5 = 0.2
    [InlineData(10, 0.10)]  // 1 / 10 = 0.1
    [InlineData(50, 0.02)]  // 1 / 50 = 0.02
    [InlineData(100, 0.01)] // 1 / 100 = 0.01
    public void CanCall_CalculateWeight(int totalTagsCount, decimal expectedWeight)
    {
        // Act
        _testClass.CalculateWeight(totalTagsCount);

        // Assert
        Assert.NotNull(_testClass);
        Assert.Equal(expectedWeight, _testClass.Weight);
    }

    [Fact]
    public void CanGet_Name()
    {
        // Assert
        Assert.IsType<string>(_testClass.Name);

        Assert.True(!string.IsNullOrWhiteSpace(_testClass.Name));
    }

    [Fact]
    public void CanGet_Count()
    {
        // Assert
        Assert.IsType<int>(_testClass.Count);

        Assert.True(_testClass.Count > 0);
    }

    [Fact]
    public void CanSet_And_Get_Weight()
    {
        // Arrange
        var testValue = _fixture.Create<decimal>();

        // Act
        _testClass.Weight = testValue;

        // Assert
        Assert.Equal(testValue, _testClass.Weight);
    }
}