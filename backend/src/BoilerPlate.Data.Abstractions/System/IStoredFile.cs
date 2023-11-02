namespace BoilerPlate.Data.Abstractions.System;

/// <summary>
/// Stored file
/// </summary>
public interface IStoredFile
{
    /// <summary> File name with extension </summary>
    string Name { get; }
}