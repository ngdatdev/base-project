namespace BaseApiReference.Base;

/// <summary>
/// Represent the base entity abstract that all
/// entity that is created must inherit from
/// this abstract.
/// </summary>
public abstract class BaseEntity<T>
{
    public T Id { get; set; }
}
