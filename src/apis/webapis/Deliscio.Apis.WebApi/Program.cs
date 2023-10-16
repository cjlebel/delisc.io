using System.Reflection;
using AspNetCore.Identity.MongoDbCore.Models;
using Deliscio.Apis.WebApi.Common.APIs;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Managers;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Middleware;
using Deliscio.Core.Models;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Commands.Handlers;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.Links.MediatR.Queries.Handlers;
using Deliscio.Modules.QueuedLinks;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Harvester;
using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MediatR.Commands;
using Deliscio.Modules.QueuedLinks.MediatR.Commands.Handlers;
using Deliscio.Modules.QueuedLinks.Tagger;
using Deliscio.Modules.QueuedLinks.Verifier;
using Deliscio.Modules.UserLinks;
using Deliscio.Modules.UserLinks.Common.Interfaces;
using Deliscio.Modules.UserLinks.Common.Models;
using Deliscio.Modules.UserLinks.MediatR.Commands;
using Deliscio.Modules.UserLinks.MediatR.Commands.Handlers;
using Deliscio.Modules.UserLinks.MediatR.Queries;
using Deliscio.Modules.UserLinks.MediatR.Queries.Handlers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
        var mongoConfig = config.GetSection(MongoDbOptions.SectionName);

        builder.Services.Configure<MongoDbOptions>(mongoConfig);
        builder.Services.Configure<QueuedLinksSettingsOptions>(config.GetSection(QueuedLinksSettingsOptions.SectionName));



        builder.Services.AddCors();
        //builder.Services.AddAuthentication();
        //builder.Services.AddAuthorization();

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

        //builder.Services.ConfigureApiVersioning().AddApiVersioning(options =>
        //{
        //    options.DefaultApiVersion = new ApiVersion(1, 0);
        //    options.AssumeDefaultVersionWhenUnspecified = true;
        //    options.ReportApiVersions = true;
        //});

        builder.Services.AddSingleton<HttpClient>();

        // Tried to get an extension method to work, but no luck so far.
        // Moving on so that I can be productive elsewhere
        //builder.Services.AddMongoDbSingleton(config);

        builder.Services.AddSingleton<MongoDbClient, MongoDbClient>(sp =>
        {
            var mongoDbOptions = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
            return new MongoDbClient(mongoDbOptions);
        });

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<QueuedLinksSettingsOptions>>().Value;

                cfg.Host(new Uri(options.Host), hostConfig =>
                {
                    hostConfig.Username(options.Username);
                    hostConfig.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
                // More options ...
            });
        });

        builder.Services.AddSingleton<IConfiguration>(config);

        builder.Services.AddIdentity<AuthUser, MongoIdentityRole>()
            // Wasn't able to pass the client in, had to do it this way for now
            // sp => sp.GetRequiredService<MongoDbClient>().Database
            .AddMongoDbStores<AuthUser, MongoIdentityRole, Guid>(mongoConfig["ConnectionString"], mongoConfig["DatabaseName"])
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "DeliscioTastyCookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Adjust the expiration time as needed
                options.SlidingExpiration = true;
                options.LoginPath = "/account/login"; // Specify your login path
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
        });

        // Links
        builder.Services.AddSingleton<ILinksManager, LinksManager>();
        builder.Services.AddSingleton<ILinksService, LinksService>();

        builder.Services.AddSingleton<IRequestHandler<GetLinkByIdQuery, Link?>, GetsLinkByIdQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinkByUrlQuery, Link?>, GetLinkByUrlQueryHandler>();

        builder.Services.AddSingleton<IRequestHandler<GetLinksByIdsQuery, IEnumerable<LinkItem>>, GetLinksByIdsQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksQuery, PagedResults<LinkItem>>, GetLinksQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksByDomainQuery, PagedResults<LinkItem>>, GetLinksByDomainQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksByTagsQuery, PagedResults<LinkItem>>, GetsLinksByTagsQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinkRelatedLinksQuery, LinkItem[]>, GetLinkRelatedLinksQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetLinksRelatedTagsQuery, LinkTag[]>, GetLinksRelatedTagsQueryHandler>();

        builder.Services.AddSingleton<IRequestHandler<AddLinkCommand, Guid>, AddLinkCommandHandler>();
        builder.Services.AddSingleton<IRequestHandler<SubmitLinkCommand, Guid>, SubmitLinkCommandHandler>();

        // User Links
        builder.Services.AddSingleton<IUserLinksManager, UserLinksManager>();
        builder.Services.AddSingleton<IUserLinksService, UserLinksService>();

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

        builder.Services.AddSingleton<AuthApiEndpoints>();
        builder.Services.AddSingleton<LinksApiEndpoints>();
        builder.Services.AddSingleton<UserLinksApiEndpoints>();

        var app = builder.Build();

        var linksApiEndpoints = app.Services.GetRequiredService<LinksApiEndpoints>();
        linksApiEndpoints.MapEndpoints(app);

        var userLinksApiEndpoints = app.Services.GetRequiredService<UserLinksApiEndpoints>();
        userLinksApiEndpoints.MapEndpoints(app);

        // *** Disabling for now to get Next.js working *** 
        //app.UseHttpsRedirection();

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

        // NOTE: Order is important.
        app.UseMiddleware<ApiKeyMiddleware>();

        app.UseAuthorization();



        app.Run();
    }
}


