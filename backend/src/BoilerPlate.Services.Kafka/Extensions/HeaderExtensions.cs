using System.Text;
using Confluent.Kafka;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Services.Kafka.Constants;

namespace BoilerPlate.Services.Kafka.Extensions;

public static class HeaderExtensions
{
    public static Guid GetMessageId(this Headers headers)
    {
        var bytes = headers.GetValueBytes(MessageHeaders.MessageId);
        return new Guid(bytes);
    }

    public static KafkaMessageType GetMessageType(this Headers headers)
    {
        var bytes = headers.GetValueBytes(MessageHeaders.MessageType);
        var typeStr = Encoding.UTF8.GetString(bytes);
        return Enum.Parse<KafkaMessageType>(typeStr, ignoreCase: true);
    }

    private static byte[] GetValueBytes(this Headers headers, string headerKey)
        => headers.First(x => x.Key == headerKey).GetValueBytes();
}