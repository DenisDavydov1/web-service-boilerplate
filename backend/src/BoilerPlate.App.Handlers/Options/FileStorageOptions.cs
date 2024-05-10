using BoilerPlate.Core.Options;

namespace BoilerPlate.App.Handlers.Options;

public class FileStorageOptions : IServiceOptions
{
    public static string SectionName => "FileStorage";

    public bool Enabled { get; set; } = true;

    public string RootDirectory { get; set; } = null!;
}