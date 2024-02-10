using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Application.Options;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Requests;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.App.Application.RequestHandlers.System.StoredFiles;

public class DeleteFileHandler : IRequestHandler<DeleteByIdRequest<StoredFile>, IdDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly FileStorageOptions _fileStorageOptions;
    private readonly IMapper _mapper;

    public DeleteFileHandler(IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory, IMapper mapper,
        IOptions<FileStorageOptions> fileStorageOptions)
    {
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
        _mapper = mapper;
        _fileStorageOptions = fileStorageOptions.Value;
    }

    public async Task<IdDto> Handle(DeleteByIdRequest<StoredFile> request, CancellationToken ct)
    {
        var storedFile = await _unitOfWork.Repository<StoredFile>().GetByIdAsync(request.Id, ct);
        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            storedFile == null,
            ExceptionCode.System_StoredFiles_DeleteFile_StoredFileNotFound,
            nameof(request.Id));

        var fileName = storedFile!.Id + Path.GetExtension(storedFile.Name);
        var filePath = Path.Combine(_fileStorageOptions.RootDirectory, fileName);

        var isFileExist = File.Exists(filePath);
        if (isFileExist)
        {
            File.Delete(filePath);
        }

        await _unitOfWork.WithTransactionAsync(() =>
        {
            _unitOfWork.Repository<StoredFile>().Remove(storedFile!);
        }, ct);

        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            isFileExist == false,
            ExceptionCode.System_StoredFiles_DeleteFile_FileNotFound,
            nameof(request.Id));

        return _mapper.Map<IdDto>(storedFile);
    }
}