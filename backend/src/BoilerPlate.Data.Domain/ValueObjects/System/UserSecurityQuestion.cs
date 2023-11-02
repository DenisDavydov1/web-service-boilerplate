using BoilerPlate.Data.Domain.ValueObjects.Base;

namespace BoilerPlate.Data.Domain.ValueObjects.System;

/// <summary>
/// Secret questions for user account recovery
/// </summary>
public class UserSecurityQuestion : BaseValueObject
{
    /// <summary> Secret question </summary>
    public string Question { get; set; } = null!;

    /// <summary> Answer SHA384 hash </summary>
    public string AnswerHash { get; set; } = null!;
}