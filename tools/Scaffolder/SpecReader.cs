using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Scaffolder;

/// <summary>
/// Reads and parses OpenSpec YAML files.
/// </summary>
public static class SpecReader
{
    /// <summary>
    /// Loads a spec file for a given template.
    /// </summary>
    public static PlatformSpec? LoadSpec(string templateName)
    {
        var specPath = TemplateManager.GetSpecPath(templateName);
        if (specPath == null || !File.Exists(specPath))
        {
            return null;
        }

        var yaml = File.ReadAllText(specPath);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        return deserializer.Deserialize<PlatformSpec>(yaml);
    }

    /// <summary>
    /// Prints spec information to the console.
    /// </summary>
    public static void PrintSpecInfo(PlatformSpec spec)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  {spec.Metadata?.DisplayName ?? spec.Metadata?.Platform ?? "Unknown"}");
        Console.ResetColor();
        Console.WriteLine();

        if (!string.IsNullOrEmpty(spec.Metadata?.Description))
        {
            Console.WriteLine($"  {spec.Metadata.Description.Trim()}");
            Console.WriteLine();
        }

        if (spec.Runtime != null)
        {
            Console.WriteLine("  Runtime:");
            Console.WriteLine($"    Host: {spec.Runtime.HostApplication}");
            Console.WriteLine($"    Framework: {spec.Runtime.Framework}");
            Console.WriteLine();
        }

        if (spec.Dependencies?.Required != null && spec.Dependencies.Required.Count > 0)
        {
            Console.WriteLine("  Dependencies:");
            foreach (var dep in spec.Dependencies.Required)
            {
                Console.WriteLine($"    - {dep.Name} {dep.Version}");
            }
            Console.WriteLine();
        }

        if (spec.Conventions != null)
        {
            Console.WriteLine("  Conventions:");
            if (spec.Conventions.Namespaces != null)
            {
                Console.WriteLine($"    Namespace: {spec.Conventions.Namespaces.Pattern}");
            }
            Console.WriteLine();
        }
    }
}

#region Spec Models

public class PlatformSpec
{
    public SpecMetadata? Metadata { get; set; }
    public RuntimeSpec? Runtime { get; set; }
    public DependenciesSpec? Dependencies { get; set; }
    public ProjectStructureSpec? ProjectStructure { get; set; }
    public ConventionsSpec? Conventions { get; set; }
    public List<PitfallSpec>? Pitfalls { get; set; }
}

public class SpecMetadata
{
    public string? Platform { get; set; }
    public string? DisplayName { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
}

public class RuntimeSpec
{
    public string? HostApplication { get; set; }
    public string? Framework { get; set; }
    public string? ClrVersion { get; set; }
    public List<string>? Architecture { get; set; }
    public List<string>? Os { get; set; }
}

public class DependenciesSpec
{
    public List<DependencySpec>? Required { get; set; }
    public List<DependencySpec>? Optional { get; set; }
}

public class DependencySpec
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Source { get; set; }
    public string? Notes { get; set; }
}

public class ProjectStructureSpec
{
    public string? Type { get; set; }
    public string? TargetFramework { get; set; }
    public List<FileRequirement>? RequiredFiles { get; set; }
    public List<FileRequirement>? RecommendedStructure { get; set; }
}

public class FileRequirement
{
    public string? Path { get; set; }
    public string? Description { get; set; }
    public string? MustImplement { get; set; }
}

public class ConventionsSpec
{
    public NamingPattern? Namespaces { get; set; }
    public NamingPattern? Components { get; set; }
    public NamingPattern? Parameters { get; set; }
    public GuidConventions? Guids { get; set; }
}

public class NamingPattern
{
    public string? Pattern { get; set; }
    public string? Example { get; set; }
    public string? ClassSuffix { get; set; }
}

public class GuidConventions
{
    public string? Format { get; set; }
    public string? Example { get; set; }
    public List<string>? Rules { get; set; }
}

public class PitfallSpec
{
    public string? Issue { get; set; }
    public string? Solution { get; set; }
}

#endregion
