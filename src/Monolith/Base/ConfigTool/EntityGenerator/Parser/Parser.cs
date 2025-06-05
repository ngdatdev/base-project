using System.Text.RegularExpressions;
using ConfigTool.EntityGenerator.Model;
using Humanizer;

namespace ConfigTool.EntityGenerator.Parser;

/// <summary>
/// Table Schema Parser
/// </summary>
public class TableSchemaParser
{
    public List<TableSchema> ParseFromText(string input)
    {
        var tables = new List<TableSchema>();
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        TableSchema? currentTable = null;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Parse table definition
            if (trimmedLine.StartsWith("Table \"") || trimmedLine.StartsWith("table \""))
            {
                var tableName = ExtractTableName(trimmedLine);
                currentTable = new TableSchema { TableName = tableName };
                tables.Add(currentTable);
                continue;
            }

            // Parse column definition
            if (currentTable != null && trimmedLine.StartsWith("\"") && trimmedLine.Contains(" "))
            {
                var column = ParseColumnDefinition(trimmedLine);
                if (column != null)
                {
                    currentTable.Columns.Add(column);
                }
            }
        }

        return tables;
    }

    public static string Singularize(string plural)
    {
        if (string.IsNullOrWhiteSpace(plural))
            return plural;

        plural = plural.Trim();

        if (plural.EndsWith("ies", StringComparison.OrdinalIgnoreCase) && plural.Length > 3)
        {
            // e.g. "companies" => "company"
            return plural.Substring(0, plural.Length - 3) + "y";
        }
        else if (plural.EndsWith("es", StringComparison.OrdinalIgnoreCase) && plural.Length > 2)
        {
            // e.g. "boxes" => "box", "wishes" => "wish"
            return plural.Substring(0, plural.Length - 2);
        }
        else if (plural.EndsWith("s", StringComparison.OrdinalIgnoreCase) && plural.Length > 1)
        {
            // e.g. "books" => "book"
            return plural.Substring(0, plural.Length - 1);
        }

        // Return as-is if it doesn't match any rule
        return plural;
    }

    private string ExtractTableName(string line)
    {
        var match = Regex.Match(line, @"Table\s+""([^""]+)""", RegexOptions.IgnoreCase);
        return (match.Success ? match.Groups[1].Value : "UnknownTable").Singularize();
    }

    private TableColumn? ParseColumnDefinition(string line)
    {
        // Example: "id" INT [pk, increment]
        // Example: "title" NVARCHAR(255) [not null]

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            return null;

        var column = new TableColumn();

        // Extract column name (remove quotes)
        column.Name = parts[0].Trim('"');

        // Extract data type
        column.DataType = parts[1];

        // Extract max length if present
        var lengthMatch = Regex.Match(column.DataType, @"\((\d+)\)");
        if (lengthMatch.Success)
        {
            column.MaxLength = int.Parse(lengthMatch.Groups[1].Value);
            column.DataType = Regex.Replace(column.DataType, @"\(\d+\)", "");
        }

        // Parse attributes in brackets
        var attributesMatch = Regex.Match(line, @"\[([^\]]+)\]");
        if (attributesMatch.Success)
        {
            var attributes = attributesMatch.Groups[1].Value.Split(',');
            foreach (var attr in attributes)
            {
                var trimmedAttr = attr.Trim().ToLower();
                switch (trimmedAttr)
                {
                    case "pk":
                    case "primary key":
                        column.IsPrimaryKey = true;
                        column.IsNullable = false;
                        break;
                    case "increment":
                    case "auto_increment":
                        //column.IsAutoIncrement = true;
                        break;
                    case "not null":
                        column.IsNullable = false;
                        break;
                    case "null":
                        column.IsNullable = true;
                        break;
                    default:
                        if (trimmedAttr.StartsWith("default:"))
                        {
                            column.DefaultValue = trimmedAttr
                                .Substring(8)
                                .Trim(' ', '`', '"', '\'');
                        }
                        break;
                }
            }
        }
        else
        {
            // If no brackets, assume nullable unless it's a primary key
            column.IsNullable = !column.IsPrimaryKey;
        }

        return column;
    }
}
