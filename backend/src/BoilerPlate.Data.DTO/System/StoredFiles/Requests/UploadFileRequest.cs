using MediatR;
using Microsoft.AspNetCore.Http;
using BoilerPlate.Data.DTO.Common.Responses;

namespace BoilerPlate.Data.DTO.System.StoredFiles.Requests;

/// <summary>
/// Upload file to storage
/// </summary>
public class UploadFileRequest : IRequest<IdDto>
{
    /// <summary> File </summary>
    public required IFormFile File { get; set; } = null!;
    //
    // /// <summary> Uploaded user ID </summary>
    // public required Guid UploadedBy { get; set; }
}