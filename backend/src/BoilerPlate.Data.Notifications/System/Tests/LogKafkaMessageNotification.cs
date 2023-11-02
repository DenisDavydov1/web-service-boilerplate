using Newtonsoft.Json.Linq;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Notifications.Attributes;
using BoilerPlate.Data.Notifications.Common;

namespace BoilerPlate.Data.Notifications.System.Tests;

[KafkaMessage(KafkaMessageType.System_Tests_Log)]
[KafkaMessage(KafkaMessageType.System_Tests_WithPayload)]
public class LogKafkaMessageNotification : BaseKafkaMessageWithPayloadNotification<JObject>
{
    public LogKafkaMessageNotification(Guid messageId, KafkaMessageType messageType, JObject payload)
        : base(messageId, messageType, payload)
    {
    }
}