using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Application.Options;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;
using BoilerPlate.Services.System.Users;

namespace BoilerPlate.App.Application.RequestHandlers.System.StoredFiles;

public class UploadFileHandler : IRequestHandler<UploadFileRequest, IdDto>
{
    private const int MaxPathLength = 259;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly FileStorageOptions _fileStorageOptions;
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;

    public UploadFileHandler(IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory,
        IOptions<FileStorageOptions> fileStorageOptions, IMapper mapper, IUsersService usersService)
    {
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
        _fileStorageOptions = fileStorageOptions.Value;
        _mapper = mapper;
        _usersService = usersService;
    }

    public async Task<IdDto> Handle(UploadFileRequest request, CancellationToken ct)
    {
        var storedFileId = Guid.NewGuid();
        var fileName = storedFileId + Path.GetExtension(request.File.FileName);
        var filePath = Path.Combine(_fileStorageOptions.RootDirectory, fileName);
        _exceptionFactory.ThrowIf<BusinessException>(
            filePath.Length > MaxPathLength,
            ExceptionCode.System_StoredFiles_UploadFile_MaxPathLengthExceeded,
            nameof(request.File));

        await using var stream = new FileStream(filePath, FileMode.Create);
        await request.File.CopyToAsync(stream, ct);

        var storedFile = new StoredFile
        {
            Id = storedFileId,
            Name = request.File.FileName,
            CreatedBy = _usersService.GetCurrentUserId()
        };

        await _unitOfWork.WithTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<StoredFile>().AddAsync(storedFile, ct);
        }, ct);

        return _mapper.Map<IdDto>(storedFile);
    }
}