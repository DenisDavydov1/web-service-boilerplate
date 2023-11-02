using MediatR;
using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.Data.Notifications.Common;

public abstract class BaseKafkaMessageNotification : INotification
{
    public Guid MessageId { get; set; }

    public KafkaMessageType MessageType { get; set; }

    protected BaseKafkaMessageNotification(Guid messageId, KafkaMessageType messageType)
    {
        MessageId = messageId;
        MessageType = messageType;
    }
}