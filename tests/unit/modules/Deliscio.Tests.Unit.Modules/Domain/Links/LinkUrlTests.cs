using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.Links;
using Xunit;

namespace Deliscio.Tests.Unit.Modules.Domain.Links;

public class LinkUrlTests
{
    private LinkUrl _testClass;
    private IFixture _fixture;
    private string _url;

    public LinkUrlTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _url = _fixture.Create<string>();
        _testClass = _fixture.Create<LinkUrl>();
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinkUrl(_url);

        // Assert
        Assert.NotNull(instance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Cannot_Construct_WithInvalid_Url(string value)
    {
        if (value is null)
            Assert.Throws<ArgumentNullException>(() => new LinkUrl(value));
        else
            Assert.Throws<ArgumentException>(() => new LinkUrl(value));
    }

    [Fact]
    public void CanGet_Value()
    {
        // Assert
        var result = Assert.IsType<string>(_testClass.Value);

       Assert.NotNull(result);
       Assert.NotEqual(string.Empty, result);
    }
}