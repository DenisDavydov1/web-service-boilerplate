using BoilerPlate.Data.DTO.System.StoredFiles.Responses;
using MediatR;

namespace BoilerPlate.Data.DTO.System.StoredFiles.Requests;

/// <summary>
/// File download request
/// </summary>
public class DownloadFileRequest : IRequest<DownloadFileResponse>
{
    /// <summary> Stored file ID </summary>
    public required Guid Id { get; init; }
}