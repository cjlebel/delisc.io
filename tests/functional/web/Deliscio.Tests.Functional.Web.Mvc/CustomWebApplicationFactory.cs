using Deliscio.Web.Site;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Deliscio.Tests.Functional.Web.Site;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

    }
}