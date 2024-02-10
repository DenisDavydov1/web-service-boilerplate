using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Application.Options;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;
using BoilerPlate.Data.DTO.System.StoredFiles.Responses;

namespace BoilerPlate.App.Application.RequestHandlers.System.StoredFiles;

public class DownloadFileHandler : IRequestHandler<DownloadFileRequest, DownloadFileResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly FileStorageOptions _fileStorageOptions;

    public DownloadFileHandler(IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory,
        IOptions<FileStorageOptions> fileStorageOptions)
    {
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
        _fileStorageOptions = fileStorageOptions.Value;
    }

    public async Task<DownloadFileResponse> Handle(DownloadFileRequest request, CancellationToken ct)
    {
        var storedFile = await _unitOfWork.Repository<StoredFile>().GetByIdAsync(request.Id, ct);
        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            storedFile == null,
            ExceptionCode.System_StoredFiles_DownloadFile_StoredFileNotFound,
            nameof(request.Id));

        var fileName = storedFile!.Id + Path.GetExtension(storedFile.Name);
        var filePath = Path.Combine(_fileStorageOptions.RootDirectory, fileName);

        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            File.Exists(filePath) == false,
            ExceptionCode.System_StoredFiles_DownloadFile_FileNotFound,
            nameof(request.Id));

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