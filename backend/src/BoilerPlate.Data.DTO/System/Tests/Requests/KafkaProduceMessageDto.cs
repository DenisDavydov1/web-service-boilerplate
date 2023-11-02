using MediatR;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.System.Tests.Requests;

/// <summary> Test produce message </summary>
public class KafkaProduceMessageDto : BaseDto, IRequest
{
    /// <summary> Topic to produce in </summary>
    public required string Topic { get; init; }

    /// <summary> Partition in a topic </summary>
    public int? Partition { get; set; }

    /// <summary> Message type </summary>
    public required KafkaMessageType Type { get; set; }

    /// <summary> Message value </summary>
    public object? Payload { get; set; }
}