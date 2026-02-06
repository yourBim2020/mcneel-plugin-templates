# Rhino Plugin Template

A template for creating Rhino plugins with native commands, targeting **Rhino 8** with .NET 4.8 / .NET 8.

## Requirements

- Visual Studio 2022 or later / JetBrains Rider
- .NET Framework 4.8 SDK and/or .NET 8 SDK
- Rhino 8 installed (for testing)

## Getting Started

1. Use the scaffolding tool to create a new project:
   ```bash
   dotnet run --project tools/Scaffolder -- new rhino MyPluginName
   ```

2. Open the generated `.csproj` in Visual Studio or Rider

3. Build the project using the appropriate configuration:

   | Configuration | Framework | Output |
   |---------------|-----------|--------|
   | Debug         | .NET 4.8  | `.rhp` in build output |
   | Release       | .NET 4.8  | `.rhp` in build output |
   | DebugNET8     | .NET 8    | `.rhp` in build output |
   | ReleaseNET8   | .NET 8    | `.rhp` in build output |

4. Select the **Rhino 8 - netcore** launch profile and press F5 to debug

## Template Placeholders

| Placeholder | Description |
|-------------|-------------|
| `$PluginName$` | Plugin name (PascalCase, no spaces) |
| `$PluginDescription$` | Brief description of the plugin |
| `$AuthorName$` | Your name |
| `$AuthorEmail$` | Contact email |
| `$CompanyName$` | Company or organization name |
| `$Version$` | Semantic version (e.g., 1.0.0) |
| `$Year$` | Current year |
| `$PluginGuid$` | Unique GUID for the plugin |

## Project Structure

```
$PluginName$/
├── $PluginName$.csproj              # .NET 4.8 / .NET 8 project file (.rhp output)
├── $PluginName$Plugin.cs            # Plugin entry point (Rhino.PlugIns.PlugIn)
├── $PluginName$Command.cs           # Sample command (Rhino.Commands.Command)
├── EmbeddedResources/
│   └── plugin-utility.ico           # Plugin icon
└── Properties/
    ├── AssemblyInfo.cs              # Plugin metadata and description attributes
    └── launchSettings.json          # Rhino 8 debug profile
```

## Key Concepts

### Plugin Class

Every Rhino plugin has exactly one class that inherits from `Rhino.PlugIns.PlugIn`. Rhino manages its lifecycle — do not instantiate it yourself.

```csharp
public class MyPlugin : Rhino.PlugIns.PlugIn
{
    public MyPlugin() { Instance = this; }
    public static MyPlugin Instance { get; private set; }
}
```

### Commands

Commands inherit from `Rhino.Commands.Command` and are automatically discovered by Rhino through reflection. Each command defines an `EnglishName` (what users type) and implements `RunCommand`.

```csharp
public class MyCommand : Command
{
    public override string EnglishName => "MyCommand";

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
        // Your command logic here
        return Result.Success;
    }
}
```

### Interactive Input

Use `GetPoint`, `GetObject`, `GetString`, and other input classes for interactive user input:

```csharp
using (GetPoint gp = new GetPoint())
{
    gp.SetCommandPrompt("Pick a point");
    if (gp.Get() != GetResult.Point) return gp.CommandResult();
    Point3d pt = gp.Point();
}
```

## Adding Commands

1. Create a new `.cs` file in your project
2. Inherit from `Rhino.Commands.Command`
3. Define `EnglishName` and implement `RunCommand`
4. Build — Rhino discovers commands automatically

## Debugging

1. Select the **Rhino 8 - netcore** launch profile
2. Press F5 to launch Rhino with `RHINO_PACKAGE_DIRS` pointing to your build output
3. Type your command name in the Rhino command line
4. Set breakpoints in Visual Studio/Rider for debugging

## NuGet Packages

This template uses:

- **RhinoCommon** (8.0.23304.9001) - Rhino SDK

Note: `ExcludeAssets="runtime"` is set because Rhino provides RhinoCommon at runtime.

## Comparison with Grasshopper Templates

| Feature | Rhino Plugin | Grasshopper 1 | Grasshopper 2 |
|---------|-------------|---------------|---------------|
| Framework | .NET 4.8 / .NET 8 | .NET 4.8 / .NET 8 | .NET 8 |
| Output | `.rhp` | `.gha` | `.rhp` |
| Entry Point | `Rhino.PlugIns.PlugIn` | `GH_AssemblyInfo` | `Grasshopper2.Framework.Plugin` |
| UI Pattern | Commands | Components | Components |
| User Input | `GetPoint`, `GetObject` | Parameters (I/O) | Parameters (I/O) |

## Resources

- [Rhino Developer Documentation](https://developer.rhino3d.com/)
- [RhinoCommon API Reference](https://developer.rhino3d.com/api/rhinocommon/)
- [Rhino Plugin Guides](https://developer.rhino3d.com/guides/rhinocommon/)
