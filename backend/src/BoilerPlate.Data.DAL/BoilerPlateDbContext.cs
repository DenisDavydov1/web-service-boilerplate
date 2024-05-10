using Microsoft.EntityFrameworkCore;
using BoilerPlate.Data.DAL.Configurations.System;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Data.DAL;

public class BoilerPlateDbContext(DbContextOptions<BoilerPlateDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("uuid-ossp");

        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new StoredFileConfiguration().Configure(modelBuilder.Entity<StoredFile>());
    }
}