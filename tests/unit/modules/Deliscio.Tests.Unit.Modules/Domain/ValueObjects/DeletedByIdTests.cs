using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using MongoDB.Bson;

namespace Deliscio.Tests.Unit.Modules.Domain.ValueObjects;

public class DeletedByIdTests
{
    private readonly ObjectId _objectId;
    private readonly DeletedById _testClass;

    public DeletedByIdTests()
    {
        _objectId = ObjectId.GenerateNewId();
        _testClass = DeletedById.Create(_objectId);
    }

    [Fact]
    public void Implements_IEquatable_DeletedById()
    {
        // Arrange
        var same = DeletedById.Create(_objectId);
        var different = DeletedById.Create(ObjectId.GenerateNewId());

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
        var result = DeletedById.Create(value);

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
        var result = DeletedById.Create(value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid")]
    public void CannotCall_Create_With_String_WithInvalid_String_Value(string value)
    {
        if (value is null)
            Assert.Throws<ArgumentNullException>(() => DeletedById.Create(value));
        else
            Assert.Throws<ArgumentException>(() => DeletedById.Create(value));
    }

    [Fact]
    public void CannotCall_Create_With_String_WithInvalid_ObjectId_Value()
    {
        var value = ObjectId.Empty;
        var deletedById = DeletedById.Create(value);

        Assert.Equal(ObjectId.Empty, deletedById.Value);
    }
}