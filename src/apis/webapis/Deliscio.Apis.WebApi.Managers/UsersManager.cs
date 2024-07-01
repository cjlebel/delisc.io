using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Apis.WebApi.Common.Responses;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Data.Entities;
using Deliscio.Modules.Authentication.MediatR.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Structurizr.Annotations;
namespace Deliscio.Apis.WebApi.Managers;

[CodeElement("UsersManager", Description = "")]
public class UsersManager : ManagerBase<UsersManager>, IUsersManager
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersManager> _logger;

    public UsersManager(IMediator mediator, ILogger<UsersManager> logger) : base(logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<FluentResults.Result<AuthUser?>> RegisterAsync(RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.UserName);

        return await _mediator.Send(command);
    }

    public async Task<SignInResponse> SignInAsync(SignInRequest request)
    {
        throw new NotImplementedException();
        //var signinResponse = new SignInResponse();

        //var signinCommand = new LoginCommand(request.EmailOrUserName, request.Password);

        //var signinResult = await _mediator.Send(signinCommand);

        //if (!signinResult.IsSuccess || signinResult.Value == null)
        //{
        //    signinResponse.IsSuccess = false;
        //    //signinResponse.Message = signinResult.Errors;

        //    return signinResponse;
        //}

        //var user = signinResult.Value;

        //return new SignInResponse
        //{
        //    IsSuccess = true,
        //    Message = string.Empty,
        //    User = new SignInResponse.UserInfo(user.Id.ToString(),
        //        user.UserName ?? string.Empty, user.Email ?? string.Empty,
        //        new[] { "User" })
        //};
    }

    public async Task SignOutAsync()
    {
        //await _signInManager.UserSignOutAsync();
    }
}