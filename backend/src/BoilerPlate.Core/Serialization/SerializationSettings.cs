using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BoilerPlate.Core.Serialization;

public static class SerializationSettings
{
    public static JsonSerializerSettings Default => new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        },
        Converters = new JsonConverter[]
        {
            new StringEnumConverter()
        }
    };

    public static JsonSerializer Serializer => JsonSerializer.Create(Default);
}