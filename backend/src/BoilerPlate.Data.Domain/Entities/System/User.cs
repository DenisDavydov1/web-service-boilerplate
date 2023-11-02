using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Abstractions.System;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.Domain.ValueObjects.System;

namespace BoilerPlate.Data.Domain.Entities.System;

/// <summary>
/// <inheritdoc cref="IUser"/> entity
/// </summary>
public class User : BaseSoftDeletableEntity, IUser
{
    /// <inheritdoc />
    public required string Login { get; set; } = null!;

    /// <summary> Password SHA384 hash </summary>
    public required string PasswordHash { get; set; } = null!;

    /// <inheritdoc />
    public string? Name { get; set; }

    /// <inheritdoc />
    public string? Email { get; set; }

    /// <inheritdoc />
    public required string LanguageCode { get; set; } = null!;

    /// <inheritdoc />
    public required UserRole Role { get; set; }

    /// <summary> Questions for account recovery </summary>
    public required IEnumerable<UserSecurityQuestion> SecurityQuestions { get; set; } = null!;

    /// <summary> Access token jti </summary>
    public Guid? AccessTokenId { get; set; }

    /// <summary> Access token exp </summary>
    public DateTime? AccessTokenExpiresAt { get; set; }

    /// <summary> Refresh token jti </summary>
    public Guid? RefreshTokenId { get; set; }

    /// <summary> Refresh token exp </summary>
    public DateTime? RefreshTokenExpiresAt { get; set; }
}