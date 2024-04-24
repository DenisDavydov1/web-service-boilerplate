using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoilerPlate.Core.Extensions;

public static class JObjectExtensions
{
    public static string Serialize(this JObject? jObject)
    {
        if (jObject == null) return "null";

        if (jObject.HasValues == false) return "{}";

        var sortedJObject = new JObject(jObject.Properties().OrderBy(x => x.Name));
        return sortedJObject.ToString(Formatting.None);
    }
}