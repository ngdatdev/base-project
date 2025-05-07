using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Config.MetadataTool.Generators;
using Config.MetadataTool.Parsers;
using MetadataTool.Templates;

namespace Config.MetadataTool;

public class MetadataToolMain
{
    public static string ENTITY_PATH = "Entities";
    public static string METADATA_PATH = "Metadata";
    public static string ASSEMBLY_NAME = "BaseApiReference";

    public static void Run()
    {
        var entities = FindFolderFromSln(ENTITY_PATH);
        var metadata = FindFolderFromSln(METADATA_PATH);

        if (!Directory.Exists(ENTITY_PATH))
        {
            Console.WriteLine("Entity path not found: " + entities);
        }

        if (!Directory.Exists(METADATA_PATH))
        {
            Console.WriteLine("Metadata path not found: " + metadata);
        }

        string[] files = Directory.GetFiles(entities);

        foreach (string file in files)
        {
            var parser = new EntityParser();
            var result = parser.Parse(file);

            //var currentAssemblyPath = Assembly.GetExecutingAssembly().Location;
            //var currentDirectory = Path.GetDirectoryName(currentAssemblyPath);

            //string projectDir = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", ".."));
            //string solutionDir = Path.GetFullPath(Path.Combine(projectDir, ".."));

            //string newAssemblyName = ASSEMBLY_NAME;
            //string newAssemblyDir = Path.Combine(solutionDir, newAssemblyName);
            //string metaDir = Path.Combine(newAssemblyDir, "MetaData");

            var generator = new EntityMetadataGenerator(
                outputNamespace: ASSEMBLY_NAME,
                defaultSchema: "dbo",
                templateEngine: new MetadataTemplateEngine(),
                entitiesDir: entities,
                metadataDir: METADATA_PATH
            );

            string csFileContent = generator.Generate(result);

            File.WriteAllText(Path.Combine(metadata, $"{result.Name}Metadata.cs"), csFileContent);

            Console.WriteLine($"Create file success: {result.Name}Metadata.cs");
        }
    }

    public static string FindFolderFromSln(string targetFolderName)
    {
        string current = AppDomain.CurrentDomain.BaseDirectory;

        while (current != null && !Directory.GetFiles(current, "*.sln").Any())
        {
            current = Directory.GetParent(current)?.FullName;
        }

        if (current == null)
        {
            throw new FileNotFoundException(
                "Solution (.sln) file not found in parent directories."
            );
        }

        string foundPath = Directory
            .GetDirectories(current, targetFolderName, SearchOption.AllDirectories)
            .FirstOrDefault();

        if (foundPath == null)
        {
            throw new DirectoryNotFoundException(
                $"Folder \"{targetFolderName}\" not found within solution directory."
            );
        }

        return foundPath;
    }
}
