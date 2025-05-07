using System.Collections.Generic;

namespace BaseApiReference.Metadata
{
    public interface ITableMetadata
    {
        string Name { get; }
        string Schema { get; }
        IReadOnlyDictionary<string, string> ColumnMappings { get; }

        public string GetColumnName(string propertyName)
        {
            return ColumnMappings.TryGetValue(propertyName, out var columnName)
                ? columnName
                : propertyName;
        }
    }
}
