using Ardalis.GuardClauses;

using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;


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
        Guard.Against.Null(mediator);
        Guard.Against.Null(bus);
        Guard.Against.Null(logger);
    }
}