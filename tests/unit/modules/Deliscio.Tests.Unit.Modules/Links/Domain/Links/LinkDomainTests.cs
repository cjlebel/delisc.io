using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.Links;
using Xunit;

namespace Deliscio.Tests.Unit.Modules.Links.Domain.Links;

public class LinkDomainTests
{
    private readonly LinkDomain _testClass;
    private readonly IFixture _fixture;

    private readonly string _domain;
    private string _url;

    public LinkDomainTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _domain = _fixture.Create<string>();
        _url = $"https://www.{_domain}/some-url";
        _testClass = _fixture.Create<LinkDomain>();
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinkDomain(_domain);

        // Assert
        Assert.NotNull(instance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Cannot_Construct_WithInvalid_Domain(string? value)
    {
        if (value is null)
            Assert.Throws<ArgumentNullException>(() => new LinkDomain(value));
        else
            Assert.Throws<ArgumentException>(() => new LinkDomain(value));
    }

    [Fact]
    public void CanCall_FromUrl()
    {
        // Arrange
        var url = "https://www.delisc.io/some/url";

        // Act
        var result = LinkDomain.FromUrl(url);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("delisc.io", result.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CannotCall_FromUrl_WithInvalid_Url(string? value)
    {
        if (value is null)
            Assert.Throws<ArgumentNullException>(() => LinkDomain.FromUrl(value));
        else
            Assert.Throws<ArgumentException>(() => LinkDomain.FromUrl(value));
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