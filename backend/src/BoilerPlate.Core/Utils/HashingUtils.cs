using System.Security.Cryptography;
using System.Text;

namespace BoilerPlate.Core.Utils;

public static class HashingUtils
{
    public static string HashBCrypt(string value) =>
        BCrypt.Net.BCrypt.HashPassword(ConvertToBase64(value));

    public static bool VerifyBCrypt(string value, string hash) =>
        BCrypt.Net.BCrypt.Verify(ConvertToBase64(value), hash);

    public static string HashMd5(string value)
    {
        var inputBytes = Encoding.ASCII.GetBytes(value);
        var hashBytes = MD5.HashData(inputBytes);
        return Convert.ToHexString(hashBytes).ToLower();
    }

    private static string ConvertToBase64(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }
}