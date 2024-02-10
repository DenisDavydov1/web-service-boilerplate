using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> DbSet;

    public Repository(BoilerPlateDbContext dbContext) =>
        DbSet = dbContext.Set<T>();

    public async Task AddAsync(T entity, CancellationToken ct = default) =>
        await DbSet.AddAsync(entity, ct);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default) =>
        await DbSet.AddRangeAsync(entities, ct);

    public void Update(T entity) =>
        DbSet.Update(entity);

    public void Remove(T entity) =>
        DbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) =>
        DbSet.RemoveRange(entities);

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(expression, ct);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync([id], cancellationToken: ct);

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default)
        => await DbSet.Where(expression).ToListAsync(ct);

    public IQueryable<T> GetAllAsQueryable()
        => DbSet.AsQueryable();

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default)
        => await DbSet.AnyAsync(expression, ct);

    public IQueryable<T> FromSql(string sql)
        => DbSet.FromSqlRaw(sql);

    public void Attach(T entity) =>
        DbSet.Attach(entity);

    public void Detach(T entity) =>
        DbSet.Entry(entity).State = EntityState.Detached;
}