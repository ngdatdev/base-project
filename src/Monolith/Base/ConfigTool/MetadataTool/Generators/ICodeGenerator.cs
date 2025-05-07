namespace ConfigTool.MetadataTool.Generators;

/// <summary>
/// Interface for code generator
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICodeGenerator<T>
    where T : class
{
    /// <summary>
    /// Generate code
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    string Generate(T model);
}
