using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace BoilerPlate.Core.Extensions;

public static class JsonElementExtensions
{
    public static JObject? ToJObject(this JsonElement? obj)
    {
        var text = obj?.GetRawText();
        return text != null ? JObject.Parse(text) : null;
    }
}