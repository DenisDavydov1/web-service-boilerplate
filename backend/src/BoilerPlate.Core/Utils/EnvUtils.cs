namespace BoilerPlate.Core.Utils;

/// <summary>
/// Environment variables utils
/// </summary>
public static class EnvUtils
{
    /// <summary> App is launched in swagger gen mode </summary>
    public static bool IsSwaggerGen => GetBoolEnvValue("SWAGGER_GEN");

    /// <summary> App is launched with DB in-memory mode </summary>
    public static bool IsDbInMemoryMode => GetBoolEnvValue("DB_IN_MEMORY");

    private static bool GetBoolEnvValue(string key)
    {
        var envValue = Environment.GetEnvironmentVariable(key);
        var isParsed = bool.TryParse(envValue, out var result);
        return isParsed && result;
    }
}