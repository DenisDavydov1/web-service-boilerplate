using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;
using BoilerPlate.Core.Serialization;

namespace BoilerPlate.Services.Kafka.Serializers;

public class ObjectToStringBytesSerializer : IAsyncSerializer<object?>
{
    public Task<byte[]> SerializeAsync(object? data, SerializationContext context)
    {
        var json = JsonConvert.SerializeObject(data, SerializationSettings.Default);
        return Task.FromResult(Encoding.UTF8.GetBytes(json));
    }
}