using System.Diagnostics.CodeAnalysis;
using BoilerPlate.Core.Attributes;
using BoilerPlate.Core.Constants;

namespace BoilerPlate.App.Validators;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ValidationErrorCode
{
#region Common

    [Localization(Code = LanguageCodes.English, Value = "The value must not be empty")]
    [Localization(Code = LanguageCodes.Russian, Value = "Значение не должно быть пустым")]
    Common_NotEmpty,

    [Localization(Code = LanguageCodes.English, Value = "Invalid e-mail address")]
    [Localization(Code = LanguageCodes.Russian, Value = "Не корректный e-mail адрес")]
    Common_EmailAddress,

    [Localization(Code = LanguageCodes.English, Value = "The value has an incorrect length")]
    [Localization(Code = LanguageCodes.Russian, Value = "Значение имеет некорректную длину")]
    Common_Length,

    [Localization(Code = LanguageCodes.English, Value = "Unknown value")]
    [Localization(Code = LanguageCodes.Russian, Value = "Неизвестное значение")]
    Common_IsInEnum,

#endregion

#region System

    #region Users

    [Localization(Code = LanguageCodes.English, Value = "The password must contain at least one uppercase letter, lowercase letter, and a digit")]
    [Localization(Code = LanguageCodes.Russian, Value = "Пароль должен содержать хотя бы одну заглавную букву, строчную букву и цифру")]
    System_Users_PasswordValidCharacters,

    #endregion

#endregion
}