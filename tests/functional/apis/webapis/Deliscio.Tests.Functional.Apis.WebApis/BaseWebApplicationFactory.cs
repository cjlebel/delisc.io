using Deliscio.Apis.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Deliscio.Tests.Functional.Apis.WebApis;

public class BaseWebApplicationFactory<T> : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json");
            config.AddJsonFile("appsettings.development.json");
        });

        return base.CreateHost(builder);
    }
}