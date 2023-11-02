using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.Seeds.Constants;

namespace BoilerPlate.Data.Seeds.Common;

internal abstract class BaseAuditableSeeds<TAuditableEntity> : BaseIdSeeds<TAuditableEntity>
    where TAuditableEntity : BaseAuditableEntity
{
    protected override void UpdateEntity(TAuditableEntity entity, TAuditableEntity existingEntity)
    {
        entity.CreatedAt = existingEntity.CreatedAt;
        entity.CreatedBy = existingEntity.CreatedBy;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = SeedConstants.RootUserId;
    }
}