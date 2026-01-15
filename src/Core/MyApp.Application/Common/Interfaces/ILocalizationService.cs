namespace MyApp.Application.Common.Interfaces;

public interface ILocalizationService
{
    string this[string key] { get; }
    string GetLocalizedString(string key);
}
