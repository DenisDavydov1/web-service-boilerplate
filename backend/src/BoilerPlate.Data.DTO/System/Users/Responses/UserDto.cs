using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Abstractions.System;
using BoilerPlate.Data.DTO.Base;

namespace BoilerPlate.Data.DTO.System.Users.Responses;

/// <summary>
/// <inheritdoc cref="IUser"/> DTO
/// </summary>
public class UserDto : BaseSoftDeletableDto, IUser
{
    /// <inheritdoc />
    public required string Login { get; set; } = null!;

    /// <inheritdoc />
    public string? Name { get; set; }

    /// <inheritdoc />
    public string? Email { get; set; }

    /// <inheritdoc />
    public required string LanguageCode { get; set; } = null!;

    /// <inheritdoc />
    public required UserRole Role { get; set; }
}