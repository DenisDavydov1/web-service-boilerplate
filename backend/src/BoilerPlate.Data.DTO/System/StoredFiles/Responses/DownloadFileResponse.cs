namespace BoilerPlate.Data.DTO.System.StoredFiles.Responses;

/// <summary>
/// Download file response
/// </summary>
public class DownloadFileResponse
{
    /// <summary> File binary data </summary>
    public required byte[] FileContents { get; set; }

    /// <summary> File mime type </summary>
    public required string ContentType { get; set; }

    /// <summary> Original file name </summary>
    public required string FileName { get; set; }
}