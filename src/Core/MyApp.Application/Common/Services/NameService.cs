using System.Text.RegularExpressions;

namespace MyApp.Application.Common.Services;

public static class NameService
{
    public static string CharacterRegulatory(string name)
    {
        name = name.Replace("\"", "")
            .Replace("!", "")
            .Replace("'", "")
            .Replace("^", "")
            .Replace("+", "")
            .Replace("%", "")
            .Replace("&", "")
            .Replace("/", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("=", "")
            .Replace("?", "")
            .Replace("_", "-")
            .Replace("@", "")
            .Replace("€", "")
            .Replace("¨", "")
            .Replace("~", "")
            .Replace(",", "")
            .Replace(";", "")
            .Replace(":", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("|", "");

        name = name.ToLower();
        name = name.Replace("ı", "i");
        name = name.Replace("ö", "o");
        name = name.Replace("ü", "u");
        name = name.Replace("ş", "s");
        name = name.Replace("ğ", "g");
        name = name.Replace("ç", "c");
        name = name.Replace(" ", "-");

        return name;
    }
}
