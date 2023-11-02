using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.Configurations.Base;

public abstract class BaseIdEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity>
    where TEntity : BaseIdEntity
{
    protected override void ConfigureProperties(EntityTypeBuilder<TEntity> builder) =>
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();

    protected override void ConfigureRelations(EntityTypeBuilder<TEntity> builder) =>
        builder.HasKey(x => x.Id);
}