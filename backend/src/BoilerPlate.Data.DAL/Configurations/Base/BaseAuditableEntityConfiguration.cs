using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Data.DAL.Configurations.Base;

public abstract class BaseAuditableEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity>
    where TEntity : BaseAuditableEntity
{
    protected override void ConfigureProperties(EntityTypeBuilder<TEntity> builder)
    {
        base.ConfigureProperties(builder);

        builder.Property(x => x.CreatedBy).HasColumnName("created_by").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("updated_by");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamptz");
    }

    protected override void ConfigureRelations(EntityTypeBuilder<TEntity> builder)
    {
        base.ConfigureRelations(builder);

        builder.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedBy);
        builder.HasOne<User>().WithMany().HasForeignKey(x => x.UpdatedBy);
    }
}