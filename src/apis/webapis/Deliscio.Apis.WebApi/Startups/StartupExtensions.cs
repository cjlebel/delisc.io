using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Managers;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Commands.Handlers;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Queries.Handlers;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.Links;
using MediatR;
using Deliscio.Modules.UserLinks.Common.Interfaces;
using Deliscio.Modules.UserLinks.Common.Models;
using Deliscio.Modules.UserLinks.MediatR.Commands.Handlers;
using Deliscio.Modules.UserLinks.MediatR.Commands;
using Deliscio.Modules.UserLinks.MediatR.Queries.Handlers;
using Deliscio.Modules.UserLinks.MediatR.Queries;
using Deliscio.Modules.UserLinks;

namespace Deliscio.Apis.WebApi.Startups;

/// <summary>
/// Just a helper to help set up the Startup
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Adds all of the dependencies that are needed for the Links
    /// </summary>
    public static void ConfigureLinksDependencies(this WebApplicationBuilder builder)
    {
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

    }

    /// <summary>
    /// Adds all of the dependencies that are needed for the AuthUser Links
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureUserLinksDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IUserLinksManager, UserLinksManager>();
        builder.Services.AddSingleton<IUserLinksService, UserLinksService>();

        builder.Services.AddSingleton<IRequestHandler<GetUserLinkByIdQuery, UserLink?>, GetUserLinkByIdQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<GetUserLinksQuery, PagedResults<UserLink>>, GetUserLinksQueryHandler>();
        builder.Services.AddSingleton<IRequestHandler<AddLinkToUserCommand, Guid>, AddLinkToUserCommandHandler>();

    }
}