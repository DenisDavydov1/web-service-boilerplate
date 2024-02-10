using AutoMapper;
using MediatR;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;

namespace BoilerPlate.App.Application.RequestHandlers.System.StoredFiles;

public class UpdateStoredFileHandler : IRequestHandler<UpdateStoredFileDto, IdDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly IMapper _mapper;

    public UpdateStoredFileHandler(IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
        _mapper = mapper;
    }

    public async Task<IdDto> Handle(UpdateStoredFileDto request, CancellationToken ct)
    {
        var storedFile = await _unitOfWork.Repository<StoredFile>().GetByIdAsync(request.Id, ct);
        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            storedFile == null,
            ExceptionCode.System_StoredFiles_UpdateStoredFile_StoredFileNotFound,
            nameof(request.Id));

        _mapper.Map(request, storedFile);

        await _unitOfWork.WithTransactionAsync(() =>
        {
            _unitOfWork.Repository<StoredFile>().Update(storedFile!);
        }, ct);

        return _mapper.Map<IdDto>(storedFile);
    }
}