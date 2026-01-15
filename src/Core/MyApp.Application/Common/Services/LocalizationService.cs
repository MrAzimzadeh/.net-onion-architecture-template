using Microsoft.Extensions.Localization;
using MyApp.Application.Common.Interfaces;

namespace MyApp.Application.Common.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizerFactory factory)
    {
        _localizer = factory.Create("CommonResources", "MyApp.Application");
    }

    public string this[string key] => _localizer[key];

    public string GetLocalizedString(string key) => _localizer[key];
}
