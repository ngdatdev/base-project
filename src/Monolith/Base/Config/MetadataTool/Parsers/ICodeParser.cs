namespace Config.MetadataTool.Parsers;

/// <summary>
/// Interface for a code parser
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICodeParser<T>
    where T : class
{
    /// <summary>
    /// Parse a file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    T Parse(string filePath);
}
