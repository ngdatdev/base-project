using Humanizer;

namespace ConfigTool.EntityGenerator.Model;

/// <summary>
/// Collection
/// </summary>
public class Collection
{
    public static List<CollectionItem> CollectionItems { get; set; } = new();

    public static void AddCollectionItem(string name)
    {
        CollectionItems.Add(
            new CollectionItem()
            {
                FileName = $"{name}.cs",
                Name = $"        public IEnumerable<{name}> {name.Pluralize()} {"{ get; set; }"}"
            }
        );
    }

    public static string FormatContent()
    {
        return string.Join("\r\n", CollectionItems.Select(x => x.Name)) + "\r\n    }" + "\r\n}";
    }

    public class CollectionItem
    {
        public string FileName { get; set; }
        public string Name { get; set; }
    }

    public static string Pluralize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        name = name.Trim();

        if (
            name.EndsWith("s", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("sh", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("ch", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("x", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("z", StringComparison.OrdinalIgnoreCase)
        )
        {
            return name + "es";
        }

        if (
            name.EndsWith("y", StringComparison.OrdinalIgnoreCase)
            && name.Length > 1
            && !IsVowel(name[name.Length - 2])
        )
        {
            return name.Substring(0, name.Length - 1) + "ies";
        }

        return name + "s";
    }

    private static bool IsVowel(char c)
    {
        return "aeiouAEIOU".IndexOf(c) >= 0;
    }
}
