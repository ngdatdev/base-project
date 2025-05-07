using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataTool.Templates;

public static class MetadataTemplate
{
    public static string GetITableMetadata(string namespaceName)
    {
        return $@"
using System.Collections.Generic;

namespace {namespaceName}
{{
    public interface ITableMetadata
    {{
        string Name {{ get; }}
        string Schema {{ get; }}
        IReadOnlyDictionary<string, string> ColumnMappings {{ get; }}
    }}
}}".Trim();
    }

    public static string GetEntityMetadata(
        string import,
        string namespaceName,
        string entity,
        string schema,
        Dictionary<string, string> columns
    )
    {
        var importLines = string.IsNullOrEmpty(import)
            ? string.Empty
            : $"{import}{Environment.NewLine}";

        string tab = "        ";
        var columnMappingLines = columns
            .Select(c => $"{tab}{{ nameof({entity}.{c.Key}), \"{c.Value}\" }}")
            .ToList();
        var columnsString = string.Join($",{Environment.NewLine}", columnMappingLines);

        return $@"
using System.Collections.Generic;
{importLines}
namespace {namespaceName};

/// <summary>
/// Metadata for {entity}
/// /summary
public class {entity}Metadata : ITableMetadata
{{
    public string Name => ""{entity}"";
    public string Schema => ""{schema}"";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {{
{columnsString}
    }};
}}
".Trim();
    }
}
