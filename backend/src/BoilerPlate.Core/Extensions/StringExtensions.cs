using CaseExtensions;

namespace BoilerPlate.Core.Extensions;

public static class StringExtensions
{
    public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, Enum
    {
        if (Enum.TryParse(value.ToPascalCase(), ignoreCase: true, out TEnum result))
        {
            return result;
        }

        throw new ArgumentException($"Unknown enum value: {value}", nameof(value));
    }
}