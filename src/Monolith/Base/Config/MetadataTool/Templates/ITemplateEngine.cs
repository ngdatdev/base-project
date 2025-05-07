namespace MetadataTool.Templates;

/// <summary>
/// Interface for a template engine
/// </summary>
public interface ITemplateEngine
{
    public string RenderTemplate(string templateName, object model);
}
