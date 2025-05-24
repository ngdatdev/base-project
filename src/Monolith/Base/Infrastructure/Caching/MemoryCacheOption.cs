namespace Infrastructure.Caching;

/// <summary>
/// Memory caching options
/// </summary>
public class MemoryCacheOption : CacheOption
{
    public long? SizeLimit { get; set; }
    public double CompactionPercentage { get; set; } = 0.05;
}
