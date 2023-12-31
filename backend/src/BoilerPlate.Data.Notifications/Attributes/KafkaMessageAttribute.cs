using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.Data.Notifications.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class KafkaMessageAttribute : Attribute
{
    public KafkaMessageType Type { get; set; }

    public KafkaMessageAttribute(KafkaMessageType type) => Type = type;
}