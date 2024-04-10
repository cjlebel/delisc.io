using System.Reflection;
using Deliscio.Apis.WebApi.Common.Clients;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Common.Settings;
using Deliscio.Core.Configuration;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Models;
using Deliscio.Modules.Authentication;
using Deliscio.Modules.Authentication.Common;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Data.Entities;
using Deliscio.Modules.Authentication.MediatR.Commands;
using Deliscio.Modules.Authentication.MediatR.Requests;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.Links.MediatR.Queries.Handlers;
using Deliscio.Modules.UserProfiles;
using Deliscio.Modules.UserProfiles.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

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

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add services to the container.
// Note: Need to add 'Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
//       and then add .AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//builder.Services.Configure<WebApiSettings>(
//    builder.Configuration.GetSection(WebApiSettings.SectionName));

builder.Services.AddHttpClient<AdminApiClient>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton(config);

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

builder.Services.AddScoped<UserManager<AuthUser>>();
builder.Services.AddScoped<RoleManager<AuthRole>>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRequestHandler<CreateUserCommand, FluentResults.Result<User?>>, CreateCommandHandler>();
builder.Services.AddScoped<IRequestHandler<LoginCommand, FluentResults.Result<SignInResult>>, LoginCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RegisterUserCommand, FluentResults.Result<AuthUser?>>, RegisterCommandHandler>();

builder.Services.AddScoped<IRequestHandler<GetUsersQuery, FluentResults.Result<PagedResults<User>>>, GetUsersQueryHandler>();


builder.Services.AddScoped<IRequestHandler<GetRolesQuery, FluentResults.Result<Role[]>>, GetRolesQueryHandler>();


builder.Services.AddScoped<IUserProfilesService, UserProfilesService>();


#endregion

//builder.Services.AddSingleton<ILinksManager, LinksManager>();
builder.Services.AddSingleton<ILinksService, LinksService>();
builder.Services.AddSingleton<ILinksAdminService, LinksAdminService>();

builder.Services.AddSingleton<ILinksRepository, LinksRepository>();

builder.Services.AddSingleton<IRequestHandler<FindLinksAdminQuery, PagedResults<LinkItem>>, FindLinksAdminQueryHandler>();

builder.Services.AddSingleton<IRequestHandler<GetLinkByIdQuery, Link?>, GetLinkByIdQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetLinkByUrlQuery, Link?>, GetLinkByUrlQueryHandler>();

builder.Services.AddSingleton<IRequestHandler<GetLinksByIdsQuery, IEnumerable<LinkItem>>, GetLinksByIdsQueryHandler>();

builder.Services.AddSingleton<IRequestHandler<GetLinksByDomainQuery, PagedResults<LinkItem>>, GetLinksByDomainQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetLinksByTagsQuery, PagedResults<LinkItem>>, GetLinksByTagsQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetLinkRelatedLinksQuery, LinkItem[]>, GetLinkRelatedLinksQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetRelatedTagsByTagsQuery, LinkTag[]>, GetRelatedTagsByTagsQueryHandler>();

builder.Services.AddSingleton<IRequestHandler<AddLinkCommand, Guid>, AddLinkCommandHandler>();
builder.Services.AddSingleton<IRequestHandler<SubmitLinkByUserCommand, Guid>, SubmitLinkByUserCommandHandler>();
builder.Services.AddSingleton<IRequestHandler<DeleteLinkCommand, bool>, DeleteLinkCommandHandler>();
builder.Services.AddSingleton<IRequestHandler<EditLinkCommand, (bool IsSuccess, string Message)>, EditLinkCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
