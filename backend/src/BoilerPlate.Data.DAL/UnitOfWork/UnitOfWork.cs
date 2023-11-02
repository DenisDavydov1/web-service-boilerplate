using Microsoft.EntityFrameworkCore.Storage;
using BoilerPlate.Data.DAL.Repositories;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    private readonly BoilerPlateDbContext _dbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(BoilerPlateDbContext dbContext) =>
        _dbContext = dbContext;

    public void Dispose() => _dbContext.Dispose();

    public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        => new Repository<TEntity>(_dbContext);

    public IIdRepository<TIdEntity> IdRepository<TIdEntity>() where TIdEntity : BaseIdEntity
        => new IdRepository<TIdEntity>(_dbContext);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            throw new Exception("Transaction has already begun");
        }

        _transaction = await _dbContext.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaction == null)
        {
            throw new Exception("Transaction has not began");
        }

        await _transaction.CommitAsync(ct);
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction == null)
        {
            throw new Exception("Transaction has not began");
        }

        await _transaction.RollbackAsync(ct);
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task SaveAsync(CancellationToken ct = default) =>
        await _dbContext.SaveChangesAsync(ct);

    public async Task WithTransactionAsync(Func<Task> task, CancellationToken ct = default)
    {
        try
        {
            await BeginTransactionAsync(ct);
            await task();
            await SaveAsync(ct);
            await CommitAsync(ct);
        }
        catch (Exception)
        {
            await RollbackAsync(ct);
            throw;
        }
    }

    public async Task WithTransactionAsync(Action action, CancellationToken ct = default) =>
        await WithTransactionAsync(async () =>
        {
            action();
            await Task.CompletedTask;
        }, ct);
}