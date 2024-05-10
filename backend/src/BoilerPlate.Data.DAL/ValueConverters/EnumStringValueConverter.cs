using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using BoilerPlate.Core.Extensions;

namespace BoilerPlate.Data.DAL.ValueConverters;

public class EnumStringValueConverter<TEnum>()
    : ValueConverter<TEnum, string>(v => v.ToString(), v => v.ToEnum<TEnum>())
    where TEnum : struct, Enum;