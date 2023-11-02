using System.Diagnostics.CodeAnalysis;

namespace BoilerPlate.Data.Abstractions.Enums;

/// <summary>
/// Message type
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum KafkaMessageType
{
    /// <summary> Log received message </summary>
    System_Tests_Log,

    /// <summary> Test with payload </summary>
    System_Tests_WithPayload,

    /// <summary> Test without payload </summary>
    System_Tests_NoPayload,
}