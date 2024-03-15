using System.Reflection;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Web.Mvc.Startups;

namespace Deliscio.Web.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Add service defaults & Aspire components.
        builder.AddServiceDefaults();
        builder.AddRedisOutputCache("cache-web");

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var config = ConfigSettingsManager.GetConfigs();

        //builder.Services.Configure<WebApiSettings>(
        //    builder.Configuration.GetSection(WebApiSettings.SectionName));

        //builder.Services.AddHttpClient<WebApiClient>();

        builder.Services.AddOptions<MongoDbOptions>()
            .BindConfiguration(MongoDbOptions.SectionName);

        // Add services to the container.
        // Note: Need to add 'Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
        //       and then add .AddRazorRuntimeCompilation();
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddSingleton(config);

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
