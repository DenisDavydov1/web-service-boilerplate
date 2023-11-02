using System.Diagnostics.CodeAnalysis;
using BoilerPlate.Core.Attributes;
using BoilerPlate.Core.Constants;

namespace BoilerPlate.Core.Exceptions.Enums;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum ExceptionCode
{
    #region Common

    [Localization(Code = LanguageCodes.English, Value = "Entity with this ID not found")]
    [Localization(Code = LanguageCodes.Russian, Value = "Объект с данным ID не найден")]
    Common_GetById_EntityNotFound,

    #endregion

#region System

    #region Authentication

    [Localization(Code = LanguageCodes.English, Value = "User not found")]
    [Localization(Code = LanguageCodes.Russian, Value = "Пользователь не найден")]
    System_Authentication_GetAccessToken_UserNotFound,

    [Localization(Code = LanguageCodes.English, Value = "Password is not valid")]
    [Localization(Code = LanguageCodes.Russian, Value = "Неверный пароль")]
    System_Authentication_GetAccessToken_PasswordInvalid,

    [Localization(Code = LanguageCodes.English, Value = "User not found")]
    [Localization(Code = LanguageCodes.Russian, Value = "Пользователь не найден")]
    System_Authentication_RefreshAccessToken_UserNotFound,

    [Localization(Code = LanguageCodes.English, Value = "Invalid refresh token")]
    [Localization(Code = LanguageCodes.Russian, Value = "Токен доступа не валиден")]
    System_Authentication_RefreshAccessToken_InvalidRefreshToken,

    #endregion

    #region Users

    [Localization(Code = LanguageCodes.English, Value = "Login taken")]
    [Localization(Code = LanguageCodes.Russian, Value = "Логин занят")]
    System_Users_RegisterUser_LoginTaken,

    [Localization(Code = LanguageCodes.English, Value = "Old password is not valid")]
    [Localization(Code = LanguageCodes.Russian, Value = "Неверный старый пароль")]
    System_Users_ChangeUserPassword_OldPasswordInvalid,

    #endregion

    #region FileStorage

    [Localization(Code = LanguageCodes.English, Value = "The file path length exceeds 259 characters")]
    [Localization(Code = LanguageCodes.Russian, Value = "Длина пути к файлу превышает 259 символов")]
    System_StoredFiles_UploadFile_MaxPathLengthExceeded,

    [Localization(Code = LanguageCodes.English, Value = "File not found in DB")]
    [Localization(Code = LanguageCodes.Russian, Value = "Файл не найден в БД")]
    System_StoredFiles_DownloadFile_StoredFileNotFound,

    [Localization(Code = LanguageCodes.English, Value = "File not found in storage")]
    [Localization(Code = LanguageCodes.Russian, Value = "Файл не найден в хранилище")]
    System_StoredFiles_DownloadFile_FileNotFound,

    [Localization(Code = LanguageCodes.English, Value = "File not found in DB")]
    [Localization(Code = LanguageCodes.Russian, Value = "Файл не найден в БД")]
    System_StoredFiles_UpdateStoredFile_StoredFileNotFound,

    [Localization(Code = LanguageCodes.English, Value = "File not found in DB")]
    [Localization(Code = LanguageCodes.Russian, Value = "Файл не найден в БД")]
    System_StoredFiles_DeleteFile_StoredFileNotFound,

    [Localization(Code = LanguageCodes.English, Value = "File not found in storage")]
    [Localization(Code = LanguageCodes.Russian, Value = "Файл не найден в хранилище")]
    System_StoredFiles_DeleteFile_FileNotFound,

    #endregion

#endregion
}