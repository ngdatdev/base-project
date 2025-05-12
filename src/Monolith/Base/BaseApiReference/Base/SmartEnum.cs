using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BaseApiReference.Base;

public abstract class SmartEnum<TEnum, TValue>
    where TEnum : SmartEnum<TEnum, TValue>
{
    private static readonly ConcurrentDictionary<TValue, TEnum> _items = new();

    public TValue Value { get; }
    public string Name { get; }

    protected SmartEnum(TValue value, string name)
    {
        Value = value;
        Name = name;

        _items.TryAdd(value, (TEnum)this);
    }

    public static IEnumerable<TEnum> GetAll() => _items.Values;

    public static TEnum FromValue(TValue value) => _items[value];

    public static bool TryFromValue(TValue value, out TEnum result) =>
        _items.TryGetValue(value, out result);

    public override string ToString() => Name;
}
