using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.Repositories;

public class IdRepository<T> : Repository<T>, IIdRepository<T> where T : BaseIdEntity
{
    public IdRepository(BoilerPlateDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object?[] { id }, cancellationToken: ct);
}