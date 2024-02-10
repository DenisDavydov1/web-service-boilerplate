using BoilerPlate.Data.DTO.Base;
using MediatR;
using BoilerPlate.Data.DTO.System.Authentication.Responses;

namespace BoilerPlate.Data.DTO.System.Authentication.Requests;

/// <summary>
/// User initial authentication DTO
/// </summary>
public class GetAccessTokenDto : BaseDto, IRequest<JwtTokensDto>
{
    /// <summary> User login </summary>
    public required string Login { get; init; } = null!;

    /// <summary> User password </summary>
    public required string Password { get; init; } = null!;
}