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

    protected abstract void ConfigureProperties(EntityTypeBuilder<TEntity> builder);
    protected abstract void ConfigureRelations(EntityTypeBuilder<TEntity> builder);
}