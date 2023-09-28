using Deliscio.Apis.WebApi.Managers;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.UserLinks.Common.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Deliscio.Tests.Unit.WebApi.Managers;


public class UserLinksManagerTests
{
    private UserLinksManager _testClass;


    public UserLinksManagerTests()
    {
        //_testClass = new UserLinksManager();
    }

    [Fact]
    public void Can_Call_MergeLinks()
    {
        // Arrange
        var userLinks = new[] { new UserLink("TestValue1162122186", "TestValue1817217642", "TestValue2098139357", "TestValue948355735", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new[] { new UserLinkTag("TestValue2008117862", 129651501, 890673292.08M), new UserLinkTag("TestValue2125684488", 1064184623, 235036394.51M), new UserLinkTag("TestValue1790349074", 722250116, 424097939.43M) }, true), new UserLink("TestValue293164977", "TestValue1796895937", "TestValue658328225", "TestValue375559266", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new[] { new UserLinkTag("TestValue1317241111", 876818579, 2022999045.21M), new UserLinkTag("TestValue2045491088", 882743398, 1300831781.04M), new UserLinkTag("TestValue132178644", 1310171993, 137864765.61M) }, false), new UserLink("TestValue229784538", "TestValue1901145646", "TestValue1202579683", "TestValue1842624330", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new[] { new UserLinkTag("TestValue37500421", 1646842492, 1518546194.55M), new UserLinkTag("TestValue85846818", 829500554, 1235182037.76M), new UserLinkTag("TestValue961793394", 1237282302, 1049497565.49M) }, true) };
        var links = new[] { new Link("TestValue786421659", "TestValue1926347119", "TestValue1391876377"), new Link("TestValue1759105162", "TestValue1276182065", "TestValue1430197956"), new Link("TestValue32667201", "TestValue2111749425", "TestValue2111349667") };

        // Act
        var result = _testClass.MergeLinks(userLinks, links);

        // Assert
        throw new NotImplementedException("Create or modify test");
    }

    [Fact]
    public void Cannot_Call_MergeLinks_WithNull_UserLinks()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.MergeLinks(default(IEnumerable<UserLink>), new[] { new Link("TestValue1633076982", "TestValue2026674981", "TestValue1942686944"), new Link("TestValue62487416", "TestValue1264114745", "TestValue934474244"), new Link("TestValue784167861", "TestValue816177091", "TestValue697945869") }));
    }

    [Fact]
    public void Cannot_Call_MergeLinks_WithNull_Links()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.MergeLinks(new[] { new UserLink("TestValue2137877809", "TestValue1195885313", "TestValue1895899950", "TestValue1310522623", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new[] { new UserLinkTag("TestValue1182314463", 2095208391, 677902198.05M), new UserLinkTag("TestValue929022912", 1776530051, 205244977.41M), new UserLinkTag("TestValue967395383", 1558153248, 1545637023.81M) }, true), new UserLink("TestValue1964203675", "TestValue1825483082", "TestValue1259069429", "TestValue1103879926", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new[] { new UserLinkTag("TestValue2069578766", 520557129, 33650028.72M), new UserLinkTag("TestValue856543030", 1818601125, 262863572.4M), new UserLinkTag("TestValue1937279626", 1716986348, 1943798666.04M) }, false), new UserLink("TestValue1584355076", "TestValue269770300", "TestValue1298856221", "TestValue45332693", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, new[] { new UserLinkTag("TestValue431481533", 263847525, 1234419258.6M), new UserLinkTag("TestValue2011696885", 469533994, 1607148855.18M), new UserLinkTag("TestValue744032656", 1444151633, 1063611912.33M) }, true) }, default(IEnumerable<Link>)));
    }
}