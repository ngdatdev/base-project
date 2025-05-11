using System.Text.Json;

namespace Common.Helper;

/// <summary>
/// Json Util.
/// </summary>
public class JsonUtil
{
    public static string ToJson(object obj) => JsonSerializer.Serialize(obj);

    public static T FromJson<T>(string json) => JsonSerializer.Deserialize<T>(json);

    public static bool IsValidJson(string json)
    {
        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
