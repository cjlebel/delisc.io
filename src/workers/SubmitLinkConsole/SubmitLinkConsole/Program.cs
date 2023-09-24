// See https://aka.ms/new-console-template for more information

using Deliscio.Modules.QueuedLinks.MassTransit.Consumers;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Deliscio.Core.Configuration;
using Deliscio.Modules.QueuedLinks.MassTransit.Models;
using Microsoft.Extensions.Options;

Console.WriteLine("Hello, World!");
try
{
    var builder = WebApplication.CreateBuilder(args);

    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, collection) =>
        {
            var config = ConfigSettingsManager.GetConfigs();

            collection.Configure<LinksQueueSettingsOptions>(config.GetSection(LinksQueueSettingsOptions.SectionName));

            collection.AddSingleton<HttpClient>();

            collection.AddHttpContextAccessor();

            collection.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();
                x.AddConsumer<AddNewQueuedLinkConsumer>();

                //x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    var options = context.Configuration; //..Configuration..GetRequiredService<IOptions<LinksQueueSettingsOptions>>().Value;
                    //cfg.Host(new Uri(options.Host), hostConfig =>
                    //{
                    //    hostConfig.Username(options.Username);
                    //    hostConfig.Password(options.Password);
                    //});

                    cfg.UseDelayedMessageScheduler();
                    instance.ConfigureEndpoints(context);
                    //cfg.ServiceInstance(instance =>
                    //{
                    //    instance.ConfigureJobServiceEndpoints();
                    //    //instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));

                    //});
                });
            });
        }).Build();

    await host.RunAsync();
}
catch (Exception ex)
{

}
finally
{

}