using System.IO;

namespace Common.Helper;

public static class FileUtil
{
    public static string GetFileExtension(string fileName) =>
        Path.GetExtension(fileName)?.ToLower();

    public static string ReadText(string path) => File.ReadAllText(path);

    public static void WriteText(string path, string content) => File.WriteAllText(path, content);

    public static byte[] ReadBytes(string path) => File.ReadAllBytes(path);

    public static void SaveTempFile(byte[] content, string path) =>
        File.WriteAllBytes(path, content);
}
