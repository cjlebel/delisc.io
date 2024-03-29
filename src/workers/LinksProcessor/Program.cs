using System.Reflection;
using Deliscio.Core.Configuration;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.Links.MediatR.Queries.Handlers;
using Deliscio.Modules.QueuedLinks;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Harvester;
using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MassTransit.Consumers;
using Deliscio.Modules.QueuedLinks.Verifier;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;

namespace Deliscio.Workers.LinksProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                var config = ConfigSettingsManager.GetConfigs();

                services.Configure<QueuedLinksSettingsOptions>(config.GetSection(QueuedLinksSettingsOptions.SectionName));

                services.AddSingleton<HttpClient>();

                services.AddSingleton<ILinksService, LinksService>();
                services.AddSingleton<ILinksRepository, LinksRepository>();

                services.AddSingleton<IQueuedLinksService, QueuedLinksService>();
                services.AddSingleton<IVerifyProcessor, VerifyProcessor>();
                services.AddSingleton<IHarvesterProcessor, HarvesterProcessor>();

                //services.AddScoped<AddNewQueuedLinkConsumer>();


                services.AddSingleton<IRequestHandler<GetLinkByIdQuery, Link?>, GetLinkByIdQueryHandler>();
                services.AddSingleton<IRequestHandler<GetLinkByUrlQuery, Link?>, GetLinkByUrlQueryHandler>();

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<AddNewQueuedLinkConsumer>();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var options = context.GetRequiredService<IOptions<QueuedLinksSettingsOptions>>().Value;

                        cfg.Host(new Uri(options.Host), hostConfig =>
                        {
                            hostConfig.Username(options.Username);
                            hostConfig.Password(options.Password);
                        });

                        //// More options ...

                        //cfg.ConfigureEndpoints(context);

                        cfg.ReceiveEndpoint(options.QueueName, e =>
                        {
                            e.ConfigureConsumer<AddNewQueuedLinkConsumer>(context);
                        });

                        //cfg.ConfigureEndpoints(context);

                        ////cfg.ReceiveEndpoint(options.QueueName, e =>
                        ////{
                        ////    e.Consumer<AddNewQueuedLinkConsumer>(context);
                        ////});


                    });

                    //x.RemoveMassTransitHostedService();
                });


                //var serviceProvider = services.BuildServiceProvider();
                //var busControl = serviceProvider.GetRequiredService<IBusControl>();
                //busControl.Start();

                services.AddHostedService<Worker>();
            })
            .Build();

        host.Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {

    }
}