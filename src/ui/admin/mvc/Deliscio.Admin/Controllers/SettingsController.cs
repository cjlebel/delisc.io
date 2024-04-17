using System.ComponentModel;
using System.Reflection;
using Deliscio.Admin.Models.Settings;
using Deliscio.Common.Interfaces;
using Deliscio.Common.Settings;
using Deliscio.Modules.Settings.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Admin.Controllers;
public class SettingsController : Controller
{
    private readonly ILogger<SettingsController> _logger;
    private readonly IMediator _mediator;

    private readonly SettingsClassInfo[] _availableSettings;

    public SettingsController(IMediator mediator, ILogger<SettingsController> logger)
    {
        _mediator = mediator;
        _logger = logger;

        _availableSettings = Discover();

        //_availableSettings = Discover()?.ToList() ?? [];

        //_availableSettings = new List<string>
        //{
        //    "WebSiteSettings",
        //    "UserSettings",
        //    "EmailSettings",
        //    "SecuritySettings",
        //    "SocialSettings",
        //    "NotificationSettings",
        //    "PaymentSettings",
        //    "ShippingSettings",
        //    "TaxSettings",
        //    "CurrencySettings",
        //    "LanguageSettings",
        //    "ThemeSettings",
        //    "PluginSettings",
        //    "ModuleSettings",
        //    "FeatureSettings",
        //    "IntegrationSettings",
        //    "MiscellaneousSettings"
        //};
    }

    public IActionResult Index()
    {
        return View(_availableSettings);
    }

    public IActionResult Details(string name)
    {
        var settings = _availableSettings.FirstOrDefault(s => s.Name == name);

        return View(settings);
    }

    private SettingsClassInfo[] Discover()
    {
        //List<AvailableSettingModel> availableSettings = new();

        var settingsTypes = SettingsHelper.Discover(typeof(WebsiteSettingsV1));

        return settingsTypes.OrderBy(s=>s.Order).ThenBy(s=>s.Name).ToArray();

        //foreach(var type in settingsTypes)
        //{
        //    var dislayAttribute = type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? type.Name;

        //    var displayName = type.Name;
        //    var sort = 999;

        //    if (dislayAttribute != null)
        //    {
        //        var x = dislayAttribute.Split(" | ");
        //        if (x.Length > 1)
        //        {
        //            if (int.TryParse(x[0], out var s))
        //            {
        //                sort = s;
        //            }

        //            displayName = x[1];
        //        }
        //    }

        //    var description = type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;
        //    var category = type.GetCustomAttribute<CategoryAttribute>()?.Category ?? string.Empty;
        //    var versionIndex = type.Name.LastIndexOf("V", StringComparison.InvariantCulture);//  (type.GetMethod("get_Version")?.Invoke(null, null) as string) ?? string.Empty;
        //    var version = "1";

        //    if(versionIndex > 0)
        //    {
        //        versionIndex++;
        //        version = type.Name.Substring(versionIndex, type.Name.Length - versionIndex);
        //    }

        //    var availableSetting = new AvailableSettingModel
        //    {
        //        Category = !string.IsNullOrWhiteSpace(category) ? category.Split('/')?.ToArray() ?? new []{ "Misc" } : new[] { "Misc" },
        //        Description = description,
        //        Name = displayName,
        //        Order = sort,
        //        Type = type.Name,
        //        Version = version
        //    };

        //    availableSettings.Add(availableSetting);
        //}

        //return availableSettings.OrderBy(s => s.Order).ThenBy(s => s.Name).ToArray();
    }



    private void WithSettings()
    {
        ViewBag.AvailableSettings = _availableSettings;
    }
}