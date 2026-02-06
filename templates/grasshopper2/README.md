# Grasshopper 2 Plugin Template

A template for creating Grasshopper 2 components targeting **Rhino 8/9** with .NET 8.

## Requirements

- Visual Studio 2022 or later / JetBrains Rider
- .NET 8 SDK
- Rhino 8 or Rhino 9 WIP installed (for testing)
- Grasshopper 2 plugin (currently in Alpha)

## Getting Started

1. Use the scaffolding tool to create a new project:
   ```bash
   dotnet run --project tools/Scaffolder -- new grasshopper2 MyPluginName
   ```

2. Open the generated `.csproj` in Visual Studio or Rider

3. Build the project — output is a `.rhp` file in `bin/`

4. Start Rhino 8 (with `/netcore` flag) or Rhino 9 and open Grasshopper to test your component

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
| `$ComponentGuid$` | Unique GUID for the component |
| `$ComponentName$` | Display name of the component |
| `$ComponentDescription$` | Component description |
| `$Subcategory$` | Subcategory within the plugin tab |

## Project Structure

```
$PluginName$/
├── $PluginName$.csproj           # .NET 8 project file (.rhp output)
├── $PluginName$Plugin.cs         # Plugin entry point (Grasshopper2.Framework.Plugin)
├── Nodes/
│   └── $PluginName$Component.cs  # Sample component (Grasshopper2.Components.Component)
├── Icons/                         # .ghicon files for component icons
└── Properties/
    └── launchSettings.json        # Debug profiles for Rhino 8/9
```

## Key Differences from Grasshopper 1

| Feature | Grasshopper 1 | Grasshopper 2 |
|---------|---------------|---------------|
| Framework | .NET 4.8 | .NET 8 |
| Output | `.gha` | `.rhp` |
| Plugin Base | `GH_AssemblyInfo` | `Grasshopper2.Framework.Plugin` |
| Component Base | `GH_Component` | `Grasshopper2.Components.Component` |
| Component ID | `ComponentGuid` property | `[IoId]` attribute |
| Input/Output | `RegisterInputParams` | `AddInputs` / `AddOutputs` |
| Solve Method | `SolveInstance(IGH_DataAccess)` | `Process(IDataAccess)` |
| Data Access | `GetData` / `SetData` | `GetItem` / `SetItem` |
| List Access | `GH_ParamAccess.list` | `Access.Twig` / `GetTwig` / `SetTwig` |
| Tree Access | `GH_ParamAccess.tree` | `Access.Tree` / `GetTree` / `SetTree` |
| Naming | Constructor strings | `Nomen` class |

## Adding Components

Create additional components by:

1. Create a new `.cs` file in the `Nodes/` folder
2. Generate a new unique GUID at https://guidgenerator.com/
3. Apply the `[IoId("your-guid")]` attribute
4. Inherit from `Component` and implement `AddInputs`, `AddOutputs`, and `Process`
5. Include the `IReader` deserialization constructor

```csharp
[IoId("NEW-GUID-HERE")]
public sealed class MyNewComponent : Component
{
    public MyNewComponent()
    : base(new Nomen("My Component", "Description", "MyPlugin", "Category")) { }

    public MyNewComponent(IReader reader) : base(reader) { }

    protected override void AddInputs(InputAdder inputs) { /* ... */ }
    protected override void AddOutputs(OutputAdder outputs) { /* ... */ }
    protected override void Process(IDataAccess access) { /* ... */ }
}
```

## Adding Icons

1. Create a `.ghicon` file (Grasshopper 2 icon format)
2. Add it to the `Icons/` folder
3. Icons are automatically embedded via the `.csproj` glob pattern

## Debugging

1. Select the **Rhino 8 - netcore** or **Rhino 9 - netcore** launch profile
2. Press F5 to launch Rhino with `RHINO_PACKAGE_DIRS` pointing to your build output
3. Open Grasshopper and test your component
4. Set breakpoints in Visual Studio/Rider for debugging

## NuGet Packages

This template uses:

- **Grasshopper2** (2.0.9225-wip.14825) - Grasshopper 2 SDK
- **RhinoCommon** (8.17.25066.7001) - Rhino geometry and SDK

Note: `CopyLocalLockFileAssemblies` is `true` for GH2 — assemblies need to be copied to the output directory.

## Resources

- [Rhino Developer Documentation](https://developer.rhino3d.com/)
- [RhinoCommon API Reference](https://developer.rhino3d.com/api/rhinocommon/)
- [Proving Ground GH2 Template](https://github.com/provingground-curaea/g2-plugin-template) (reference)
