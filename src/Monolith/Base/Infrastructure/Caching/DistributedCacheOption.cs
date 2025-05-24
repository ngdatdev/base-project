namespace Infrastructure.Caching;

/// <summary>
/// Distributed caching options
/// </summary>
public class DistributedCacheOption : CacheOption
{
    public string ConnectionString { get; set; }
    public string InstanceName { get; set; } = "MyApp";
    public int Database { get; set; } = 0;
}
