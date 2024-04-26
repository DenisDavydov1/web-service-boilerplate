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

public class GetByIdHandler<TEntity, TDto> : IRequestHandler<GetByIdRequest<TEntity, TDto>, TDto>
    where TEntity : BaseEntity
    where TDto : BaseEntityDto
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly IMapper _mapper;

    public GetByIdHandler(IMapper mapper, IExceptionFactory exceptionFactory, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _exceptionFactory = exceptionFactory;
        _unitOfWork = unitOfWork;
    }

    public async Task<TDto> Handle(GetByIdRequest<TEntity, TDto> request, CancellationToken ct)
    {
        var entity = await _unitOfWork.Repository<TEntity>().GetByIdAsync(request.Id, ct);
        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            entity == null,
            ExceptionCode.Common_GetById_EntityNotFound,
            args: [nameof(request.Id)],
            formatArgs: [typeof(TEntity).Name, request.Id]);

        return _mapper.Map<TDto>(entity);
    }
}