using BoilerPlate.Data.DAL.UnitOfWork;

namespace BoilerPlate.Data.Seeds.Common;

internal abstract class BaseSeeds
{
    public abstract Task SeedAsync(IUnitOfWork unitOfWork, CancellationToken ct = default);
}