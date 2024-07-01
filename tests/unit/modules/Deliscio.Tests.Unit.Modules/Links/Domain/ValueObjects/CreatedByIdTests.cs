using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using MongoDB.Bson;

namespace Deliscio.Tests.Unit.Modules.Links.Domain.ValueObjects;

public class CreatedByIdTests
{
    private readonly ObjectId _objectId;
    private readonly CreatedById _testClass;

    public CreatedByIdTests()
    {
        _objectId = ObjectId.GenerateNewId();
        _testClass = CreatedById.Create(_objectId);
    }

    [Fact]
    public void Implements_IEquatable_CreatedById()
    {
        // Arrange
        var same = CreatedById.Create(_objectId);
        var different = CreatedById.Create(ObjectId.GenerateNewId());

        // Assert
        Assert.False(_testClass.Equals(default(object)));
        Assert.False(_testClass.Equals(new object()));
        Assert.True(_testClass.Equals((object)same));
        Assert.False(_testClass.Equals((object)different));
        Assert.True(_testClass.Equals(same));
        Assert.False(_testClass.Equals(different));
        Assert.Equal(same.GetHashCode(), _testClass.GetHashCode());
        Assert.NotEqual(different.GetHashCode(), _testClass.GetHashCode());
        Assert.True(_testClass == same);
        Assert.False(_testClass == different);
        Assert.False(_testClass != same);
        Assert.True(_testClass != different);
    }

    [Fact]
    public void CanCall_Create_With_ObjectId()
    {
        // Arrange
        var value = ObjectId.GenerateNewId();

        // Act
        var result = CreatedById.Create(value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void CanCall_CreateWithString()
    {
        // Arrange
        var value = ObjectId.GenerateNewId().ToString();

        // Act
        var result = CreatedById.Create(value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid")]
    public void CannotCall_Create_With_String_WithInvalid_String_Value(string? value)
    {
        Assert.Throws<ArgumentException>(() => CreatedById.Create(value));
    }

    [Fact]
    public void CannotCall_Create_With_String_WithInvalid_ObjectId_Value()
    {
        var value = ObjectId.Empty;
        Assert.Throws<ArgumentException>(() => CreatedById.Create(value));
    }
}