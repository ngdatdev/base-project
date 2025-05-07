using System;
using System.Collections.Generic;

namespace MetadataTool.Templates;

public class MetadataTemplateEngine : ITemplateEngine
{
    private readonly Dictionary<string, Func<object, string>> _templates;

    public MetadataTemplateEngine()
    {
        _templates = new Dictionary<string, Func<object, string>>
        {
            ["ITableMetadata"] = model =>
                MetadataTemplate.GetITableMetadata(GetPropertyValue<string>(model, "Namespace")),
            ["EntityMetadata"] = model =>
                MetadataTemplate.GetEntityMetadata(
                    GetPropertyValue<string>(model, "Import"),
                    GetPropertyValue<string>(model, "Namespace"),
                    GetPropertyValue<string>(model, "Entity"),
                    GetPropertyValue<string>(model, "Schema"),
                    GetPropertyValue<Dictionary<string, string>>(model, "Columns")
                )
        };
    }

    public string RenderTemplate(string templateName, object model)
    {
        if (!_templates.TryGetValue(templateName, out var template))
            throw new ArgumentException($"Template '{templateName}' not found");

        return template(model);
    }

    private T GetPropertyValue<T>(object obj, string propertyName)
    {
        var propertyInfo = obj.GetType().GetProperty(propertyName);
        if (propertyInfo == null)
            throw new ArgumentException(
                $"Property '{propertyName}' not found on object of type {obj.GetType().Name}"
            );

        var value = propertyInfo.GetValue(obj);
        if (value == null)
            return default;

        return (T)value;
    }
}
