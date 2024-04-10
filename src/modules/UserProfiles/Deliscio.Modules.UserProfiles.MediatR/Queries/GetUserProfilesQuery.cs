using System;
using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.UserProfiles.Common.Interfaces;
using Deliscio.Modules.UserProfiles.Common.Models;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.UserProfiles.MediatR.Queries;

public class GetUserProfilesQuery : IRequest<Result<PagedResults<UserProfileItem>>>
{
    public bool? IsOnline { get; init; }

    public string DisplayName { get; init; }

    public string Email { get; set; }

    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public GetUserProfilesQuery(int pageNo = 1, int pageSize = 50, string displayName = "", bool? isOnline = null)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        IsOnline = isOnline;

        PageNo = pageNo;
        PageSize = pageSize;
    }
}

public class GetUserProfileItemsQueryHandler : IRequestHandler<GetUserProfilesQuery, Result<PagedResults<UserProfileItem>>>
{
    private readonly IUserProfilesService _service;

    public GetUserProfileItemsQueryHandler(IUserProfilesService service)
    {
        Guard.Against.Null(service);

        _service = service;
    }

    public async Task<Result<PagedResults<UserProfileItem>>> Handle(GetUserProfilesQuery command, CancellationToken token)
    {
        var results = await _service.SearchAsync(command.DisplayName, command.Email, command.PageNo, command.PageSize, token);

        return results;
    }
}