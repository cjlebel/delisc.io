using Deliscio.Core.Models;
using Deliscio.Modules.Authentication.Common;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Data.Entities;
using Deliscio.Modules.Authentication.MediatR.Commands;
using Deliscio.Modules.Authentication.MediatR.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace Deliscio.Modules.Authentication;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection RegisterAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbAuthOptions = new MongoDbAuthOptions();
       configuration.Bind(MongoDbAuthOptions.SectionName, mongoDbAuthOptions);

        services.AddOptions<MongoDbAuthOptions>()
            .BindConfiguration(MongoDbAuthOptions.SectionName)
            .ValidateDataAnnotations();


        services.AddIdentity<AuthUser, AuthRole>()
            .AddMongoDbStores<AuthUser, AuthRole, ObjectId>(mongoDbAuthOptions.ConnectionString, mongoDbAuthOptions.DatabaseName)
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(o =>
        {
            o.Password.RequiredLength = 6;
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequireUppercase = true;
            o.Password.RequiredUniqueChars = 1;

            //options.AuthUser.AllowedUserNameCharacters 
            o.User.RequireUniqueEmail = true;

            o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            o.Lockout.MaxFailedAccessAttempts = 5;
            o.Lockout.AllowedForNewUsers = true;
        });

        var loginPath = !string.IsNullOrEmpty(mongoDbAuthOptions.LoginPath) ? mongoDbAuthOptions.LoginPath : "/account/login";

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
            {
                o.Cookie.Name = "DeliscioTastyCookie";
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.HttpOnly = true;
                o.ExpireTimeSpan = TimeSpan.FromDays(30); // Adjust the expiration time as needed
                o.SlidingExpiration = true;
                o.LoginPath = loginPath; // Specify your login path
            });

        //services.AddAuthorization(o =>
        //{
        //    o.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
        //});

        services.AddScoped<UserManager<AuthUser>>();
        services.AddScoped<RoleManager<AuthRole>>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRequestHandler<CreateUserCommand, FluentResults.Result<User?>>, CreateCommandHandler>();
        services.AddScoped<IRequestHandler<LoginCommand, FluentResults.Result<SignInResult>>, LoginCommandHandler>();
        services.AddScoped<IRequestHandler<RegisterUserCommand, FluentResults.Result<AuthUser?>>, RegisterCommandHandler>();

        services.AddScoped<IRequestHandler<GetUsersQuery, FluentResults.Result<PagedResults<User>>>, GetUsersQueryHandler>();


        services.AddScoped<IRequestHandler<GetRolesQuery, FluentResults.Result<Role[]>>, GetRolesQueryHandler>();

        return services;
    }
}