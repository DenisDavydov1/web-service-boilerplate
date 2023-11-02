using Newtonsoft.Json.Linq;
using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.Data.Notifications.Common;

public abstract class BaseKafkaMessageWithPayloadNotification<TPayload> : BaseKafkaMessageNotification
    where TPayload : class
{
    public TPayload Payload { get; init; }

    protected BaseKafkaMessageWithPayloadNotification(Guid messageId, KafkaMessageType messageType, JObject payload)
        : base(messageId, messageType) =>
        Payload = payload.ToObject<TPayload>() ?? throw new Exception("Payload deserialization error");
}