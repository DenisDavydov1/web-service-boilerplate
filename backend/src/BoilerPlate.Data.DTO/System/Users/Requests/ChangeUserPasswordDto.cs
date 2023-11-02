using MediatR;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.Data.DTO.System.Users.Requests;

/// <summary>
/// Change password request DTO
/// </summary>
public class ChangeUserPasswordDto : IRequest<IdDto>
{
    /// <summary> Old user password </summary>
    public required string OldPassword { get; init; } = null!;

    /// <summary> New user password </summary>
    public required string NewPassword { get; init; } = null!;
}