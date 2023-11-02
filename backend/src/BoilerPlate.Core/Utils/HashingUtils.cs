using System.Text;

namespace BoilerPlate.Core.Utils;

public static class HashingUtils
{
    public static string Hash(string value) =>
        BCrypt.Net.BCrypt.HashPassword(ConvertToBase64(value));

    public static bool Verify(string value, string hash) =>
        BCrypt.Net.BCrypt.Verify(ConvertToBase64(value), hash);

    private static string ConvertToBase64(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }
}