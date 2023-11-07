namespace Deliscio.Web.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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
