using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BaseApiReference.Base;

/// <summary>
/// Base class for creating strongly-typed enumerations with rich behavior.
/// </summary>
/// <typeparam name="TEnum">The type that is inheriting from this class.</typeparam>
/// <typeparam name="TValue">The type of the enum value.</typeparam>
public abstract class SmartEnum<TEnum, TValue>
    where TEnum : SmartEnum<TEnum, TValue>
    where TValue : IEquatable<TValue>
{
    private static readonly ConcurrentDictionary<TValue, TEnum> _items = new();
    private static readonly ConcurrentDictionary<string, TEnum> _itemsByName =
        new(StringComparer.OrdinalIgnoreCase);

    public TValue Value { get; }

    public string Name { get; }

    protected SmartEnum(TValue value, string name)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Value = value;
        Name = name;
    }

    static SmartEnum()
    {
        foreach (
            var field in typeof(TEnum).GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly
            )
        )
        {
            var item = (TEnum)field.GetValue(null);
            Register(item);
        }

        if (!_items.Any())
        {
            throw new InvalidOperationException($"No static fields found in {typeof(TEnum).Name}");
        }
    }

    protected static TEnum Register(TEnum item)
    {
        _items[item.Value] = item;
        _itemsByName[item.Name] = item;
        return item;
    }

    public static IEnumerable<TEnum> GetAll() => _items.Values;

    public static List<TEnum> List() => _items.Values.ToList();

    public static TEnum FromValue(TValue value)
    {
        if (_items.TryGetValue(value, out var result))
        {
            return result;
        }

        throw new KeyNotFoundException($"No {typeof(TEnum).Name} with value {value} found.");
    }

    public static bool TryFromValue(TValue value, out TEnum result) =>
        _items.TryGetValue(value, out result);

    public static TEnum FromName(string name)
    {
        if (_itemsByName.TryGetValue(name, out var result))
        {
            return result;
        }

        throw new KeyNotFoundException($"No {typeof(TEnum).Name} with name {name} found.");
    }

    public static bool TryFromName(string name, out TEnum result) =>
        _itemsByName.TryGetValue(name, out result);

    public static bool Contains(TValue value) => _items.ContainsKey(value);

    public override string ToString() => Name;

    public override bool Equals(object obj)
    {
        if (obj is SmartEnum<TEnum, TValue> other)
        {
            return Value.Equals(other.Value);
        }

        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(SmartEnum<TEnum, TValue> left, SmartEnum<TEnum, TValue> right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(SmartEnum<TEnum, TValue> left, SmartEnum<TEnum, TValue> right)
    {
        return !(left == right);
    }
}
