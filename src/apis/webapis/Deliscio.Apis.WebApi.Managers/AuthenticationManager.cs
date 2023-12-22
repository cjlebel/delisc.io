using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Modules.Authentication.MediatR.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Managers;

//public class AuthenticationManager : ManagerBase<AuthenticationManager>, IAuthManager
//{
//    //private readonly IMediator _mediator;
//    //private readonly ILogger<AuthenticationManager> _logger;

//    //public AuthenticationManager(IMediator mediator, ILogger<AuthenticationManager> logger) : base(logger)
//    //{
//    //    _mediator = mediator;
//    //    _logger = logger;
//    //}

//    //public async Task<(bool IsSuccess, string Message)> RegisterAsync(RegisterRequest request)
//    //{
//    //    var command = new RegisterCommand(request.Email, request.Password, request.UserName);

//    //    return await _mediator.Send(command);
//    //}

//    //public async Task<(bool IsSuccess, string Message)> SignInAsync(SignInRequest request)
//    //{
//    //    var command = new SignInCommand(request.EmailOrUserName, request.Password);

//    //    return await _mediator.Send(command);
//    //}

//    //public async Task SignOutAsync()
//    //{
//    //    //await _signInManager.SignOutAsync();
//    //}
//}