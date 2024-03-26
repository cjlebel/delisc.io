using System.Reflection;
using AspNetCore.Identity.MongoDbCore.Models;
using Deliscio.Apis.WebApi.Common.APIs;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Apis.WebApi.Managers;
using Deliscio.Apis.WebApi.Startups;
using Deliscio.Common.Middleware;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Authentication;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.MediatR.Commands;
using Deliscio.Modules.Authentication.MediatR.Commands.Handlers;
using Deliscio.Modules.QueuedLinks;
using Deliscio.Modules.QueuedLinks.Common.Models;
using Deliscio.Modules.QueuedLinks.Harvester;
using Deliscio.Modules.QueuedLinks.Interfaces;
using Deliscio.Modules.QueuedLinks.MediatR.Commands;
using Deliscio.Modules.QueuedLinks.MediatR.Commands.Handlers;
using Deliscio.Modules.QueuedLinks.Tagger;
using Deliscio.Modules.QueuedLinks.Verifier;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
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

        builder.Services.AddSingleton<HttpClient>();

        var config = ConfigSettingsManager.GetConfigs();
        var apiKey = config.GetValue<string>("ApiKey");
        var mongoConfig = config.GetSection(MongoDbOptions.SectionName);
        var mongoConfigConnectionString = config.GetSection($"{MongoDbOptions.SectionName}:ConnectionString").Value;
        var mongoConfigDatabaseName = config.GetSection($"{MongoDbOptions.SectionName}:DatabaseName").Value;

        //BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V2;
        //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        builder.Services.Configure<MongoDbOptions>(mongoConfig);
        builder.Services.Configure<QueuedLinksSettingsOptions>(config.GetSection(QueuedLinksSettingsOptions.SectionName));

        builder.Services.AddCors();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(API_VERSION, new OpenApiInfo { Title = "Delisc.io", Version = API_VERSION });
            c.EnableAnnotations();

            c.AddSecurityDefinition("SwaggerAuthApiKey", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Name = "deliscio-api-key",
                Description = apiKey,

            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "SwaggerAuthApiKey"
                        }
                    },
                    new string[] { }
                }
            });
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        // Tried to get an extension method to work, but no luck so far.
        // Moving on so that I can be productive elsewhere
        //builder.Services.AddMongoDbSingleton(config);

        //builder.Services.AddScoped<MongoDbClient, MongoDbClient>(sp =>
        //{
        //    var mongoDbOptions = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
        //    var client = new MongoDbClient(mongoDbOptions);

        //    return client;
        //});

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddMassTransit(x =>
        {
            QueuedLinksSettingsOptions options;

            x.UsingRabbitMq((context, cfg) =>
            {
                options = context.GetRequiredService<IOptions<QueuedLinksSettingsOptions>>().Value;

                cfg.Host(new Uri(options.Host), hostConfig =>
                {
                    hostConfig.Username(options.Username);
                    hostConfig.Password(options.Password);
                });

                cfg.ConfigureEndpoints(context);
                // More options ...
            });
        });

        builder.Services.AddSingleton(config);

        #region - Authentication / Authorization -
        //builder.Services.AddIdentity<AuthUser, MongoIdentityRole>()
        //    // Wasn't able to pass the client in, had to do it this way for now
        //    // sp => sp.GetRequiredService<MongoDbClient>().Database
        //    .AddMongoDbStores<AuthUser, MongoIdentityRole, Guid>(mongoConfigConnectionString, mongoConfigDatabaseName)
        //    .AddDefaultTokenProviders();

        //builder.Services.Configure<IdentityOptions>(options =>
        //{
        //    options.Password.RequiredLength = 6;
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireLowercase = true;
        //    options.Password.RequireNonAlphanumeric = true;
        //    options.Password.RequireUppercase = true;
        //    options.Password.RequiredUniqueChars = 1;

        //    //options.AuthUser.AllowedUserNameCharacters 
        //    options.User.RequireUniqueEmail = true;

        //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        //    options.Lockout.MaxFailedAccessAttempts = 5;
        //    options.Lockout.AllowedForNewUsers = true;
        //});

        //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //    .AddCookie(options =>
        //    {
        //        options.Cookie.Name = "DeliscioTastyCookie";
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //        options.Cookie.HttpOnly = true;
        //        options.ExpireTimeSpan = TimeSpan.FromDays(30); // Adjust the expiration time as needed
        //        options.SlidingExpiration = true;
        //        options.LoginPath = "/account/login"; // Specify your login path
        //    });

        //builder.Services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
        //});
        #endregion

        // Authentication 
        //builder.Services.AddScoped<IUsersManager, UsersManager>();
        //builder.Services.AddScoped<IAuthService, AuthService>();


        //builder.Services.AddScoped<IRequestHandler<RegisterCommand, (bool IsSuccess, string Message, string[] ErrorMessages)>, RegisterCommandHandler>();
        //builder.Services.AddScoped<IRequestHandler<SignInCommand, (bool IsSuccess, string Message, AuthUser? User)>, SignInCommandHandler>();

        // Links
        builder.ConfigureLinksDependencies();

        // AuthUser Links
        builder.ConfigureUserLinksDependencies();

        // Queue
        builder.Services.AddSingleton<IQueuedLinksService, QueuedLinksService>();

        // Adds a new link to the queue - currently masstransit/rabbitmq aren't co-operating
        builder.Services.AddSingleton<IRequestHandler<AddNewLinkQueueCommand, bool>, AddNewLinkQueueCommandHandler>();


        // This is weird, I should not need to add these here?!?!?
        builder.Services.AddSingleton<IVerifyProcessor, VerifyProcessor>();
        builder.Services.AddSingleton<IHarvesterProcessor, HarvesterProcessor>();
        builder.Services.AddSingleton<ITaggerProcessor, TaggerProcessor>();
        // This is weird, I should not need to add these here?!?!?

        //builder.Services.AddScoped<AuthApiEndpoints>();
        builder.Services.AddSingleton<LinksApiEndpoints>();
        builder.Services.AddSingleton<UserLinksApiEndpoints>();

        var app = builder.Build();

        //// Unfortunately, have to place these in here due to 'scoped' issues
        //app.MapPost("/v1/auth/register", [AllowAnonymous] async (
        //    [FromBody] RegisterRequest register,
        //    HttpRequest req,
        //    HttpResponse res,
        //    IUsersManager manager
        //    ) =>
        //{
        //    await manager.RegisterAsync(register);
        //}).AllowAnonymous();

        //app.MapPost("/v1/auth/signin",
        //    [AllowAnonymous] async (
        //        [FromBody] SignInRequest signIn,
        //        HttpRequest req,
        //        HttpResponse res,
        //        IUsersManager manager) =>
        //{
        //    await manager.SignInAsync(signIn);
        //}).AllowAnonymous();

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
        else
        {
            // Temp - allow dev to bypass middleware, until I can get Swagger to authenticate as a client
            app.UseMiddleware<ApiKeyMiddleware>();
        }

        // NOTE: Order is important.
        ////app.UseAuthentication();
        ////app.UseAuthorization();
        /// 
        //TODO: Not able to get swagger to authenticate
        //app.UseMiddleware<ApiKeyMiddleware>();

        app.Run();
    }
}


