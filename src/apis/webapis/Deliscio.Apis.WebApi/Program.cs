
using Deliscio.Apis.WebApi.Common.APIs;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Commands;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.MediatR.Handlers;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.MediatR.Commands;
using Deliscio.Modules.QueuedLinks.MediatR.Handlers;
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
        builder.Services.Configure<LinksQueueOptions>(config.GetSection(LinksQueueOptions.SectionName));
        // Add services to the container.
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

        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<LinksQueueOptions>>().Value;

                cfg.Host(options.Host);
                // More options ...
            });
        });

        builder.Services.AddSingleton<IRequestHandler<GetLinkByIdQuery, Link?>, GetLinkByIdQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinkByUrlQuery, Link?>, GetLinkByUrlQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksByDomainQuery, PagedResults<Link>>, GetLinkByDomainQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksByTagsQuery, PagedResults<Link>>, GetLinkByTagsQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<SubmitLinkCommand, Guid>, SubmitLinkCommandHandler>();


        builder.Services.AddSingleton<IRequestHandler<AddNewLinkQueueCommand, bool>, AddNewLinkQueueCommandHandler>();

        builder.Services.AddSingleton<ILinksRepository, LinksRepository>();

        builder.Services.AddSingleton<ILinksService, LinksService>();

        builder.Services.AddSingleton<LinksApiEndpoints>();

        var app = builder.Build();

        var linksApiEndpoints = app.Services.GetRequiredService<LinksApiEndpoints>();
        linksApiEndpoints.MapEndpoints(app);

        app.MapGet("/", () => "Hello World!");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Delisc.io API {API_VERSION}");
            });
        }

        app.Run();
    }
}


