using System.Text;
using ConfigTool.EntityGenerator.Model;

namespace ConfigTool.EntityGenerator.Generator;

/// <summary>
/// Class to generate Entity classes
/// </summary>
public class EntityGenerators
{
    public string GenerateEntity(TableSchema table)
    {
        var className = ToPascalCase(table.TableName);

        var sb = new StringBuilder();

        sb.AppendLine("using System;");
        sb.AppendLine("using System.ComponentModel.DataAnnotations;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine($"namespace YourProject.Entities");
        sb.AppendLine("{");
        sb.AppendLine($"    /// <summary>");
        sb.AppendLine($"    /// Entity class for {table.TableName} table");
        sb.AppendLine($"    /// </summary>");
        sb.AppendLine($"    public class {className}");
        sb.AppendLine("    {");

        foreach (var column in table.Columns)
        {
            GenerateProperty(sb, column);
        }

        // Generate navigation properties placeholder
        sb.AppendLine();
        sb.AppendLine("        // Navigation Properties");
        foreach (var column in table.Columns)
        {
            var propertyName = ToPascalCase(column.Name);
            if (column.Name.Contains("_id"))
            {
                sb.AppendLine(
                    $"        public {propertyName.Split("Id")[0]} {propertyName.Split("Id")[0]} {{ get; set; }}"
                );
            }
        }
        foreach (var column in table.Columns)
        {
            if (column.Name.Contains("_id"))
            {
                var propertyName = ToPascalCase(column.Name);
                if (!Collection.FormatContent().Contains(propertyName.Split("Id")[0]))
                {
                    Collection.AddCollectionItem(propertyName.Split("Id")[0]);
                }
            }
        }

        //sb.AppendLine("    }");
        //sb.AppendLine("}");

        return sb.ToString();
    }

    private void GenerateProperty(StringBuilder sb, TableColumn column)
    {
        var propertyName = ToPascalCase(column.Name);
        var csharpType = MapSqlTypeToCSharp(column.DataType, column.IsNullable);

        sb.AppendLine();

        // Add attributes
        if (column.IsPrimaryKey)
        {
            sb.AppendLine("        [Key]");
        }

        if (!column.IsNullable && !column.IsPrimaryKey)
        {
            sb.AppendLine("        [Required]");
        }

        if (
            column.MaxLength.HasValue
            && (
                column.DataType.ToUpper().Contains("VARCHAR")
                || column.DataType.ToUpper().Contains("CHAR")
            )
        )
        {
            sb.AppendLine($"        [MaxLength({column.MaxLength})]");
        }

        // Generate property
        sb.AppendLine(
            $"        public {csharpType} {propertyName} {{ get; set; }}{GetDefaultValue(column)}"
        );
    }

    private string GetDefaultValue(TableColumn column)
    {
        if (column.DefaultValue != null)
        {
            var csharpType = MapSqlTypeToCSharp(column.DataType, column.IsNullable);

            // Handle specific default values
            if (
                column.DefaultValue.ToUpper() == "GETDATE()"
                || column.DefaultValue.ToUpper() == "NOW()"
            )
            {
                return " = DateTime.Now;";
            }

            if (csharpType == "bool" || csharpType == "bool?")
            {
                return column.DefaultValue == "1" ? " = true;" : " = false;";
            }

            if (csharpType == "string")
            {
                return $" = \"{column.DefaultValue}\";";
            }

            if (IsNumericType(csharpType))
            {
                return $" = {column.DefaultValue};";
            }
        }

        // Default initialization for non-nullable strings
        if (!column.IsNullable && MapSqlTypeToCSharp(column.DataType, false) == "string")
        {
            return " = string.Empty;";
        }

        return string.Empty;
    }

    public string GenerateEntityConfiguration(TableSchema table)
    {
        var className = ToPascalCase(table.TableName);
        var sb = new StringBuilder();

        sb.AppendLine("using Microsoft.EntityFrameworkCore;");
        sb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
        sb.AppendLine($"using YourProject.Entities;");
        sb.AppendLine();
        sb.AppendLine($"namespace YourProject.Data.Configurations");
        sb.AppendLine("{");
        sb.AppendLine($"    /// <summary>");
        sb.AppendLine($"    /// Entity Configuration for {className}");
        sb.AppendLine($"    /// </summary>");
        sb.AppendLine(
            $"    public class {className}Configuration : IEntityTypeConfiguration<{className}>"
        );
        sb.AppendLine("    {");
        sb.AppendLine($"        public void Configure(EntityTypeBuilder<{className}> builder)");
        sb.AppendLine("        {");
        sb.AppendLine($"            // Table name");
        sb.AppendLine($"            builder.ToTable(\"{className}\");");
        sb.AppendLine();

        // Primary key
        var pkColumns = table.Columns.Where(c => c.IsPrimaryKey).ToList();
        if (pkColumns.Count == 1)
        {
            sb.AppendLine($"            // Primary Key");
            sb.AppendLine($"            builder.HasKey(e => e.{ToPascalCase(pkColumns[0].Name)});");
        }
        else if (pkColumns.Count > 1)
        {
            var pkProps = string.Join(", ", pkColumns.Select(c => $"e.{ToPascalCase(c.Name)}"));
            sb.AppendLine($"            // Composite Primary Key");
            sb.AppendLine($"            builder.HasKey(e => new {{ {pkProps} }});");
        }

        sb.AppendLine();
        sb.AppendLine($"            // Properties Configuration");

        foreach (var column in table.Columns)
        {
            GeneratePropertyConfiguration(sb, column, className);
        }

        // Indexes
        sb.AppendLine();
        sb.AppendLine($"            // Indexes");
        sb.AppendLine($"            // TODO: Add indexes based on your requirements");

        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private void GeneratePropertyConfiguration(
        StringBuilder sb,
        TableColumn column,
        string className
    )
    {
        var propertyName = ToPascalCase(column.Name);
        var entityVar = className.ToLower()[0];

        sb.AppendLine();
        sb.AppendLine($"            builder.Property({entityVar} => {entityVar}.{propertyName})");
        sb.AppendLine($"                .HasColumnName(\"{propertyName}\")");

        // Column type
        if (!string.IsNullOrEmpty(column.DataType))
        {
            if (column.MaxLength.HasValue)
            {
                sb.AppendLine(
                    $"                .HasColumnType(\"{column.DataType.ToUpper()}({column.MaxLength})\")"
                );
            }
            else
            {
                sb.AppendLine($"                .HasColumnType(\"{column.DataType.ToUpper()}\")");
            }
        }

        // Required/Optional
        sb.AppendLine($"                .IsRequired({(!column.IsNullable).ToString().ToLower()})");

        // Auto increment
        if (column.IsAutoIncrement)
        {
            sb.AppendLine($"                .ValueGeneratedOnAdd()");
        }

        // Default value
        if (column.DefaultValue != null)
        {
            if (
                column.DefaultValue.ToUpper() == "GETDATE()"
                || column.DefaultValue.ToUpper() == "NOW()"
            )
            {
                sb.AppendLine(
                    $"                .HasDefaultValueSql(\"{column.DefaultValue.ToUpper()}\")"
                );
            }
            else
            {
                sb.AppendLine(
                    $"                .HasDefaultValue({FormatDefaultValueForConfiguration(column)})"
                );
            }
        }

        sb.ToString().Trim();
        sb.Append($";");
    }

    private string FormatDefaultValueForConfiguration(TableColumn column)
    {
        var csharpType = MapSqlTypeToCSharp(column.DataType, column.IsNullable);

        if (csharpType == "bool" || csharpType == "bool?")
        {
            return column.DefaultValue == "1" ? "true" : "false";
        }

        if (csharpType == "string")
        {
            return $"\"{column.DefaultValue}\"";
        }

        return column.DefaultValue ?? "null";
    }

    private string MapSqlTypeToCSharp(string sqlType, bool isNullable)
    {
        var baseType = sqlType.ToUpper() switch
        {
            "INT" or "INTEGER" => "int",
            "BIGINT" => "long",
            "SMALLINT" => "short",
            "TINYINT" => "byte",
            "BIT" => "bool",
            "DECIMAL" or "NUMERIC" or "MONEY" => "decimal",
            "FLOAT" or "REAL" => "float",
            "DOUBLE" => "double",
            "VARCHAR" or "NVARCHAR" or "CHAR" or "NCHAR" or "TEXT" or "NTEXT" => "string",
            "DATE" or "DATETIME" or "DATETIME2" or "TIMESTAMP" => "DateTime",
            "TIME" => "TimeSpan",
            "UNIQUEIDENTIFIER" or "GUID" => "Guid",
            "VARBINARY" or "BINARY" or "IMAGE" => "byte[]",
            _ => "object"
        };

        // String and byte[] are reference types, don't need nullable annotation
        if (baseType == "string" || baseType == "byte[]" || baseType == "object")
        {
            return isNullable ? $"{baseType}?" : baseType;
        }

        // Value types need nullable annotation when nullable
        return isNullable ? $"{baseType}?" : baseType;
    }

    private bool IsNumericType(string csharpType)
    {
        var numericTypes = new[]
        {
            "int",
            "long",
            "short",
            "byte",
            "decimal",
            "float",
            "double",
            "int?",
            "long?",
            "short?",
            "byte?",
            "decimal?",
            "float?",
            "double?"
        };
        return numericTypes.Contains(csharpType);
    }

    private string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Handle snake_case
        if (input.Contains('_'))
        {
            var parts = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(
                "",
                parts.Select(part => char.ToUpper(part[0]) + part.Substring(1).ToLower())
            );
        }

        // Handle single word
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }
}
