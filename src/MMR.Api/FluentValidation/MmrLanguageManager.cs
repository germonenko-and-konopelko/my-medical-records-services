using System.Globalization;
using FluentValidation.Resources;

namespace MMR.Api.FluentValidation;

public class MmrLanguageManager : LanguageManager
{
    private static readonly CultureInfo[] Cultures = [
        new("en"),
        new("ru"),
    ];

    public MmrLanguageManager()
    {
        AddLocalizedTranslations("NotEmptyValidator", "NotEmpty");
        AddLocalizedTranslations("EmailValidator", "Email");
        AddLocalizedTranslations("MaximumLengthValidator", "MaximumLength");
        AddLocalizedTranslations("MinimumLengthValidator", "MinimumLength");
    }

    private void AddLocalizedTranslations(string errorCode, string resourceKey)
    {
        foreach (CultureInfo? culture in Cultures)
        {
            var message = Resources.FluentValidation.ResourceManager.GetString(resourceKey, culture);
            AddTranslation(culture.Name, errorCode, message);
        }
    }
}