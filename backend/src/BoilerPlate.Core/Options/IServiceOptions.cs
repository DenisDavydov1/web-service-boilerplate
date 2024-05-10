namespace BoilerPlate.Core.Options;

public interface IServiceOptions
{
    static abstract string SectionName { get; }

    bool Enabled { get; }
}