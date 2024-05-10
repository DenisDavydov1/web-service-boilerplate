using AutoMapper;
using MediatR;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;
using BoilerPlate.Services.System.Users;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.StoredFiles;

public class UpdateStoredFileHandler(
    IUnitOfWork unitOfWork,
    IExceptionFactory exceptionFactory,
    IMapper mapper,
    IUsersService usersService)
    : IRequestHandler<UpdateStoredFileDto, IdDto>
{
    public async Task<IdDto> Handle(UpdateStoredFileDto request, CancellationToken ct)
    {
        var storedFile = await unitOfWork.Repository<StoredFile>().GetByIdAsync(request.Id, ct);
        exceptionFactory.ThrowIf<EntityNotFoundException>(
            storedFile == null,
            ExceptionCode.System_StoredFiles_UpdateStoredFile_StoredFileNotFound,
            args: [nameof(request.Id)]);

        mapper.Map(request, storedFile);
        storedFile!.UpdatedBy = usersService.GetCurrentUserId();
        storedFile.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.WithTransactionAsync(() =>
        {
            unitOfWork.Repository<StoredFile>().Update(storedFile!);
        }, ct);

        return mapper.Map<IdDto>(storedFile);
    }
}