using System.Linq.Expressions;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.DAL.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);

    void Update(T entity);

    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    Task<T?> GetAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);
    IQueryable<T> GetAllAsQueryable();

    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);

    IQueryable<T> FromSql(string sql);

    void Attach(T entity);
    void Detach(T entity);
}