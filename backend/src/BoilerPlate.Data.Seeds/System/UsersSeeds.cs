using BoilerPlate.Core.Constants;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.Domain.ValueObjects.System;
using BoilerPlate.Data.Seeds.Common;
using BoilerPlate.Data.Seeds.Constants;

namespace BoilerPlate.Data.Seeds.System;

internal class UsersSeeds : BaseSoftDeletableSeeds<User>
{
    protected override IEnumerable<User> Seeds => new User[]
    {
        new()
        {
            Id = SeedConstants.RootUserId,
            Login = "root",
            PasswordHash = string.Empty,
            Name = "root",
            LanguageCode = LanguageCodes.English,
            Role = UserRole.Viewer,
            SecurityQuestions = Array.Empty<UserSecurityQuestion>(),
            CreatedBy = SeedConstants.RootUserId
        },
        new()
        {
            Id = Guid.Parse("ffbd7478-11b6-45d4-8a5f-cc6ca66448fc"),
            Login = SeedConstants.AdminLogin,
            PasswordHash = HashingUtils.Hash(SeedConstants.AdminPassword),
            Name = "Admin",
            Email = "admin@boilerPlate.com",
            LanguageCode = LanguageCodes.English,
            Role = UserRole.Admin,
            SecurityQuestions = Array.Empty<UserSecurityQuestion>(),
            CreatedBy = SeedConstants.RootUserId
        },
        new()
        {
            Id = Guid.Parse("fded35a5-f10b-4e4b-aaf5-00eacbe939ad"),
            Login = SeedConstants.UserLogin,
            PasswordHash = HashingUtils.Hash(SeedConstants.UserPassword),
            Name = "User",
            Email = "user@boilerPlate.com",
            LanguageCode = LanguageCodes.Russian,
            Role = UserRole.User,
            SecurityQuestions = Array.Empty<UserSecurityQuestion>(),
            CreatedBy = SeedConstants.RootUserId
        }
    };

    protected override void UpdateEntity(User entity, User existingEntity)
    {
        entity.AccessTokenId = existingEntity.AccessTokenId;
        entity.AccessTokenExpiresAt = existingEntity.AccessTokenExpiresAt;
        entity.RefreshTokenId = existingEntity.RefreshTokenId;
        entity.RefreshTokenExpiresAt = existingEntity.RefreshTokenExpiresAt;
    }
}