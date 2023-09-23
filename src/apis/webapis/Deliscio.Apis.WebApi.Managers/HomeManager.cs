using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.QueuedLinks.Common.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Structurizr.Annotations;


namespace Deliscio.Apis.WebApi.Managers;

/// <summary>
/// HomeManager is responsible for gathering all of the data needed for the home page,
/// and returning it to the api in a form that is ready to be consumed by the UI.
/// </summary>
/// <seealso cref="IHomeManager" />
public class HomeManager : ManagerBase<HomeManager>, IHomeManager
{
    public HomeManager(IMediator mediator, IBusControl bus, ILogger<HomeManager> logger) : base(bus, logger)
    {
    }
}