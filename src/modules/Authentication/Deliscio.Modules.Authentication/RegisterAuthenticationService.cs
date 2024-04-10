using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Authentication.Common;
using Deliscio.Modules.Authentication.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Deliscio.Modules.Authentication;

public class RegisterAuthenticationService
{
    private readonly string _connectionString;
    private readonly string _dbName;

    public RegisterAuthenticationService(IOptions<MongoDbAuthOptions> options)
    {
        var opts = options.Value ?? throw new ArgumentNullException(nameof(options));

        _connectionString = opts.ConnectionString;
        _dbName = opts.DatabaseName;
    }

    public void RegisterConfigureServices(IServiceCollection services)
    {
        services.AddOptions<MongoDbAuthOptions>()
            .BindConfiguration(MongoDbAuthOptions.SectionName);

        services.AddIdentity<AuthUser, AuthRole>()
            .AddMongoDbStores<AuthUser, AuthRole, ObjectId>(_connectionString, _dbName)
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
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

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "DeliscioTastyCookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30); // Adjust the expiration time as needed
                options.SlidingExpiration = true;
                options.LoginPath = "/account/login"; // Specify your login path
            });

        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
        //});
    }
}