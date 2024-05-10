using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Handlers.Options;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Requests;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.StoredFiles;

public class DeleteFileHandler(
    IUnitOfWork unitOfWork,
    IExceptionFactory exceptionFactory,
    IMapper mapper,
    IOptions<FileStorageOptions> fileStorageOptions)
    : IRequestHandler<DeleteByIdRequest<StoredFile>, IdDto>
{
    private readonly FileStorageOptions _fileStorageOptions = fileStorageOptions.Value;

    public async Task<IdDto> Handle(DeleteByIdRequest<StoredFile> request, CancellationToken ct)
    {
        var storedFile = await unitOfWork.Repository<StoredFile>().GetByIdAsync(request.Id, ct);
        exceptionFactory.ThrowIf<EntityNotFoundException>(
            storedFile == null,
            ExceptionCode.System_StoredFiles_DeleteFile_StoredFileNotFound,
            args: [nameof(request.Id)]);

        var fileName = storedFile!.Id + Path.GetExtension(storedFile.Name);
        var filePath = Path.Combine(_fileStorageOptions.RootDirectory, fileName);

        var isFileExist = File.Exists(filePath);
        if (isFileExist)
        {
            File.Delete(filePath);
        }

        await unitOfWork.WithTransactionAsync(() =>
        {
            unitOfWork.Repository<StoredFile>().Remove(storedFile!);
        }, ct);

        exceptionFactory.ThrowIf<EntityNotFoundException>(
            isFileExist == false,
            ExceptionCode.System_StoredFiles_DeleteFile_FileNotFound,
            args: [nameof(request.Id)]);

        return mapper.Map<IdDto>(storedFile);
    }
}