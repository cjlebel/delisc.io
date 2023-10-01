using System.Reflection;
using Deliscio.Apis.WebApi.Common.APIs;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Managers;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Commands.Handlers;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.Links.MediatR.Queries.Handlers;
using Deliscio.Modules.QueuedLinks;
using Deliscio.Modules.QueuedLinks.Harvester;
using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MassTransit.Models;
using Deliscio.Modules.QueuedLinks.MediatR.Commands;
using Deliscio.Modules.QueuedLinks.MediatR.Commands.Handlers;
using Deliscio.Modules.QueuedLinks.Tagger;
using Deliscio.Modules.QueuedLinks.Verifier;
using Deliscio.Modules.UserLinks;
using Deliscio.Modules.UserLinks.Common.Interfaces;
using Deliscio.Modules.UserLinks.Common.Models;
using Deliscio.Modules.UserLinks.Interfaces;
using Deliscio.Modules.UserLinks.MediatR.Commands;
using Deliscio.Modules.UserLinks.MediatR.Commands.Handlers;
using Deliscio.Modules.UserLinks.MediatR.Queries;
using Deliscio.Modules.UserLinks.MediatR.Queries.Handlers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Structurizr.Annotations;

namespace Deliscio.Apis.WebApi;

[Component(Description = "The Deliscio website's API service", Technology = "C#")]
[UsedByPerson("End Users", Description = "Public APIs")]
public class Program
{
    private const string API_VERSION = "v1";

    //public Program() { }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = ConfigSettingsManager.GetConfigs();
        builder.Services.Configure<MongoDbOptions>(config.GetSection(MongoDbOptions.SectionName));
        builder.Services.Configure<LinksQueueSettingsOptions>(config.GetSection(LinksQueueSettingsOptions.SectionName));

        // AddAsync services to the container.
        builder.Services.AddCors();
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(API_VERSION, new OpenApiInfo { Title = "Delisc.io", Version = API_VERSION });
            c.EnableAnnotations();
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        //builder.Services.ConfigureApiVersioning().A.AddApiVersioning(options =>
        //{
        //    options.DefaultApiVersion = new ApiVersion(1, 0);
        //    options.AssumeDefaultVersionWhenUnspecified = true;
        //    options.ReportApiVersions = true;
        //});

        builder.Services.AddSingleton<HttpClient>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<LinksQueueSettingsOptions>>().Value;

                cfg.Host(new Uri(options.Host), hostConfig =>
                {
                    hostConfig.Username(options.Username);
                    hostConfig.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
                // More options ...
            });
        });

        // Links
        builder.Services.AddSingleton<ILinksManager, LinksManager>();
        builder.Services.AddSingleton<ILinksService, LinksService>();
        builder.Services.AddSingleton<ILinksRepository, LinksRepository>();

        builder.Services.AddSingleton<IRequestHandler<GetLinkByIdQuery, Link?>, GetsLinkByIdQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinkByUrlQuery, Link?>, GetLinkByUrlQueryHandler>();

        builder.Services.AddSingleton<IRequestHandler<GetLinksByIdsQuery, IEnumerable<Link>>, GetLinksByIdsQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksQuery, PagedResults<Link>>, GetLinksQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksByDomainQuery, PagedResults<Link>>, GetLinksByDomainQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksByTagsQuery, PagedResults<Link>>, GetsLinksByTagsQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksRelatedTagsQuery, LinkTag[]>, GetLinksRelatedTagsQueryHandler>();

        builder.Services.AddSingleton<IRequestHandler<AddLinkCommand, Guid>, AddLinkCommandHandler>();
        builder.Services.AddSingleton<IRequestHandler<SubmitLinkCommand, Guid>, SubmitLinkCommandHandler>();

        // User Links
        builder.Services.AddSingleton<IUserLinksManager, UserLinksManager>();
        builder.Services.AddSingleton<IUserLinksService, UserLinksService>();
        builder.Services.AddSingleton<IUserLinksRepository, UserLinksRepository>();

        builder.Services.AddSingleton<IRequestHandler<GetUserLinkByIdQuery, UserLink?>, GetUserLinkByIdQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetUserLinksQuery, PagedResults<UserLink>>, GetUserLinksQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<AddLinkToUserCommand, Guid>, AddLinkToUserCommandHandler>();

        // Queue
        builder.Services.AddSingleton<IQueuedLinksService, QueuedLinksService>();

        // Adds a new link to the queue - currently masstransit/rabbitmq aren't co-operating
        builder.Services.AddSingleton<IRequestHandler<AddNewLinkQueueCommand, bool>, AddNewLinkQueueCommandHandler>();


        // This is weird, I should not need to add these here?!?!?
        builder.Services.AddSingleton<IVerifyProcessor, VerifyProcessor>();
        builder.Services.AddSingleton<IHarvesterProcessor, HarvesterProcessor>();
        builder.Services.AddSingleton<ITaggerProcessor, TaggerProcessor>();
        // This is weird, I should not need to add these here?!?!?


        builder.Services.AddSingleton<LinksApiEndpoints>();
        builder.Services.AddSingleton<UserLinksApiEndpoints>();

        var app = builder.Build();

        var linksApiEndpoints = app.Services.GetRequiredService<LinksApiEndpoints>();
        linksApiEndpoints.MapEndpoints(app);

        var userLinksApiEndpoints = app.Services.GetRequiredService<UserLinksApiEndpoints>();
        userLinksApiEndpoints.MapEndpoints(app);

        // Disabling for now to get Next working
        //app.UseHttpsRedirection();


        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // DO not use AllowAnyOrigin in production
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Delisc.io API {API_VERSION}");
            });
        }

        app.Run();
    }
}


