using Microsoft.EntityFrameworkCore;
using BoilerPlate.Data.DAL.Configurations.System;
using BoilerPlate.Data.Domain.Entities.System;

namespace BoilerPlate.Data.DAL;

public class BoilerPlateDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public BoilerPlateDbContext(DbContextOptions<BoilerPlateDbContext> options) : base(options)
    {
    }

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