using Deliscio.Core.Models;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using MediatR;

namespace Deliscio.Modules.Authentication.MediatR.Requests;

public sealed record GetUsersQuery : IRequest<FluentResults.Result<PagedResults<User>>>
{
    public int PageNo { get; }

    public int PageSize { get; }

    public GetUsersQuery(int pageNo = 1, int pageSize = 50)
    {
        PageNo = pageNo > 0 ? pageNo : throw new ArgumentException(nameof(pageNo));
        PageSize = pageSize > 0 ? pageSize : throw new ArgumentException(nameof(pageSize));
    }
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, FluentResults.Result<PagedResults<User>>>
{
    private readonly IAuthService _authService;

    public GetUsersQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }


    public async Task<FluentResults.Result<PagedResults<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // Due to MediatR not having a synchronous version of the method, we have to use Task.Run to run the method synchronously
        var rslt = await Task.Run(() => _authService.UsersGet(request.PageNo, request.PageSize), cancellationToken);

        return rslt;
    }
}