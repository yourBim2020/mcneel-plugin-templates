namespace Scaffolder;

/// <summary>
/// Manages template discovery and paths.
/// </summary>
public static class TemplateManager
{
    /// <summary>
    /// Gets the root directory of the repository.
    /// </summary>
    public static string GetRepoRoot()
    {
        // Walk up from executable location to find repo root
        var dir = AppContext.BaseDirectory;

        while (!string.IsNullOrEmpty(dir))
        {
            if (Directory.Exists(Path.Combine(dir, "templates")) &&
                Directory.Exists(Path.Combine(dir, "specs")))
            {
                return dir;
            }

            var parent = Directory.GetParent(dir);
            if (parent == null) break;
            dir = parent.FullName;
        }

        // Fallback: check current directory and parents
        dir = Directory.GetCurrentDirectory();
        while (!string.IsNullOrEmpty(dir))
        {
            if (Directory.Exists(Path.Combine(dir, "templates")) &&
                Directory.Exists(Path.Combine(dir, "specs")))
            {
                return dir;
            }

            var parent = Directory.GetParent(dir);
            if (parent == null) break;
            dir = parent.FullName;
        }

        throw new InvalidOperationException(
            "Could not find repository root. Ensure you're running from within the mcneel-plugin-templates directory.");
    }

    /// <summary>
    /// Gets the templates directory path.
    /// </summary>
    public static string GetTemplatesDirectory()
    {
        return Path.Combine(GetRepoRoot(), "templates");
    }

    /// <summary>
    /// Gets the specs directory path.
    /// </summary>
    public static string GetSpecsDirectory()
    {
        return Path.Combine(GetRepoRoot(), "specs");
    }

    /// <summary>
    /// Gets the path to a specific template.
    /// </summary>
    public static string GetTemplatePath(string templateName)
    {
        var path = Path.Combine(GetTemplatesDirectory(), templateName);
        if (!Directory.Exists(path))
        {
            throw new ArgumentException($"Template not found: {templateName}");
        }
        return path;
    }

    /// <summary>
    /// Gets the path to a template's spec file.
    /// </summary>
    public static string? GetSpecPath(string templateName)
    {
        var path = Path.Combine(GetSpecsDirectory(), $"{templateName}.yaml");
        return File.Exists(path) ? path : null;
    }

    /// <summary>
    /// Lists all available templates.
    /// </summary>
    public static List<string> GetAvailableTemplates()
    {
        try
        {
            var templatesDir = GetTemplatesDirectory();
            return Directory.GetDirectories(templatesDir)
                .Select(Path.GetFileName)
                .Where(name => !string.IsNullOrEmpty(name))
                .OrderBy(name => name)
                .ToList()!;
        }
        catch
        {
            return new List<string>();
        }
    }
}
