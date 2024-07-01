using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.Links;

namespace Deliscio.Tests.Unit.Modules.Links.Domain.Links;

public class LinkImageUrlTests
{
    private readonly LinkImageUrl _testClass;
    private readonly IFixture _fixture;
    private readonly string _value;

    public LinkImageUrlTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _value = _fixture.Create<string>();
        _testClass = _fixture.Create<LinkImageUrl>();
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinkImageUrl(_value);

        // Assert
        Assert.NotNull(instance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Can_Construct_WithNullOrEmpty_Value(string? value)
    {
        var instance = new LinkImageUrl(value);

        Assert.NotNull(instance);
        Assert.Equal(string.Empty, instance.Value);
    }
}