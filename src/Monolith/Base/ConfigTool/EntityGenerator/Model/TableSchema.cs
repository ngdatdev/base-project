namespace ConfigTool.EntityGenerator.Model;

/// <summary>
/// Table Schema
/// </summary>
public class TableSchema
{
    public string TableName { get; set; } = string.Empty;
    public List<TableColumn> Columns { get; set; } = new List<TableColumn>();
}
