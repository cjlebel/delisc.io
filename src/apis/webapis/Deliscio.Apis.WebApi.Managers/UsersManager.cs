using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Apis.WebApi.Common.Responses;
using Deliscio.Modules.Authentication.Common.Models;
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

    public async Task<(bool IsSuccess, string Message, string[] ErrorMessages)> RegisterAsync(RegisterRequest request)
    {
        var command = new RegisterCommand(request.Email, request.Password, request.UserName);

        return await _mediator.Send(command);
    }

    public async Task<SignInResponse> SignInAsync(SignInRequest request)
    {
        var signinResponse = new SignInResponse();

        var signinCommand = new SignInCommand(request.EmailOrUserName, request.Password);

        var signinResult = await _mediator.Send(signinCommand);

        if (signinResult.IsSuccess || signinResult.User == null)
        {
            signinResponse.IsSuccess = false;
            signinResponse.Message = signinResult.Message;
            return signinResponse;
        }

        return new SignInResponse
        {
            IsSuccess = true,
            Message = signinResult.Message,
            User = new SignInResponse.UserInfo(signinResult.User.Id.ToString(),
                signinResult.User.UserName ?? string.Empty, signinResult.User.Email ?? string.Empty,
                new[] { "User" })
        };
    }

    public async Task SignOutAsync()
    {
        //await _signInManager.SignOutAsync();
    }
}