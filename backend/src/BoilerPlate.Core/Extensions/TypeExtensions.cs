namespace BoilerPlate.Core.Extensions;

public static class TypeExtensions
{
    public static bool IsInheritedFromOpenGenericType(this Type type, Type openGenericType)
    {
        var currentType = type;

        while (currentType != null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == openGenericType)
            {
                return true;
            }

            currentType = currentType.BaseType;
        }

        return false;
    }
}