using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage;
using BoilerPlate.Data.DAL.Repositories;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.UnitOfWork;

internal class UnitOfWork(BoilerPlateDbContext dbContext) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repositoriesCache = new();

    private IDbContextTransaction? _transaction;

    public void Dispose() => dbContext.Dispose();

    public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var isInCache = _repositoriesCache.TryGetValue(typeof(TEntity), out var repositoryObject);
        if (isInCache) return (IRepository<TEntity>) repositoryObject!;

        var repository = new Repository<TEntity>(dbContext);
        var isAdded = _repositoriesCache.TryAdd(typeof(TEntity), repository);
        if (!isAdded)
        {
            throw new Exception("Repository add error");
        }

        return repository;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            throw new Exception("Transaction has already started");
        }

        _transaction = await dbContext.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaction == null)
        {
            throw new Exception("Transaction has not started");
        }

        await _transaction.CommitAsync(ct);
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction == null)
        {
            throw new Exception("Transaction has not started");
        }

        await _transaction.RollbackAsync(ct);
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task SaveAsync(CancellationToken ct = default) =>
        await dbContext.SaveChangesAsync(ct);

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