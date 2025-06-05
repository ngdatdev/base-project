using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigTool.EntityGenerator.Model;

/// <summary>
/// Table Column
/// </summary>
public class TableColumn
{
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public bool IsNullable { get; set; } = true;
    public bool IsPrimaryKey { get; set; } = false;
    public bool IsAutoIncrement { get; set; } = false;
    public string? DefaultValue { get; set; }
    public int? MaxLength { get; set; }
    public bool IsForeignKey { get; set; } = false;
    public string? ReferencedTable { get; set; }
}
