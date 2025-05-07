using System.Collections.Generic;

namespace ConfigTool.MetadataTool.Models;

public class EntityMetadata
{
    public string Name { get; set; }
    public string Namespace { get; set; }
    public List<string> ImplementedInterfaces { get; set; }
    public string FilePath { get; set; }
    public List<PropertyMetadata> Properties { get; set; }
}
