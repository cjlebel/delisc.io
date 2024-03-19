using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.MediatR.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Deliscio.Tests.Integration.Web.Controllers;

public class BaseControllerTests
{
    protected IMediator MediatR { get; private set; }

    protected BaseControllerTests()
    {
        Init();
    }

    //TODO: Would like to make this common for web api, mvc, tests, ...
    private void Init()
    {
        var config = ConfigSettingsManager.GetConfigs();

        var mongoConfig = config.GetSection(MongoDbOptions.SectionName);

        var services = new ServiceCollection();

        //services.Configure<WebApiSettings>(
        //    config.GetSection(WebApiSettings.SectionName));

        //services.AddHttpClient<WebApiClient>();

        services.Configure<MongoDbOptions>(mongoConfig);
        services.AddLogging();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(GetLinkByIdQuery).Assembly);
        });

        services.AddSingleton<ILinksService, LinksService>();
        services.AddSingleton<ILinksRepository, LinksRepository>();

        var provider = services.BuildServiceProvider();

        MediatR = provider.GetRequiredService<IMediator>();
    }
}