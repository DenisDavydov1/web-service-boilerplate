using System.Net.Mime;
using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Handlers.Options;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;
using BoilerPlate.Data.DTO.System.StoredFiles.Responses;
using Microsoft.AspNetCore.StaticFiles;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.StoredFiles;

public class DownloadFileHandler(
    IUnitOfWork unitOfWork,
    IExceptionFactory exceptionFactory,
    IOptions<FileStorageOptions> fileStorageOptions)
    : IRequestHandler<DownloadFileRequest, DownloadFileResponse>
{
    private readonly FileStorageOptions _fileStorageOptions = fileStorageOptions.Value;

    public async Task<DownloadFileResponse> Handle(DownloadFileRequest request, CancellationToken ct)
    {
        var storedFile = await unitOfWork.Repository<StoredFile>().GetByIdAsync(request.Id, ct);
        exceptionFactory.ThrowIf<EntityNotFoundException>(
            storedFile == null,
            ExceptionCode.System_StoredFiles_DownloadFile_StoredFileNotFound,
            args: [nameof(request.Id)]);

        var fileName = storedFile!.Id + Path.GetExtension(storedFile.Name);
        var filePath = Path.Combine(_fileStorageOptions.RootDirectory, fileName);

        exceptionFactory.ThrowIf<EntityNotFoundException>(
            File.Exists(filePath) == false,
            ExceptionCode.System_StoredFiles_DownloadFile_FileNotFound,
            args: [nameof(request.Id)]);

        var bytes = await File.ReadAllBytesAsync(filePath, ct);
        var isContentTypeFound = new FileExtensionContentTypeProvider()
            .TryGetContentType(storedFile.Name, out var contentType);

        return new DownloadFileResponse
        {
            FileContents = bytes,
            ContentType = isContentTypeFound ? contentType! : MediaTypeNames.Application.Octet,
            FileName = storedFile.Name
        };
    }
}