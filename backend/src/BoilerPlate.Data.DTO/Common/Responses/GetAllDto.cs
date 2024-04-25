using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.Common.Responses;

/// <summary>
/// Collection with pagination DTO
/// </summary>
public class GetAllDto<TEntityDto> : BaseDto
    where TEntityDto : BaseEntityDto
{
    /// <summary> Results </summary>
    public required IEnumerable<TEntityDto> Content { get; set; }

    /// <summary> Page number </summary>
    public required int Page { get; set; }

    /// <summary> Number of results per page </summary>
    public required int ResultsPerPage { get; set; }

    /// <summary> Total items count </summary>
    public required int TotalResults { get; set; }

    /// <summary> Has no more results to load </summary>
    public required bool IsLast { get; set; }
}