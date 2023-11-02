using Newtonsoft.Json.Linq;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Notifications.Attributes;
using BoilerPlate.Data.Notifications.Common;

namespace BoilerPlate.Data.Notifications.System.Tests;

[KafkaMessage(KafkaMessageType.System_Tests_WithPayload)]
public class TestWithPayloadKafkaMessageNotification
    : BaseKafkaMessageWithPayloadNotification<TestWithPayloadMessageNotificationPayload>
{
    public TestWithPayloadKafkaMessageNotification(Guid messageId, KafkaMessageType type, JObject payload)
        : base(messageId, type, payload)
    {
    }
}