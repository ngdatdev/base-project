using System.Text;
using ConfigTool.EntityGenerator.Generator;
using ConfigTool.EntityGenerator.Model;
using ConfigTool.EntityGenerator.Parser;

namespace ConfigTool.EntityGenerator
{
    /// <summary>
    /// Main entry point for entity generation
    /// </summary>
    public class EntityGeneratorMain
    {
        // Global configuration variables
        private static string InputPath =
            @"D:\project\Monolith\src\Monolith\Base\ConfigTool\EntityGenerator\Input\schema.txt";
        private static string OutputDirectory =
            @"D:\project\Monolith\src\Monolith\Base\ConfigTool\EntityGenerator\OutputTest\";

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                ASP.NET Entity Generator Tool                ║");
            Console.WriteLine("║              Parse Table Schema & Generate Entities         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                ValidatePaths();

                // Read input file
                string input = File.ReadAllText(InputPath);
                Console.WriteLine($"Successfully read file: {InputPath}");

                // Parse tables
                var parser = new TableSchemaParser();
                var tables = parser.ParseFromText(input);
                Console.WriteLine($"Parsed {tables.Count} table(s)");

                // Generate code
                var generator = new EntityGenerators();

                foreach (var table in tables)
                {
                    Console.WriteLine($"\n Generating for table: {table.TableName}");

                    // Generate Entity
                    string entityCode = generator.GenerateEntity(table);
                    string entityPath = Path.Combine(
                        OutputDirectory,
                        "Entities",
                        $"{ToPascalCase(table.TableName)}.cs"
                    );
                    Directory.CreateDirectory(Path.GetDirectoryName(entityPath)!);
                    File.WriteAllText(entityPath, entityCode);
                    Console.WriteLine($" Entity: {entityPath}");

                    // Generate Configuration
                    string configCode = generator.GenerateEntityConfiguration(table);
                    string configPath = Path.Combine(
                        OutputDirectory,
                        "Configurations",
                        $"{ToPascalCase(table.TableName)}Configuration.cs"
                    );
                    Directory.CreateDirectory(Path.GetDirectoryName(configPath)!);
                    File.WriteAllText(configPath, configCode);
                    Console.WriteLine($" Configuration: {configPath}");
                }

                string entityDir = Path.Combine(OutputDirectory, "Entities");
                Collection.CollectionItems.ForEach(ci =>
                    AppendToFile(Path.Combine(entityDir, ci.FileName), Collection.FormatContent())
                );
                // Generate DbContext template
                // GenerateDbContextTemplate(tables, OutputDirectory);

                Console.WriteLine(
                    $"\n🎉 Completed! Generated {tables.Count * 2 + 1} file(s) at: {OutputDirectory}"
                );
                Console.WriteLine("\n Notes:");
                Console.WriteLine("   - Adjust namespaces to match your project");
                Console.WriteLine("   - Add navigation properties for foreign keys");
                Console.WriteLine("   - Configure relationships in DbContext");
                Console.WriteLine("   - Add necessary indexes and constraints");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        public static void AppendToFile(string pathInput, string contentToAppend)
        {
            if (string.IsNullOrWhiteSpace(pathInput))
            {
                Console.WriteLine("❌ Invalid file path.");
                return;
            }

            try
            {
                string[] existingLines = File.Exists(pathInput)
                    ? File.ReadAllLines(pathInput)
                    : Array.Empty<string>();

                var allLines = existingLines.ToList();

                allLines.ForEach(line =>
                {
                    if (line.Contains(contentToAppend))
                    {
                        return;
                    }
                });

                allLines.Add(contentToAppend);

                File.WriteAllLines(pathInput, allLines);

                Console.WriteLine($"✅ Đã đọc và ghi thêm nội dung vào file: {pathInput}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi đọc/ghi file: {ex.Message}");
            }
        }

        private static void ValidatePaths()
        {
            if (string.IsNullOrEmpty(InputPath) || !File.Exists(InputPath))
            {
                throw new FileNotFoundException(
                    "The schema file does not exist or the path is invalid."
                );
            }

            if (string.IsNullOrEmpty(OutputDirectory))
            {
                OutputDirectory = "./Generated";
            }
        }

        private static void GenerateDbContextTemplate(List<TableSchema> tables, string outputDir)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using YourProject.Entities;");
            sb.AppendLine("using YourProject.Data.Configurations;");
            sb.AppendLine();
            sb.AppendLine("namespace YourProject.Data");
            sb.AppendLine("{");
            sb.AppendLine("    public class YourDbContext : DbContext");
            sb.AppendLine("    {");
            sb.AppendLine(
                "        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)"
            );
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();

            // DbSets
            sb.AppendLine("        // DbSets");
            foreach (var table in tables)
            {
                var className = ToPascalCase(table.TableName);
                var pluralName = className.EndsWith("s") ? className : className + "s";
                sb.AppendLine($"        public DbSet<{className}> {pluralName} {{ get; set; }}");
            }

            sb.AppendLine();
            sb.AppendLine(
                "        protected override void OnModelCreating(ModelBuilder modelBuilder)"
            );
            sb.AppendLine("        {");
            sb.AppendLine("            base.OnModelCreating(modelBuilder);");
            sb.AppendLine();
            sb.AppendLine("            // Apply configurations");
            foreach (var table in tables)
            {
                var className = ToPascalCase(table.TableName);
                sb.AppendLine(
                    $"            modelBuilder.ApplyConfiguration(new {className}Configuration());"
                );
            }
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string dbContextPath = Path.Combine(outputDir, "YourDbContext.cs");
            File.WriteAllText(dbContextPath, sb.ToString());
            Console.WriteLine($"   ✅ DbContext: {dbContextPath}");
        }

        private static string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Contains('_'))
            {
                var parts = input.Split('_', StringSplitOptions.RemoveEmptyEntries);
                return string.Join(
                    "",
                    parts.Select(part => char.ToUpper(part[0]) + part.Substring(1).ToLower())
                );
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
