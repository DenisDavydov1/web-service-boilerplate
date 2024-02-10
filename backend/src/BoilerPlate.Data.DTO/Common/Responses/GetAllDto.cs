using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.Common.Responses;

/// <summary>
/// Collection with pagination DTO
/// </summary>
public class GetAllDto<TEntityDto> : BaseDto
    where TEntityDto : BaseEntityDto
{
    /// <summary> Results </summary>
    public required IEnumerable<TEntityDto> Items { get; set; }

    /// <summary> Page number </summary>
    public required int Page { get; set; }

    /// <summary> Take results per page </summary>
    public required int PageSize { get; set; }

    /// <summary> Total items count </summary>
    public required int TotalItems { get; set; }

    /// <summary> Has more results to load </summary>
    public required bool HasMore { get; set; }
}