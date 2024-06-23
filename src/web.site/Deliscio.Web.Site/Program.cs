using System.Reflection;
using AspNetCore.Identity.MongoDbCore.Models;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Authentication.Common;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Data.Entities;
using Deliscio.Web.Site.Startups;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using RedisCaching;

namespace Deliscio.Web.Site;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var config = ConfigSettingsManager.GetConfigs();
        builder.Services.AddSingleton(config);

        var mongoConfig = config.GetSection(MongoDbOptions.SectionName);
        var mongoConfigConnectionString = config.GetSection($"{MongoDbOptions.SectionName}:ConnectionString").Value;
        var mongoConfigDatabaseName = config.GetSection($"{MongoDbOptions.SectionName}:DatabaseName").Value;

        // Add service defaults & Aspire components.
        builder.AddServiceDefaults();
        builder.AddRedisOutputCache("cache");
        builder.AddKeyedRedisDistributedCache("cache");

        // Add services to the container.
        // Note: Need to add 'Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
        //       and then add .AddRazorRuntimeCompilation();
        builder.Services.AddControllersWithViews()
            .AddRazorRuntimeCompilation();

        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.AppendTrailingSlash = false;
            options.LowercaseQueryStrings = true;
        });

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();



        builder.Services.AddSingleton<IRedisCaching, RedisCaching.RedisCaching>();


        //builder.Services.Configure<WebApiSettings>(
        //    builder.Configuration.GetSection(WebApiSettings.SectionName));

        //builder.Services.AddHttpClient<WebApiClient>();

        builder.Services.AddOptions<MongoDbOptions>()
            .BindConfiguration(MongoDbOptions.SectionName);





        builder.Services.AddOptions<MongoDbAuthOptions>()
            .BindConfiguration(MongoDbAuthOptions.SectionName);

        #region - Authentication / Authorization -
        builder.Services.AddIdentity<AuthUser, AuthRole>()
            .AddMongoDbStores<AuthUser, AuthRole, ObjectId>(mongoConfigConnectionString, mongoConfigDatabaseName)
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredUniqueChars = 1;

            //options.AuthUser.AllowedUserNameCharacters 
            options.User.RequireUniqueEmail = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

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
        #endregion



        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Links
        builder.ConfigureLinksDependencies();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days.
            // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
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
