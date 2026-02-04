using System.CommandLine;
using Scaffolder;

var rootCommand = new RootCommand("McNeel Plugin Template Scaffolder")
{
    Name = "scaffolder"
};

// List command
var listCommand = new Command("list", "List available templates");
listCommand.SetHandler(() =>
{
    var templates = TemplateManager.GetAvailableTemplates();
    if (templates.Count == 0)
    {
        Console.WriteLine("No templates found.");
        return;
    }

    Console.WriteLine("Available templates:");
    foreach (var template in templates)
    {
        Console.WriteLine($"  - {template}");
    }
});
rootCommand.AddCommand(listCommand);

// New command
var newCommand = new Command("new", "Create a new project from a template");

var templateArg = new Argument<string>("template", "Template name (e.g., grasshopper1)");
var nameArg = new Argument<string>("name", "Project name (PascalCase)");
var outputOption = new Option<DirectoryInfo?>(
    new[] { "--output", "-o" },
    "Output directory (default: current directory)");
var interactiveOption = new Option<bool>(
    new[] { "--interactive", "-i" },
    () => true,
    "Enable interactive prompts");

newCommand.AddArgument(templateArg);
newCommand.AddArgument(nameArg);
newCommand.AddOption(outputOption);
newCommand.AddOption(interactiveOption);

newCommand.SetHandler(async (template, name, output, interactive) =>
{
    var outputDir = output?.FullName ?? Directory.GetCurrentDirectory();

    try
    {
        var scaffolder = new ProjectScaffolder();
        await scaffolder.CreateProjectAsync(template, name, outputDir, interactive);
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
        Environment.ExitCode = 1;
    }
}, templateArg, nameArg, outputOption, interactiveOption);

rootCommand.AddCommand(newCommand);

// Info command
var infoCommand = new Command("info", "Show information about a template");
var infoTemplateArg = new Argument<string>("template", "Template name");
infoCommand.AddArgument(infoTemplateArg);

infoCommand.SetHandler((template) =>
{
    var spec = SpecReader.LoadSpec(template);
    if (spec == null)
    {
        Console.WriteLine($"No spec found for template: {template}");
        return;
    }

    SpecReader.PrintSpecInfo(spec);
}, infoTemplateArg);

rootCommand.AddCommand(infoCommand);

return await rootCommand.InvokeAsync(args);
