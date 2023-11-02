namespace BoilerPlate.App.Application.Options;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";

    public string RootDirectory { get; set; } = null!;
}