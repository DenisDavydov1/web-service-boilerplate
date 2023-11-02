using System.Reflection;
using BoilerPlate.Core.Attributes;
using BoilerPlate.Core.Constants;

namespace BoilerPlate.Core.Extensions;

public static class ErrorCodeLocalizationExtensions
{
    private const string DefaultLanguageCode = LanguageCodes.English;

    public static string GetText<TEnum>(this TEnum errorCode, string languageCode)
        where TEnum : struct, Enum
    {
        var fieldInfo = typeof(TEnum).GetField(errorCode.ToString());
        var attributes = (fieldInfo?.GetCustomAttributes(typeof(LocalizationAttribute))
            as IEnumerable<LocalizationAttribute>)?.ToArray();
        if (attributes?.Any() != true)
        {
            return errorCode.ToString();
        }

        var text = attributes.FirstOrDefault(x => x.Code == languageCode)?.Value;
        if (text != null)
        {
            return text;
        }

        var defaultText = attributes.FirstOrDefault(x => x.Code == DefaultLanguageCode)?.Value;
        if (defaultText != null)
        {
            return defaultText;
        }

        throw new Exception($"No localization was found for error code {errorCode}");
    }
}