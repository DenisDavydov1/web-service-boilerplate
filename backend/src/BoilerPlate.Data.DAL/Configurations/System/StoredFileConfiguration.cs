using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BoilerPlate.Data.DAL.Configurations.Base;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Data.DAL.Configurations.System;

public class StoredFileConfiguration : BaseAuditableEntityConfiguration<StoredFile>
{
    protected override string Schema => Schemas.Public;
    protected override string Table => Tables.System.StoredFiles;

    protected override void ConfigureProperties(EntityTypeBuilder<StoredFile> builder)
    {
        base.ConfigureProperties(builder);

        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
    }
}