using System.Reflection;
using AspNetCore.Identity.MongoDbCore.Models;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Authentication;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.MediatR.Commands;
using Deliscio.Modules.Authentication.MediatR.Commands.Handlers;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Web.Mvc.Startups;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Deliscio.Web.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var config = ConfigSettingsManager.GetConfigs();
        var apiKey = config.GetValue<string>("ApiKey");
        var mongoConfig = config.GetSection(MongoDbOptions.SectionName);
        var mongoConfigConnectionString = config.GetSection($"{MongoDbOptions.SectionName}:ConnectionString").Value;
        var mongoConfigDatabaseName = config.GetSection($"{MongoDbOptions.SectionName}:DatabaseName").Value;

        //BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        builder.Services.Configure<MongoDbOptions>(mongoConfig);

        //// Set up the React files
        //if (env == Environments.Development)
        //{
        //    builder.Services.AddSpaStaticFiles(configuration =>
        //    {
        //        configuration.RootPath = "client";
        //    });
        //}
        //else
        //{
        //    // Use production settings
        //    builder.Services.AddSpaStaticFiles(configuration =>
        //    {
        //        configuration.RootPath = "wwwroot";
        //    });
        //}

        // Add services to the container.
        // Note: Need to add 'Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
        //       and then add .AddRazorRuntimeCompilation();
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

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
        //builder.ConfigureUserLinksDependencies();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            //app.UseSpa(spa =>
            //{
            //    spa.UseReactDevelopmentServer(npmScript: "start");
            //});

            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            // Use production settings
            //app.UseSpaStaticFiles();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
