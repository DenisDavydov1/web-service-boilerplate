namespace BoilerPlate.Core.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class LocalizationAttribute : Attribute
{
    public string Code { get; set; } = null!;
    public string Value { get; set; } = null!;
}