using BoilerPlate.Data.DTO.Base;
using MediatR;
using BoilerPlate.Data.DTO.System.Authentication.Responses;

namespace BoilerPlate.Data.DTO.System.Authentication.Requests;

/// <summary>
/// Refresh expired access token DTO
/// </summary>
public class RefreshAccessTokenDto : BaseDto, IRequest<JwtTokensDto>
{
    /// <summary> Refresh token </summary>
    public required string RefreshToken { get; init; } = null!;
}