using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetadataTool.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Config.MetadataTool.Parsers;

/// <summary>
/// Parser C#
/// </summary>
public class EntityParser : ICodeParser<EntityMetadata>
{
    public EntityMetadata Parse(string filePath)
    {
        string sourceCode = File.ReadAllText(filePath);

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

        string namespaceName = "";
        var namespaceDeclaration = root.DescendantNodes()
            .OfType<NamespaceDeclarationSyntax>()
            .FirstOrDefault();

        if (namespaceDeclaration != null)
        {
            namespaceName = namespaceDeclaration.Name.ToString();
        }
        else
        {
            var fileScopedNamespace = root.DescendantNodes()
                .OfType<FileScopedNamespaceDeclarationSyntax>()
                .FirstOrDefault();

            if (fileScopedNamespace != null)
            {
                namespaceName = fileScopedNamespace.Name.ToString();
            }
        }

        var classDeclaration = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault();
        if (classDeclaration == null)
        {
            throw new Exception("Can't find class declaration in file");
        }

        string className = classDeclaration.Identifier.ValueText;

        var implementedInterfaces = new List<string>();
        if (classDeclaration.BaseList != null)
        {
            foreach (var baseType in classDeclaration.BaseList.Types)
            {
                string typeName = baseType.ToString();
                if (typeName.StartsWith("I") && char.IsUpper(typeName[1]))
                {
                    implementedInterfaces.Add(typeName);
                }
            }
        }

        var entityMetadata = new EntityMetadata
        {
            Name = className,
            Namespace = namespaceName,
            ImplementedInterfaces = implementedInterfaces,
            FilePath = filePath,
            Properties = new List<PropertyMetadata>()
        };

        var properties = classDeclaration
            .DescendantNodes()
            .OfType<PropertyDeclarationSyntax>()
            .ToList();

        foreach (var property in properties)
        {
            string propertyName = property.Identifier.ValueText;
            string typeName = property.Type.ToString();

            var propertyMetadata = new PropertyMetadata
            {
                Name = propertyName,
                Type = typeName,
                IsRequired = false
            };

            var propertyAttributes = property
                .AttributeLists.SelectMany(al => al.Attributes)
                .ToList();

            foreach (var attr in propertyAttributes)
            {
                string attrName = attr.Name.ToString();

                if (attrName == "Required")
                {
                    propertyMetadata.IsRequired = true;
                }
            }

            bool isReferenceType =
                !typeName.EndsWith("?") && !IsValueType(typeName) && typeName != "string";

            if (isReferenceType && propertyMetadata.Type == typeName)
            {
                propertyMetadata.IsRequired = true;
            }

            entityMetadata.Properties.Add(propertyMetadata);
        }

        return entityMetadata;
    }

    private static bool IsValueType(string typeName)
    {
        var valueTypes = new[]
        {
            "bool",
            "byte",
            "sbyte",
            "char",
            "decimal",
            "double",
            "float",
            "int",
            "uint",
            "long",
            "ulong",
            "short",
            "ushort",
            "DateTime",
            "DateTimeOffset",
            "TimeSpan",
            "Guid"
        };

        return valueTypes.Contains(typeName);
    }
}
