namespace MetadataTool.Models;

/// <summary>
/// Property metadata
/// </summary>
public class PropertyMetadata
{
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsRequired { get; set; }
    public int? MaxLength { get; set; } = 255;
    public int? MinLength { get; set; } = 0;
}
