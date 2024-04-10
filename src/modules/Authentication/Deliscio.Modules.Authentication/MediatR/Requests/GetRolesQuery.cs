using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Requests;

public sealed record GetRolesQuery : IRequest<FluentResults.Result<Role[]>>
{
    public GetRolesQuery() { }
}

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, FluentResults.Result<Role[]>>
{
    private readonly IAuthService _authService;

    public GetRolesQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }


    public async Task<Result<Role[]>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _authService.RolesGetAsync();
    }
}