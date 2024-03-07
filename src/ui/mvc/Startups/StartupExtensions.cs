using Deliscio.Core.Models;
using Deliscio.Modules.Links;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Mongo;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.Links.MediatR.Queries.Handlers;
using Deliscio.Web.Mvc.Managers;
using MediatR;

namespace Deliscio.Web.Mvc.Startups;

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
        builder.Services.AddSingleton<IHomePageManager, HomePagePageManager>();
        builder.Services.AddSingleton<ILinksPageManager, LinksPageManager>();

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
        builder.Services.AddSingleton<IRequestHandler<SubmitLinkByUserCommand, Guid>, SubmitLinkByUserCommandHandler>();

    }

    ///// <summary>
    ///// Adds all of the dependencies that are needed for the AuthUser Links
    ///// </summary>
    ///// <param name="builder"></param>
    //public static void ConfigureUserLinksDependencies(this WebApplicationBuilder builder)
    //{
    //    //builder.Services.AddSingleton<IUserLinksManager, UserLinksManager>();
    //    //builder.Services.AddSingleton<IUserLinksService, UserLinksService>();

    //    builder.Services.AddSingleton<IRequestHandler<GetUserLinkByIdQuery, UserLink?>, GetUserLinkByIdQueryHandler>();
    //    builder.Services.AddSingleton<IRequestHandler<GetUserLinksQuery, PagedResults<UserLink>>, GetUserLinksQueryHandler>();
    //    builder.Services.AddSingleton<IRequestHandler<AddLinkToUserCommand, Guid>, AddLinkToUserCommandHandler>();

    //}
}