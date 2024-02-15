using Deliscio.Apis.WebApi.Common.Clients;
using Deliscio.Common.Settings;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Commands.Handlers;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Queries.Handlers;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Web.Admin.Components;
using MediatR;
using System.Reflection;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Configuration;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var config = ConfigSettingsManager.GetConfigs();

builder.Services.Configure<WebApiSettings>(
    builder.Configuration.GetSection(WebApiSettings.SectionName));

builder.Services.AddHttpClient<AdminApiClient>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton(config);

builder.Services.AddOptions<MongoDbOptions>()
    .BindConfiguration(MongoDbOptions.SectionName);

//builder.Services.AddSingleton<ILinksManager, LinksManager>();
builder.Services.AddSingleton<ILinksService, LinksService>();
builder.Services.AddSingleton<ILinksRepository, LinksRepository>();

builder.Services.AddSingleton<IRequestHandler<FindLinksQuery, PagedResults<LinkItem>>, FindLinksQueryHandler>();

builder.Services.AddSingleton<IRequestHandler<GetLinkByIdQuery, Link?>, GetLinkByIdQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetLinkByUrlQuery, Link?>, GetLinkByUrlQueryHandler>();

builder.Services.AddSingleton<IRequestHandler<GetLinksByIdsQuery, IEnumerable<LinkItem>>, GetLinksByIdsQueryHandler>();

//Obsolete - GetLinksQuery
builder.Services.AddSingleton<IRequestHandler<GetLinksQuery, PagedResults<LinkItem>>, GetLinksQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetLinksByDomainQuery, PagedResults<LinkItem>>, GetLinksByDomainQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetLinksByTagsQuery, PagedResults<LinkItem>>, GetLinksByTagsQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetLinkRelatedLinksQuery, LinkItem[]>, GetLinkRelatedLinksQueryHandler>();
builder.Services.AddSingleton<IRequestHandler<GetRelatedTagsByTagsQuery, LinkTag[]>, GetRelatedTagsByTagsQueryHandler>();

builder.Services.AddSingleton<IRequestHandler<AddLinkCommand, Guid>, AddLinkCommandHandler>();
builder.Services.AddSingleton<IRequestHandler<SubmitLinkCommand, Guid>, SubmitLinkCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();
//.AddAdditionalAssemblies(typeof(Home).Assembly);

app.Run();
