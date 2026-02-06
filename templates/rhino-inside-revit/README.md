# Rhino.Inside Revit Plugin Template

A template for creating Grasshopper components that run inside **Autodesk Revit** via Rhino.Inside.Revit.

## Requirements

- Visual Studio 2022 or later / JetBrains Rider
- .NET Framework 4.8 SDK
- Autodesk Revit (2020 or later) installed
- [Rhino.Inside.Revit](https://www.rhino3d.com/inside/revit/) installed

## Getting Started

1. Use the scaffolding tool to create a new project:
   ```bash
   dotnet run --project tools/Scaffolder -- new rhino-inside-revit MyPluginName
   ```

2. Open the generated `.csproj` in Visual Studio or Rider

3. Verify the Revit API reference paths match your Revit installation:
   - `RevitAPI.dll` and `RevitAPIUI.dll` from your Revit install directory
   - `RhinoInside.Revit.dll` from the Rhino.Inside.Revit addin folder

4. Build the project:
   - **Debug**: Automatically copies to `%APPDATA%\Grasshopper\Libraries-Inside-Revit-20XX\`
   - **Release**: Outputs to `bin\Release\`

5. Launch Revit, start Rhino.Inside.Revit, open Grasshopper, and test your component

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
| `$RevitYear$` | Target Revit version year (e.g., 2024) |
| `$PluginGuid$` | Unique GUID for the plugin |
| `$AssemblyGuid$` | Unique GUID for the assembly |
| `$ComponentGuid$` | Unique GUID for the component |
| `$ComponentName$` | Display name of the component |
| `$ComponentNickname$` | Short nickname (1-3 chars) |
| `$ComponentDescription$` | Component description |
| `$Category$` | Grasshopper tab category |
| `$Subcategory$` | Subcategory within the tab |

## Project Structure

```
$PluginName$/
├── $PluginName$.csproj        # SDK-style project file
├── $PluginName$Component.cs   # Main component class (accesses Revit API)
├── $PluginName$Info.cs        # GH_AssemblyInfo implementation
├── Properties/
│   └── AssemblyInfo.cs        # Assembly attributes
└── Resources/                  # Icons and embedded resources
```

## Accessing Revit API

Inside your `SolveInstance()` method, access Revit through the `RhinoInside.Revit.Revit` static type:

```csharp
using DB = Autodesk.Revit.DB;
using UI = Autodesk.Revit.UI;

// Get the active Revit document
DB.Document doc = RhinoInside.Revit.Revit.ActiveDBDocument;

// Get the Revit UI application
UI.UIApplication uiApp = RhinoInside.Revit.Revit.ActiveUIApplication;

// Query elements with FilteredElementCollector
using (var collector = new DB.FilteredElementCollector(doc))
{
    var walls = collector.OfClass(typeof(DB.Wall)).ToList();
}
```

## Adding Components

Create additional components by:

1. Copy `$PluginName$Component.cs` as a starting point
2. Generate a new unique GUID for `ComponentGuid`
3. Update the component name, description, and parameters
4. Implement your Revit-aware logic in `SolveInstance()`

## Adding Icons

1. Create a 24x24 pixel PNG or BMP image
2. Add it to the `Resources` folder
3. Set Build Action to "Embedded Resource"
4. Reference it in your component's `Icon` property

## NuGet Packages

This template uses:

- **Grasshopper** (7.0.0) - Includes RhinoCommon

The `ExcludeAssets="runtime"` ensures Rhino's own assemblies are used at runtime.

## Assembly References

These are direct DLL references (not NuGet), set to `Private=false`:

- **RevitAPI.dll** - From `C:\Program Files\Autodesk\Revit 20XX\`
- **RevitAPIUI.dll** - From `C:\Program Files\Autodesk\Revit 20XX\`
- **RhinoInside.Revit.dll** - From `%ProgramData%\Autodesk\Revit\Addins\20XX\RhinoInside.Revit\`

## Installation Paths

Place the compiled `.gha` in one of these version-specific folders:

- **All Users**: `%PROGRAMDATA%\Grasshopper\Libraries-Inside-Revit-20XX`
- **Current User**: `%APPDATA%\Grasshopper\Libraries-Inside-Revit-20XX`

This ensures the plugin only loads within Rhino.Inside.Revit where Revit API access is available.

## Debugging

1. Set a breakpoint in your code
2. In Visual Studio: Debug > Attach to Process > `Revit.exe`
3. In Revit, start Rhino.Inside, open Grasshopper, and use your component

## Publishing

For distribution:

1. Build in Release mode
2. Package the `.gha` with any dependencies
3. Instruct users to place it in `Libraries-Inside-Revit-20XX`

## Resources

- [Rhino.Inside.Revit Documentation](https://www.rhino3d.com/inside/revit/)
- [Rhino.Inside.Revit Plugin Guide](https://www.rhino3d.com/inside/revit/1.0/reference/rir-plugins)
- [Grasshopper Developer Documentation](https://developer.rhino3d.com/guides/grasshopper/)
- [RhinoCommon API Reference](https://developer.rhino3d.com/api/rhinocommon/)
- [Revit API Documentation](https://www.revitapidocs.com/)
