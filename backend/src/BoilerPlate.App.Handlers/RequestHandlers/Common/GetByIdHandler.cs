using AutoMapper;
using MediatR;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;
using BoilerPlate.Data.DTO.Common.Requests;

namespace BoilerPlate.App.Handlers.RequestHandlers.Common;

public class GetByIdHandler<TEntity, TDto>(IMapper mapper, IExceptionFactory exceptionFactory, IUnitOfWork unitOfWork)
    : IRequestHandler<GetByIdRequest<TEntity, TDto>, TDto>
    where TEntity : BaseEntity
    where TDto : BaseEntityDto
{
    public async Task<TDto> Handle(GetByIdRequest<TEntity, TDto> request, CancellationToken ct)
    {
        var entity = await unitOfWork.Repository<TEntity>().GetByIdAsync(request.Id, ct);
        exceptionFactory.ThrowIf<EntityNotFoundException>(
            entity == null,
            ExceptionCode.Common_GetById_EntityNotFound,
            args: [nameof(request.Id)],
            formatArgs: [typeof(TEntity).Name, request.Id]);

        return mapper.Map<TDto>(entity);
    }
}