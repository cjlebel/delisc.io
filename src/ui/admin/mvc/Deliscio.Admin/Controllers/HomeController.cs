using System.Diagnostics;
using Deliscio.Admin.Models;
using Deliscio.Modules.Authentication.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Admin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator, ILogger<HomeController> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    [Route("setup")]
    public IActionResult Setup()
    {
        return View();
    }

    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        var m = new LoginModel.InputModel()
        {
            RememberMe = true
        };// { ReturnUrl = returnUrl };

        return View(m);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login([FromForm] LoginModel.InputModel model)
    {
        if (!ModelState.IsValid)
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

        var command = new LoginCommand(model.Email, model.Password);
        var rslt = await _mediator.Send(command);

        if (rslt.IsFailed)
        {
            ViewBag.Errors = rslt.Errors.Select(e => e.Message).ToList();

            model.Password = string.Empty;

            return View(model);
        }

        var signIn = rslt.Value;

        //await HttpContext.SignInAsync(signIn);

        if (signIn.Succeeded)
        {
            _logger.LogInformation("User logged in.");
            return LocalRedirect("~/");
        }
        //if (signIn.RequiresTwoFactor)
        //{
        //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl =  model.ReturnUrl, RememberMe = Input.RememberMe });
        //}
        if (signIn.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    public IActionResult Logout()
    {
        return RedirectToAction("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
