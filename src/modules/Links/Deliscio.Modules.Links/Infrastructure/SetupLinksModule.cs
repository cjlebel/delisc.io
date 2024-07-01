using System.Reflection;
using Deliscio.Modules.Links.Application.Commands.SetLinkActivateState;
using Deliscio.Modules.Links.Application.Contracts;
using Deliscio.Modules.Links.Infrastructure.Data;
using Deliscio.Modules.Links.Infrastructure.Data.Mongo;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Deliscio.Modules.Links.Infrastructure;
public static class SetupLinksModule
{
    /// <summary>
    /// Registration for the Links module for public purposes
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterLinksModule(this IServiceCollection services, IConfiguration config)
    {
        RegisterModule(services, config);

        return services;
    }

    /// <summary>
    /// Registration for the Links module for Admin purposes
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterAdminLinksModule(this IServiceCollection services, IConfiguration config)
    {
        RegisterModule(services, config);

        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<IRequestHandler<SetLinkActiveStateCommand, Result>, SetLinkActiveStateCommandHandler>();

        /*
         Some services are not able to be constructed
        (Error while validating the service descriptor 'ServiceType: Deliscio.Modules.Links.Application.Contracts.ILinksModule Lifetime: Scoped
        ImplementationType: Deliscio.Modules.Links.LinksModule': Unable to resolve service for type 'MediatR.IMediator' while attempting to activate
        'Deliscio.Modules.Links.LinksModule'.) (Error while validating the service descriptor
        'ServiceType: MediatR.IRequestHandler`2[Deliscio.Modules.Links.Application.Commands.ActivateLink.ActivateLinkCommand,FluentResults.Result`1[System.Boolean]]
        Lifetime: Singleton ImplementationType: Deliscio.Modules.Links.Application.Commands.ActivateLink.ActivateLinkCommandHandler':
        Cannot consume scoped service 'Deliscio.Modules.Links.Common.Interfaces.ILinksRepository'
        from singleton 'MediatR.IRequestHandler`2[Deliscio.Modules.Links.Application.Commands.ActivateLink.ActivateLinkCommand,FluentResults.Result`1[System.Boolean]]'.)'

        */

        //services.AddOptions<MongoDbOptions>()
        //   .BindConfiguration(MongoDbOptions.SectionName);

        //var mongoDbAuthOptions = new MongoDbAuthOptions();
        //config.Bind(MongoDbAuthOptions.SectionName, mongoDbAuthOptions);

        //services.AddSingleton(config);



        return services;
    }

    /// <summary>
    /// Base registration for the Links module
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    private static void RegisterModule(IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<ILinksModule, LinksModule>();
        services.AddScoped<ILinksRepository, LinksRepository>();
    }
}
