using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.Links;

namespace Deliscio.Tests.Unit.Modules.Domain.Links;

public class LinkDescriptionTests
{
    private LinkDescription _testClass;
    private IFixture _fixture;
    private string _value;

    public LinkDescriptionTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _value = _fixture.Create<string>();
        _testClass = _fixture.Create<LinkDescription>();
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinkDescription(_value);

        // Assert
        Assert.NotNull(instance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Can_Construct_WithNullOrEmpty_Value(string value)
    {
        var instance = new LinkDescription(value);

        Assert.NotNull(instance);
        Assert.Equal(string.Empty, instance.Value);
    }
}