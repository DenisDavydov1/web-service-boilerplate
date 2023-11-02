namespace BoilerPlate.Data.Abstractions.Enums;

/// <summary>
/// User role model
/// </summary>
public enum UserRole
{
    /// <summary> Read-only user </summary>
    Viewer = 0,

    /// <summary> Regular user </summary>
    User = 1,

    /// <summary> User with moderation rights </summary>
    Moderator = 2,

    /// <summary> User with full rights </summary>
    Admin = 3,
}