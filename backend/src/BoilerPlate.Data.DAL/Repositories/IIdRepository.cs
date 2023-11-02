using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.Repositories;

public interface IIdRepository<T> : IRepository<T> where T : BaseIdEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
}