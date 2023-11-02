using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.Base;
using BoilerPlate.Data.DTO.Base;
using BoilerPlate.Data.DTO.Common.Requests;

namespace BoilerPlate.App.Application.RequestHandlers.Common;

public class GetAllHandler<TEntity, TDto> : IRequestHandler<GetAllRequest<TEntity, TDto>, IEnumerable<TDto>>
    where TEntity : BaseEntity
    where TDto : BaseIdDto
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TDto>> Handle(GetAllRequest<TEntity, TDto> request, CancellationToken ct)
    {
        var entities = await _unitOfWork.Repository<TEntity>().GetAllAsQueryable()
            .Skip(request.Skip ?? 0)
            .Take(request.Take ?? 100)
            .ToListAsync(ct);

        return _mapper.Map<IEnumerable<TDto>>(entities);
    }
}