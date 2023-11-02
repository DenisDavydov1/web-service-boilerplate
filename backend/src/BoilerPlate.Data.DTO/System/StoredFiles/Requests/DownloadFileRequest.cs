using MediatR;
using BoilerPlate.Data.DTO.System.StoredFiles.Responses;

namespace BoilerPlate.Data.DTO.System.StoredFiles.Requests;

/// <summary>
/// File download request
/// </summary>
public class DownloadFileRequest : IRequest<DownloadFileResponse>
{
    /// <summary> Stored file ID </summary>
    public required Guid Id { get; init; }
    //
    // /// <summary> Downloader user ID </summary>
    // public required Guid DownloaderId { get; init; }
    //
    // /// <summary> Downloader user role </summary>
    // public required UserRole DownloaderRole { get; init; }
}