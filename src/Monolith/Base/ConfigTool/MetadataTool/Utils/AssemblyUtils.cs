using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;

namespace ConfigTool.MetadataTool.Utils
{
    /// <summary>
    /// Provides utility methods for working with .NET assemblies
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// Loads an assembly from a file path
        /// </summary>
        /// <param name="path">Path to the assembly file</param>
        /// <returns>The loaded assembly</returns>
        public static Assembly LoadAssembly(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Assembly file not found: {path}");

            return AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(path));
        }

        /// <summary>
        /// Gets the location of the executing assembly
        /// </summary>
        /// <returns>Full path of the executing assembly</returns>
        public static string GetExecutingAssemblyLocation()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        /// <summary>
        /// Gets all types in an assembly
        /// </summary>
        /// <param name="assembly">The assembly to inspect</param>
        /// <returns>A collection of types</returns>
        public static IEnumerable<Type> GetAllTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null);
            }
        }

        /// <summary>
        /// Gets all public classes in an assembly
        /// </summary>
        /// <param name="assembly">The assembly to inspect</param>
        /// <returns>A collection of public classes</returns>
        public static IEnumerable<Type> GetPublicClasses(Assembly assembly)
        {
            return GetAllTypes(assembly).Where(t => t.IsClass && t.IsPublic);
        }

        /// <summary>
        /// Gets all classes in an assembly that implement a specific interface
        /// </summary>
        /// <param name="assembly">The assembly to inspect</param>
        /// <param name="interfaceType">The interface type to check for</param>
        /// <returns>A collection of types that implement the interface</returns>
        public static IEnumerable<Type> GetClassesImplementingInterface(
            Assembly assembly,
            Type interfaceType
        )
        {
            if (!interfaceType.IsInterface)
                throw new ArgumentException(
                    "The provided type must be an interface",
                    nameof(interfaceType)
                );

            return GetAllTypes(assembly)
                .Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t));
        }

        /// <summary>
        /// Gets all classes in an assembly that inherit from a specific base class
        /// </summary>
        /// <param name="assembly">The assembly to inspect</param>
        /// <param name="baseType">The base type to check for</param>
        /// <returns>A collection of types that inherit from the base class</returns>
        public static IEnumerable<Type> GetClassesInheritingFromClass(
            Assembly assembly,
            Type baseType
        )
        {
            if (!baseType.IsClass)
                throw new ArgumentException("The provided type must be a class", nameof(baseType));

            return GetAllTypes(assembly)
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));
        }

        /// <summary>
        /// Gets all classes with a specific attribute
        /// </summary>
        /// <param name="assembly">The assembly to inspect</param>
        /// <param name="attributeType">The attribute type to check for</param>
        /// <returns>A collection of types with the attribute</returns>
        public static IEnumerable<Type> GetClassesWithAttribute(
            Assembly assembly,
            Type attributeType
        )
        {
            if (!attributeType.IsSubclassOf(typeof(Attribute)))
                throw new ArgumentException(
                    "The provided type must be an attribute",
                    nameof(attributeType)
                );

            return GetAllTypes(assembly)
                .Where(t => t.IsClass && t.GetCustomAttributes(attributeType, true).Any());
        }

        /// <summary>
        /// Gets all public methods in a class
        /// </summary>
        /// <param name="type">The type to inspect</param>
        /// <returns>A collection of public methods</returns>
        public static IEnumerable<MethodInfo> GetPublicMethods(Type type)
        {
            return type.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
            );
        }

        /// <summary>
        /// Gets all methods with a specific attribute
        /// </summary>
        /// <param name="type">The type to inspect</param>
        /// <param name="attributeType">The attribute type to check for</param>
        /// <returns>A collection of methods with the attribute</returns>
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type type, Type attributeType)
        {
            if (!attributeType.IsSubclassOf(typeof(Attribute)))
                throw new ArgumentException(
                    "The provided type must be an attribute",
                    nameof(attributeType)
                );

            return type.GetMethods(
                    BindingFlags.Public
                        | BindingFlags.NonPublic
                        | BindingFlags.Instance
                        | BindingFlags.Static
                )
                .Where(m => m.GetCustomAttributes(attributeType, true).Any());
        }

        /// <summary>
        /// Creates an instance of a class
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <returns>An instance of the type</returns>
        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Creates an instance of a class with constructor parameters
        /// </summary>
        /// <param name="type">The type to instantiate</param>
        /// <param name="parameters">Constructor parameters</param>
        /// <returns>An instance of the type</returns>
        public static object CreateInstance(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }

        /// <summary>
        /// Invokes a method on an object
        /// </summary>
        /// <param name="obj">The object instance</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="parameters">Method parameters</param>
        /// <returns>The return value of the method</returns>
        public static object InvokeMethod(object obj, string methodName, params object[] parameters)
        {
            Type type = obj.GetType();
            MethodInfo method = type.GetMethod(
                methodName,
                BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.Static
            );

            if (method == null)
                throw new ArgumentException($"Method '{methodName}' not found on type {type.Name}");

            return method.Invoke(obj, parameters);
        }

        /// <summary>
        /// Gets the version of an assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The assembly version</returns>
        public static Version GetAssemblyVersion(Assembly assembly)
        {
            return assembly.GetName().Version;
        }

        /// <summary>
        /// Gets information about the assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>Dictionary with assembly information</returns>
        public static Dictionary<string, string> GetAssemblyInfo(Assembly assembly)
        {
            var info = new Dictionary<string, string>
            {
                { "Name", assembly.GetName().Name },
                { "FullName", assembly.FullName },
                { "Version", assembly.GetName().Version.ToString() },
                { "Location", assembly.Location },
                { "CodeBase", assembly.CodeBase }
            };

            // Get assembly attributes
            var attributes = assembly.GetCustomAttributes(true);

            foreach (var attribute in attributes)
            {
                if (attribute is AssemblyDescriptionAttribute descAttr)
                    info.Add("Description", descAttr.Description);
                else if (attribute is AssemblyCopyrightAttribute copyAttr)
                    info.Add("Copyright", copyAttr.Copyright);
                else if (attribute is AssemblyCompanyAttribute companyAttr)
                    info.Add("Company", companyAttr.Company);
                else if (attribute is AssemblyProductAttribute productAttr)
                    info.Add("Product", productAttr.Product);
            }

            return info;
        }

        /// <summary>
        /// Gets a strong name key for an assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The public key token as a string</returns>
        public static string GetPublicKeyToken(Assembly assembly)
        {
            byte[] token = assembly.GetName().GetPublicKeyToken();
            if (token == null || token.Length == 0)
                return "Not signed";

            return BitConverter.ToString(token).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Checks if an assembly is signed with a strong name
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>True if the assembly is signed</returns>
        public static bool IsAssemblySigned(Assembly assembly)
        {
            byte[] token = assembly.GetName().GetPublicKeyToken();
            return token != null && token.Length > 0;
        }

        /// <summary>
        /// Gets all referenced assemblies
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>Collection of referenced assemblies</returns>
        public static IEnumerable<AssemblyName> GetReferencedAssemblies(Assembly assembly)
        {
            return assembly.GetReferencedAssemblies();
        }

        /// <summary>
        /// Calculates a hash of the assembly file
        /// </summary>
        /// <param name="assemblyPath">Path to the assembly file</param>
        /// <returns>SHA256 hash of the assembly file</returns>
        public static string CalculateAssemblyFileHash(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
                throw new FileNotFoundException($"Assembly file not found: {assemblyPath}");

            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(assemblyPath))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
