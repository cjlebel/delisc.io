using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.MediatR.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Deliscio.Tests.Integration.Web.Controllers;

public class BaseControllerTests
{
    protected readonly IMediator MediatR;

    protected BaseControllerTests()
    {
        MediatR = BuildMediator();
    }

    //TODO: Would like to make this common for web api, mvc, tests, ...
    private static IMediator BuildMediator()
    {
        var config = ConfigSettingsManager.GetConfigs();
        var mongoConfig = config.GetSection(MongoDbOptions.SectionName);

        var services = new ServiceCollection();

        services.Configure<MongoDbOptions>(mongoConfig);
        services.AddLogging();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(GetLinkByIdQuery).Assembly);
        });

        services.AddSingleton<ILinksService, LinksService>();
        services.AddSingleton<ILinksRepository, LinksRepository>();

        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<IMediator>();
    }
}