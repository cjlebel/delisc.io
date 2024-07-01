using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.Links;
using Xunit;

namespace Deliscio.Tests.Unit.Modules.Domain.Links;

public class LinkTitleTests
{
    private LinkTitle _testClass;
    private IFixture _fixture;
    private string _title;

    public LinkTitleTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _title = _fixture.Create<string>();
        _testClass = _fixture.Create<LinkTitle>();
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinkTitle(_title);

        // Assert
        Assert.NotNull(instance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Cannot_Construct_WithInvalid_Title(string value)
    {
        if (value is null)
            Assert.Throws<ArgumentNullException>(() => new LinkTitle(value));
        else
            Assert.Throws<ArgumentException>(() => new LinkTitle(value));
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