using System.Text.RegularExpressions;
using Spectre.Console;

namespace Scaffolder;

/// <summary>
/// Handles project scaffolding from templates.
/// </summary>
public partial class ProjectScaffolder
{
    // Placeholder definitions with prompts and defaults
    private static readonly Dictionary<string, PlaceholderDef> Placeholders = new()
    {
        ["$PluginName$"] = new("Plugin name (PascalCase)", null, ValidatePascalCase),
        ["$PluginDescription$"] = new("Plugin description", "A McNeel plugin"),
        ["$AuthorName$"] = new("Author name", Environment.UserName),
        ["$AuthorEmail$"] = new("Author email", ""),
        ["$CompanyName$"] = new("Company/Organization name", ""),
        ["$Version$"] = new("Initial version", "1.0.0"),
        ["$Year$"] = new(null, DateTime.Now.Year.ToString()),
        ["$PluginGuid$"] = new(null, () => Guid.NewGuid().ToString()),
        ["$AssemblyGuid$"] = new(null, () => Guid.NewGuid().ToString()),
        ["$ComponentGuid$"] = new(null, () => Guid.NewGuid().ToString()),
        ["$ComponentName$"] = new("Component display name", Default: null),
        ["$ComponentNickname$"] = new("Component nickname (1-3 chars)", Default: null),
        ["$ComponentDescription$"] = new("Component description", "A sample component"),
        ["$Category$"] = new("Grasshopper tab category", "Custom"),
        ["$Subcategory$"] = new("Subcategory within tab", "Util"),
        ["$RevitYear$"] = new("Target Revit version year", "2024"),
    };

    /// <summary>
    /// Creates a new project from a template.
    /// </summary>
    public async Task CreateProjectAsync(
        string templateName,
        string projectName,
        string outputDir,
        bool interactive)
    {
        // Validate project name
        if (!PascalCaseRegex().IsMatch(projectName))
        {
            throw new ArgumentException(
                "Project name must be PascalCase (e.g., MyPlugin, GeometryTools)");
        }

        // Verify template exists
        var templatePath = TemplateManager.GetTemplatePath(templateName);
        var projectPath = Path.Combine(outputDir, projectName);

        if (Directory.Exists(projectPath))
        {
            throw new InvalidOperationException($"Directory already exists: {projectPath}");
        }

        // Load spec for context (optional)
        var spec = SpecReader.LoadSpec(templateName);

        // Collect placeholder values
        AnsiConsole.MarkupLine($"\n[cyan]Scaffolding[/] [white]{templateName}[/] template as [green]{projectName}[/]");
        AnsiConsole.WriteLine(new string('─', 50));

        var values = CollectPlaceholderValues(projectName, interactive, spec);

        // Create project directory
        Directory.CreateDirectory(projectPath);

        AnsiConsole.MarkupLine("\n[cyan]Creating project structure...[/]\n");

        // Process all files in template
        await ProcessTemplateDirectoryAsync(templatePath, projectPath, values);

        // Success message
        AnsiConsole.MarkupLine($"\n[green]✓[/] Project created: [link]{projectPath}[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Next steps:[/]");
        AnsiConsole.MarkupLine($"  1. [white]cd {projectPath}[/]");
        AnsiConsole.MarkupLine($"  2. Open [white]{projectName}.csproj[/] in Visual Studio or Rider");
        var testTarget = templateName.StartsWith("grasshopper") ? "Grasshopper"
            : templateName.Contains("inside-revit") ? "Revit via Rhino.Inside.Revit"
            : templateName.Contains("inside-autocad") ? "AutoCAD via Rhino.Inside.AutoCAD"
            : "Rhino";
        AnsiConsole.MarkupLine($"  3. Build and test in {testTarget}");
        AnsiConsole.WriteLine();
    }

    private Dictionary<string, string> CollectPlaceholderValues(
        string projectName,
        bool interactive,
        PlatformSpec? spec)
    {
        var values = new Dictionary<string, string>();

        foreach (var (key, def) in Placeholders)
        {
            string value;

            // Handle $PluginName$ - always use project name
            if (key == "$PluginName$")
            {
                value = projectName;
                values[key] = value;
                continue;
            }

            // Handle derived values
            if (key == "$ComponentName$" && def.Default == null)
            {
                value = projectName;
                values[key] = value;
                continue;
            }

            if (key == "$ComponentNickname$" && def.Default == null)
            {
                // Take uppercase letters, up to 3
                var nickname = string.Concat(projectName.Where(char.IsUpper)).Take(3);
                value = new string(nickname.ToArray());
                if (string.IsNullOrEmpty(value))
                    value = projectName[..Math.Min(3, projectName.Length)];
                values[key] = value;
                continue;
            }

            // Handle auto-generated values (no prompt)
            if (def.Prompt == null)
            {
                value = def.DefaultFunc?.Invoke() ?? def.Default ?? "";
                values[key] = value;
                continue;
            }

            // Interactive prompting
            if (interactive)
            {
                var prompt = new TextPrompt<string>($"  {def.Prompt}:")
                    .AllowEmpty();

                if (!string.IsNullOrEmpty(def.Default))
                {
                    prompt.DefaultValue(def.Default);
                    prompt.ShowDefaultValue();
                }

                while (true)
                {
                    value = AnsiConsole.Prompt(prompt);

                    if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(def.Default))
                    {
                        value = def.Default;
                    }

                    if (def.Validator == null || def.Validator(value))
                    {
                        break;
                    }

                    AnsiConsole.MarkupLine("  [red]Invalid value. Please try again.[/]");
                }
            }
            else
            {
                value = def.Default ?? "";
            }

            values[key] = value;
        }

        return values;
    }

    private async Task ProcessTemplateDirectoryAsync(
        string sourceDir,
        string destDir,
        Dictionary<string, string> values)
    {
        // Process subdirectories
        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            var dirName = Path.GetFileName(dir);
            var newDirName = ReplacePlaceholders(dirName, values);
            var newDirPath = Path.Combine(destDir, newDirName);

            Directory.CreateDirectory(newDirPath);
            await ProcessTemplateDirectoryAsync(dir, newDirPath, values);
        }

        // Process files
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var fileName = Path.GetFileName(file);
            var newFileName = ReplacePlaceholders(fileName, values);
            var destFile = Path.Combine(destDir, newFileName);

            try
            {
                // Try to read as text and replace placeholders
                var content = await File.ReadAllTextAsync(file);
                var newContent = ReplacePlaceholders(content, values);
                await File.WriteAllTextAsync(destFile, newContent);

                var relativePath = Path.GetRelativePath(
                    Path.GetDirectoryName(destDir)!, destFile);
                AnsiConsole.MarkupLine($"  [green]✓[/] {relativePath}");
            }
            catch (Exception)
            {
                // Binary file - just copy
                File.Copy(file, destFile);
                var relativePath = Path.GetRelativePath(
                    Path.GetDirectoryName(destDir)!, destFile);
                AnsiConsole.MarkupLine($"  [green]✓[/] {relativePath} [dim](binary)[/]");
            }
        }
    }

    private static string ReplacePlaceholders(string input, Dictionary<string, string> values)
    {
        foreach (var (key, value) in values)
        {
            input = input.Replace(key, value);
        }
        return input;
    }

    private static bool ValidatePascalCase(string value)
    {
        return PascalCaseRegex().IsMatch(value);
    }

    [GeneratedRegex(@"^[A-Z][a-zA-Z0-9]*$")]
    private static partial Regex PascalCaseRegex();

    private record PlaceholderDef(
        string? Prompt,
        string? Default,
        Func<string, bool>? Validator = null,
        Func<string>? DefaultFunc = null)
    {
        public PlaceholderDef(string? prompt, Func<string> defaultFunc)
            : this(prompt, null, null, defaultFunc) { }
    }
}
