using Bogus;
using BoilerPlate.Core.Constants;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.Domain.ValueObjects.System;
using BoilerPlate.Data.Seeds.Constants;

namespace BoilerPlate.Tests.Seeds.Fakers.Domain.Entities.System;

public class UserFaker : BaseFaker<User>
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string LanguageCode { get; set; }
    public UserRole Role { get; set; }
    public IEnumerable<UserSecurityQuestion> SecurityQuestions { get; set; }
    public Guid? AccessTokenId { get; set; }
    public DateTime? AccessTokenExpiresAt { get; set; }
    public Guid? RefreshTokenId { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    public UserFaker(IEnumerable<UserSecurityQuestion> securityQuestions)
    {
        Id = Guid.NewGuid();
        Login = FakerHub.Random.String2(10);
        Password = FakerHub.Internet.Password(prefix: "1Aa");
        LanguageCode = LanguageCodes.English;
        Role = UserRole.User;
        SecurityQuestions = securityQuestions;
        CreatedBy = SeedConstants.RootUserId;
        CreatedAt = DateTime.UtcNow;
    }

    protected override User Create(Faker faker)
    {
        var entity = new User
        {
            Id = Id,
            Login = Login,
            PasswordHash = HashingUtils.Hash(Password),
            Name = Name,
            Email = Email,
            LanguageCode = LanguageCode,
            Role = Role,
            SecurityQuestions = SecurityQuestions,
            AccessTokenId = AccessTokenId,
            AccessTokenExpiresAt = AccessTokenExpiresAt,
            RefreshTokenId = RefreshTokenId,
            RefreshTokenExpiresAt = RefreshTokenExpiresAt,
            CreatedBy = CreatedBy,
            CreatedAt = CreatedAt,
            UpdatedBy = UpdatedBy,
            UpdatedAt = UpdatedAt,
            DeletedBy = DeletedBy,
            DeletedAt = DeletedAt
        };

        return entity;
    }
}