using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoilerPlate.Data.DAL.ValueConverters;

namespace BoilerPlate.Data.DAL.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TEnum> HasEnumStringConversion<TEnum>(this PropertyBuilder<TEnum> propertyBuilder)
        where TEnum : struct, Enum =>
        propertyBuilder.HasConversion(new EnumStringValueConverter<TEnum>());
}