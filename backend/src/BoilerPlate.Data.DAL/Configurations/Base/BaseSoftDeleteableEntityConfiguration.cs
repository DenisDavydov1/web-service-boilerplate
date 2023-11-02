using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Data.DAL.Configurations.Base;

public abstract class BaseSoftDeletableEntityConfiguration<TEntity> : BaseAuditableEntityConfiguration<TEntity>
    where TEntity : BaseSoftDeletableEntity
{
    protected override void ConfigureProperties(EntityTypeBuilder<TEntity> builder)
    {
        base.ConfigureProperties(builder);

        builder.Property(x => x.DeletedBy).HasColumnName("deleted_by");
        builder.Property(x => x.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamptz");
    }

    protected override void ConfigureRelations(EntityTypeBuilder<TEntity> builder)
    {
        base.ConfigureRelations(builder);

        builder.HasOne<User>().WithMany().HasForeignKey(x => x.DeletedBy);
    }
}