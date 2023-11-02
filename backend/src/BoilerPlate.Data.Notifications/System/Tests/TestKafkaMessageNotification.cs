using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Notifications.Attributes;
using BoilerPlate.Data.Notifications.Common;

namespace BoilerPlate.Data.Notifications.System.Tests;

[KafkaMessage(KafkaMessageType.System_Tests_NoPayload)]
public class TestKafkaMessageNotification : BaseKafkaMessageNotification
{
    public TestKafkaMessageNotification(Guid messageId, KafkaMessageType type) : base(messageId, type)
    {
    }
}