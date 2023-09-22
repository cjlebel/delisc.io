using Deliscio.Core.Configuration;
using Deliscio.Modules.QueuedLinks.Common.Models;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Deliscio.Workers.LinksProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
            })
            .Build();

        host.Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var config = ConfigSettingsManager.GetConfigs();
        services.Configure<LinksQueueOptions>(config.GetSection(LinksQueueOptions.SectionName));

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<LinksQueueOptions>>().Value;

                cfg.Host(options.Host);
                // More options ...
            });
        });
    }
}