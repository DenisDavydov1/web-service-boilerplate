using System.Web;

namespace BoilerPlate.Services.Mail.Utils;

internal static class HtmlUtils
{
    public static string Encode(string input) =>
        HttpUtility.HtmlEncode(input)
            .Replace("\n", "<br/>")
            .Replace("\r", "");
}