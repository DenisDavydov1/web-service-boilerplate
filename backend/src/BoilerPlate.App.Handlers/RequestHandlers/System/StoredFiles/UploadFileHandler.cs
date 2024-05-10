using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Handlers.Options;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;
using BoilerPlate.Services.System.Users;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.StoredFiles;

public class UploadFileHandler(
    IUnitOfWork unitOfWork,
    IExceptionFactory exceptionFactory,
    IOptions<FileStorageOptions> fileStorageOptions,
    IMapper mapper,
    IUsersService usersService)
    : IRequestHandler<UploadFileRequest, IdDto>
{
    private const int MaxPathLength = 259;

    private readonly FileStorageOptions _fileStorageOptions = fileStorageOptions.Value;

    public async Task<IdDto> Handle(UploadFileRequest request, CancellationToken ct)
    {
        var storedFileId = Guid.NewGuid();
        var fileName = storedFileId + Path.GetExtension(request.File.FileName);
        var filePath = Path.Combine(_fileStorageOptions.RootDirectory, fileName);
        exceptionFactory.ThrowIf<BusinessException>(
            filePath.Length > MaxPathLength,
            ExceptionCode.System_StoredFiles_UploadFile_MaxPathLengthExceeded,
            args: [nameof(request.File)]);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await request.File.CopyToAsync(stream, ct);

        var storedFile = new StoredFile
        {
            Id = storedFileId,
            Name = request.File.FileName,
            CreatedBy = usersService.GetCurrentUserId()
        };

        await unitOfWork.WithTransactionAsync(async () =>
        {
            await unitOfWork.Repository<StoredFile>().AddAsync(storedFile, ct);
        }, ct);

        return mapper.Map<IdDto>(storedFile);
    }
}