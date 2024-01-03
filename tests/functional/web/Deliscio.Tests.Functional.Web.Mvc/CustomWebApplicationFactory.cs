using Deliscio.Web.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Deliscio.Tests.Functional.Web.Mvc;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

    }
}