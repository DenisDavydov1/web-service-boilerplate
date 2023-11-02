using BoilerPlate.Data.Abstractions.Enums;

namespace BoilerPlate.Data.Abstractions.System;

/// <summary>
/// User
/// </summary>
public interface IUser
{
    /// <summary> User unique nickname </summary>
    string Login { get; }

    /// <summary> User name </summary>
    string? Name { get; }

    /// <summary> E-mail address </summary>
    string? Email { get; }

    /// <summary> User language ISO code </summary>
    string LanguageCode { get; }

    /// <summary> Role in user role model </summary>
    UserRole Role { get; set; }
}