using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoilerPlate.Data.DAL.Configurations.Base;
using BoilerPlate.Data.DAL.Extensions;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Data.DAL.Configurations.System;

public class UserConfiguration : BaseSoftDeletableEntityConfiguration<User>
{
    protected override string Schema => Schemas.Public;
    protected override string Table => Tables.System.Users;

    protected override void ConfigureProperties(EntityTypeBuilder<User> builder)
    {
        base.ConfigureProperties(builder);

        builder.Property(x => x.Login).HasColumnName("login").IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.LanguageCode).HasColumnName("language_code");
        builder.Property(x => x.Role).HasColumnName("role").HasEnumStringConversion().IsRequired();
        builder.Property(x => x.SecurityQuestions).HasColumnName("security_questions").HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.AccessTokenId).HasColumnName("access_token_id");
        builder.Property(x => x.AccessTokenExpiresAt).HasColumnName("access_token_expires_at");
        builder.Property(x => x.RefreshTokenId).HasColumnName("refresh_token_id");
        builder.Property(x => x.RefreshTokenExpiresAt).HasColumnName("refresh_token_expires_at");
    }

    protected override void ConfigureRelations(EntityTypeBuilder<User> builder)
    {
        base.ConfigureRelations(builder);

        builder.HasIndex(x => x.Login).IsUnique();
    }
}