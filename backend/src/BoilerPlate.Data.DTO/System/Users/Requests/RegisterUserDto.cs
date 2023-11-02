using MediatR;
using Newtonsoft.Json;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Abstractions.System;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.Data.DTO.System.Users.Requests;

/// <summary>
/// User registration DTO
/// </summary>
public class RegisterUserDto : IUser, IRequest<IdDto>
{
    /// <inheritdoc />
    public required string Login { get; init; } = null!;

    /// <summary> Password </summary>
    public required string Password { get; init; } = null!;

    /// <inheritdoc />
    public string? Name { get; init; }

    /// <inheritdoc />
    public string? Email { get; init; }

    /// <inheritdoc />
    public required string LanguageCode { get; init; } = null!;

    /// <inheritdoc />
    [JsonIgnore]
    public UserRole Role { get; set; }

    /// <summary> Questions for account recovery </summary>
    public required IDictionary<string, string> SecurityQuestions { get; init; } = null!;
}