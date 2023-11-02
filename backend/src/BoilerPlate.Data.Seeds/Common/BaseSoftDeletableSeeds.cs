using BoilerPlate.Data.Domain.Entities.Base;

namespace BoilerPlate.Data.Seeds.Common;

internal abstract class BaseSoftDeletableSeeds<TSoftDeletableEntity> : BaseAuditableSeeds<TSoftDeletableEntity>
    where TSoftDeletableEntity : BaseSoftDeletableEntity
{
    protected override void UpdateEntity(TSoftDeletableEntity entity, TSoftDeletableEntity existingEntity)
    {
        entity.DeletedAt = existingEntity.DeletedAt;
        entity.DeletedBy = existingEntity.DeletedBy;
    }
}