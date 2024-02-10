using BoilerPlate.Data.DAL.Repositories;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
    Task SaveAsync(CancellationToken ct = default);
    Task WithTransactionAsync(Func<Task> task, CancellationToken ct = default);
    Task WithTransactionAsync(Action action, CancellationToken ct = default);
}