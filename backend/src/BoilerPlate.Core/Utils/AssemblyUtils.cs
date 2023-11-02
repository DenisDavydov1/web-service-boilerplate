using System.Reflection;

namespace BoilerPlate.Core.Utils;

public static class AssemblyUtils
{
    public static Assembly[] BoilerPlateAssemblies =>
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName?.StartsWith("BoilerPlate") ?? false)
            .ToArray();

    public static IEnumerable<Type> GetTypes() => BoilerPlateAssemblies.SelectMany(x => x.GetTypes()).ToArray();

    public static Type? GetType(string name) => GetTypes().FirstOrDefault(x => x.Name == name);
}