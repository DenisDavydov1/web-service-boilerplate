namespace BoilerPlate.Data.DTO.System.Authentication.Responses;

/// <summary>
/// User authentication result
/// </summary>
public class JwtTokensDto
{
    /// <summary> Access token </summary>
    public required string AccessToken { get; init; } = null!;

    /// <summary> Refresh token </summary>
    public required string RefreshToken { get; init; } = null!;
}