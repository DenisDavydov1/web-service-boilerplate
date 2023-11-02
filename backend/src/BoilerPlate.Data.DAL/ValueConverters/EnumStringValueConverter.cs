using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using BoilerPlate.Core.Extensions;

namespace BoilerPlate.Data.DAL.ValueConverters;

public class EnumStringValueConverter<TEnum> : ValueConverter<TEnum, string>
    where TEnum : struct, Enum
{
    public EnumStringValueConverter() : base(v => v.ToString(), v => v.ToEnum<TEnum>())
    {
    }
}