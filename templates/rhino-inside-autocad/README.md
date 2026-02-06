# Rhino.Inside AutoCAD Plugin Template

A template for creating Grasshopper components that run inside **Autodesk AutoCAD** via Rhino.Inside.AutoCAD.

## Requirements

- Visual Studio 2022 or later / JetBrains Rider
- .NET Framework 4.8 SDK and/or .NET 8 SDK
- Autodesk AutoCAD (2024 or later) installed
- [Rhino.Inside.AutoCAD](https://www.rhino3d.com/inside/autocad/) installed

## Getting Started

1. Use the scaffolding tool to create a new project:
   ```bash
   dotnet run --project tools/Scaffolder -- new rhino-inside-autocad MyPluginName
   ```

2. Open the generated `.csproj` in Visual Studio or Rider

3. Verify the `AutoCAD.NET` and `AutoCAD.NET.Core` NuGet package versions match your target AutoCAD version

4. Build the project using the appropriate configuration:

   | Configuration | Framework | Output |
   |---------------|-----------|--------|
   | Debug         | .NET 4.8  | `%APPDATA%\Grasshopper\Libraries\` |
   | Release       | .NET 4.8  | `bin\Release\` |
   | DebugNET8     | .NET 8    | `%APPDATA%\Grasshopper\Libraries\` |
   | ReleaseNET8   | .NET 8    | `bin\Release\` |

5. Launch AutoCAD, start Rhino.Inside.AutoCAD, open Grasshopper, and test your component

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
├── $PluginName$Component.cs   # Main component class (accesses AutoCAD API)
├── $PluginName$Info.cs        # GH_AssemblyInfo implementation
├── Properties/
│   └── AssemblyInfo.cs        # Assembly attributes
└── Resources/                  # Icons and embedded resources
```

## Accessing AutoCAD API

Inside your `SolveInstance()` method, access AutoCAD through the standard .NET API:

```csharp
using Autodesk.AutoCAD.ApplicationServices.Core;
using AcDb = Autodesk.AutoCAD.DatabaseServices;

// Get the active AutoCAD document
var doc = Application.DocumentManager.MdiActiveDocument;
var db = doc.Database;

// Query entities with a Transaction
using (var tr = db.TransactionManager.StartTransaction())
{
    var blockTable = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
    var modelSpace = (AcDb.BlockTableRecord)tr.GetObject(
        blockTable[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForRead);

    foreach (AcDb.ObjectId id in modelSpace)
    {
        var entity = (AcDb.Entity)tr.GetObject(id, AcDb.OpenMode.ForRead);
        // Process entity
    }

    tr.Commit();
}
```

## Adding Components

Create additional components by:

1. Copy `$PluginName$Component.cs` as a starting point
2. Generate a new unique GUID for `ComponentGuid`
3. Update the component name, description, and parameters
4. Implement your AutoCAD-aware logic in `SolveInstance()`

## Adding Icons

1. Create a 24x24 pixel PNG or BMP image
2. Add it to the `Resources` folder
3. Set Build Action to "Embedded Resource"
4. Reference it in your component's `Icon` property

## NuGet Packages

This template uses:

- **Grasshopper** (8.0.23304.9001) - Includes RhinoCommon
- **AutoCAD.NET** (24.0.0) - AutoCAD managed wrappers (acmgd, acdbmgd)
- **AutoCAD.NET.Core** (24.0.0) - AutoCAD core managed wrapper (accoremgd)

All packages use `ExcludeAssets="runtime"` to use the host application's assemblies at runtime.

Update the `AutoCAD.NET` and `AutoCAD.NET.Core` versions to match your target AutoCAD release:

| AutoCAD Version | NuGet Package Version |
|-----------------|----------------------|
| AutoCAD 2024    | 24.0.0               |
| AutoCAD 2025    | 25.0.0               |

## Debugging

1. Set a breakpoint in your code
2. In Visual Studio: Debug > Attach to Process > `acad.exe`
3. In AutoCAD, start Rhino.Inside, open Grasshopper, and use your component

## Publishing

For distribution:

1. Build in Release mode
2. Package the `.gha` with any dependencies
3. Instruct users to place it in `%APPDATA%\Grasshopper\Libraries\`

## Resources

- [Rhino.Inside.AutoCAD](https://www.rhino3d.com/inside/autocad/)
- [Grasshopper Developer Documentation](https://developer.rhino3d.com/guides/grasshopper/)
- [RhinoCommon API Reference](https://developer.rhino3d.com/api/rhinocommon/)
- [AutoCAD .NET Developer Guide](https://help.autodesk.com/view/OARX/2025/ENU/?guid=GUID-C3F3C736-40CF-44A0-9210-55F6A939B6F2)
