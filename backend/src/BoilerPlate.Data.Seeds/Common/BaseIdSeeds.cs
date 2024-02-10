using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.Seeds.Common;

internal abstract class BaseIdSeeds<TEntity> : BaseSeeds where TEntity : BaseEntity
{
    protected abstract IEnumerable<TEntity> Seeds { get; }

    public override async Task SeedAsync(IUnitOfWork unitOfWork, CancellationToken ct = default)
    {
        var repository = unitOfWork.Repository<TEntity>();

        await unitOfWork.WithTransactionAsync(async () =>
        {
            foreach (var entity in Seeds)
            {
                var existingEntity = await repository.GetByIdAsync(entity.Id, ct);
                if (existingEntity != null)
                {
                    UpdateEntity(entity, existingEntity);
                    repository.Detach(existingEntity);
                    repository.Update(entity);
                }
                else
                {
                    await repository.AddAsync(entity, ct);
                }
            }
        }, ct);
    }

    protected abstract void UpdateEntity(TEntity entity, TEntity existingEntity);
}