using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.Configurations.Base;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    protected abstract string Schema { get; }
    protected abstract string Table { get; }

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(Table, Schema);
        ConfigureProperties(builder);
        ConfigureRelations(builder);
    }

    protected virtual void ConfigureProperties(EntityTypeBuilder<TEntity> builder) =>
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();

    protected virtual void ConfigureRelations(EntityTypeBuilder<TEntity> builder) =>
        builder.HasKey(x => x.Id);
}