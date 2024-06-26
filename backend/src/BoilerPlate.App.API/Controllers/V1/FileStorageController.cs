using Asp.Versioning;
using BoilerPlate.App.API.Attributes;
using BoilerPlate.App.API.Constants;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Requests;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;
using BoilerPlate.Data.DTO.System.StoredFiles.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.App.API.Controllers.V1;

/// <summary>
/// File storage controller
/// </summary>
[ApiVersion(1)]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{version:apiVersion}/file-storage")]
public class FileStorageController : BaseApiController
{
    /// <inheritdoc />
    public FileStorageController(IMediator mediator, ILogger<FileStorageController> logger,
        IExceptionFactory exceptionFactory) : base(mediator, logger, exceptionFactory)
    {
    }

    #region GET

    /// <summary> Download file from storage </summary>
    [HttpGet("{id:guid}")]
    [MinimumRoleAuthorize(UserRole.Viewer)]
    public async Task<FileContentResult> DownloadFileAsync(Guid id, CancellationToken ct)
    {
        var request = new DownloadFileRequest { Id = id };
        var response = await Mediator.Send(request, ct);
        return File(response.FileContents, response.ContentType, response.FileName);
    }

    /// <summary> Get all stored files info </summary>
    [HttpGet]
    [MinimumRoleAuthorize(UserRole.Viewer)]
    public async Task<ActionResult<GetAllDto<StoredFileDto>>> GetAllFilesInfoAsync(
        [FromQuery] int? page, [FromQuery] int? resultsPerPage,
        [FromQuery] string? sort, [FromQuery] string? filter,
        CancellationToken ct)
    {
        var request = new GetAllRequest<StoredFile, StoredFileDto>
        {
            Page = page,
            ResultsPerPage = resultsPerPage,
            Sort = sort,
            Filter = filter
        };
        var response = await Mediator.Send(request, ct);
        return response;
    }

    #endregion

    #region POST

    /// <summary> Upload file to file storage </summary>
    [HttpPost]
    [MinimumRoleAuthorize(UserRole.User)]
    public async Task<ActionResult<IdDto>> UploadFileAsync(IFormFile file, CancellationToken ct)
    {
        var request = new UploadFileRequest { File = file };
        var response = await Mediator.Send(request, ct);
        return response;
    }

    #endregion

    #region PUT

    /// <summary> Change stored file info </summary>
    [HttpPut("{id:guid}")]
    [MinimumRoleAuthorize(UserRole.User)]
    public async Task<ActionResult<IdDto>> UpdateFileInfoAsync(Guid id, [FromBody] UpdateStoredFileDto request,
        CancellationToken ct)
    {
        request.Id = id;
        var response = await Mediator.Send(request, ct);
        return response;
    }

    #endregion

    #region DELETE

    /// <summary> Delete file from storage </summary>
    [HttpDelete("{id:guid}")]
    [MinimumRoleAuthorize(UserRole.User)]
    public async Task<ActionResult<IdDto>> DeleteFileAsync(Guid id, CancellationToken ct)
    {
        var request = new DeleteByIdRequest<StoredFile> { Id = id };
        var response = await Mediator.Send(request, ct);
        return response;
    }

    #endregion
}