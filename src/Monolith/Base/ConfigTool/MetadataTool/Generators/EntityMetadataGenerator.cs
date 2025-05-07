using System.Linq;
using ConfigTool.MetadataTool.Models;
using ConfigTool.MetadataTool.Templates;

namespace ConfigTool.MetadataTool.Generators;

/// <summary>
/// EntityMetadata Generator
/// </summary>
public class EntityMetadataGenerator : ICodeGenerator<EntityMetadata>
{
    private readonly string _outputNamespace;
    private readonly string _defaultSchema;
    private readonly ITemplateEngine _templateEngine;
    private string _entitiesDir = "Entities";
    private string _metadataDir = "Metadata";

    public EntityMetadataGenerator(
        string outputNamespace,
        string defaultSchema,
        ITemplateEngine templateEngine = null,
        string entitiesDir = "Entities",
        string metadataDir = "Metadata"
    )
    {
        _outputNamespace = outputNamespace;
        _defaultSchema = defaultSchema;
        _templateEngine = templateEngine;
        _entitiesDir = entitiesDir;
        _metadataDir = metadataDir;
    }

    public string Generate(EntityMetadata entityType)
    {
        string imports = GenerateImports(_entitiesDir);

        return _templateEngine.RenderTemplate(
            "EntityMetadata",
            new
            {
                Import = imports,
                Namespace = $"{_outputNamespace}.{_metadataDir}",
                Entity = entityType.Name,
                Schema = _defaultSchema,
                Columns = entityType.Properties.ToDictionary(kvp => kvp.Name, kvp => kvp.Name)
            }
        );
    }

    private string GenerateImports(string entitiesDir)
    {
        if (string.IsNullOrEmpty(entitiesDir))
        {
            return string.Empty;
        }
        return $"using {_outputNamespace}.{entitiesDir};";
    }
}
