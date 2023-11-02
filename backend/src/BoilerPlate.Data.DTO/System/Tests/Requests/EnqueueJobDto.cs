using Newtonsoft.Json.Linq;
using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.System.Tests.Requests;

/// <summary>
/// Start new job
/// </summary>
public class EnqueueJobDto : BaseDto
{
    /// <summary> Job name </summary>
    public required string JobName { get; init; } = null!;

    /// <summary> Job args </summary>
    public JObject? Payload { get; set; }
}