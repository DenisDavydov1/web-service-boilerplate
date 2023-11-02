using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json.Linq;

namespace BoilerPlate.Services.Kafka.Serializers;

public class StringBytesToJObjectDeserializer : IDeserializer<JObject?>
{
    public JObject? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return null;
        }

        var json = Encoding.UTF8.GetString(data);
        if (json.ToLower() == "null")
        {
            return null;
        }

        var jObject = JObject.Parse(json);

        return jObject;
    }
}