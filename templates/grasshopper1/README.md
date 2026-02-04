# Grasshopper 1 Plugin Template

A template for creating Grasshopper 1 components targeting **Rhino 8**.

## Requirements

- Visual Studio 2022 or later / JetBrains Rider
- .NET Framework 4.8 SDK
- Rhino 8 installed (for testing)

## Getting Started

1. Use the scaffolding script to create a new project:
   ```bash
   python scripts/scaffold.py grasshopper1 MyPluginName
   ```

2. Open the generated `.csproj` in Visual Studio or Rider

3. Build the project:
   - **Debug**: Automatically copies to `%APPDATA%\Grasshopper\Libraries\`
   - **Release**: Outputs to `bin\Release\`

4. Start Rhino 8 and open Grasshopper to test your component

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
├── $PluginName$Component.cs   # Main component class
├── $PluginName$Info.cs        # GH_AssemblyInfo implementation
├── Properties/
│   └── AssemblyInfo.cs        # Assembly attributes
└── Resources/                  # Icons and embedded resources
```

## Adding Components

Create additional components by:

1. Copy `$PluginName$Component.cs` as a starting point
2. Generate a new unique GUID for `ComponentGuid`
3. Update the component name, description, and parameters
4. Implement your logic in `SolveInstance()`

## Adding Icons

1. Create a 24x24 pixel PNG or BMP image
2. Add it to the `Resources` folder
3. Set Build Action to "Embedded Resource"
4. Reference it in your component's `Icon` property

## NuGet Packages

This template uses:

- **Grasshopper** (8.0.23304.9001) - Includes RhinoCommon

The `ExcludeAssets="runtime"` ensures Rhino's own assemblies are used at runtime.

## Debugging

1. Set a breakpoint in your code
2. In Visual Studio: Debug → Attach to Process → `Rhino.exe`
3. In Grasshopper, use your component to hit the breakpoint

## Publishing

For distribution:

1. Build in Release mode
2. Package the DLL with any dependencies
3. Consider creating a Yak package for the Rhino Package Manager

## Resources

- [Grasshopper Developer Documentation](https://developer.rhino3d.com/guides/grasshopper/)
- [RhinoCommon API Reference](https://developer.rhino3d.com/api/rhinocommon/)
- [Yak Package Manager](https://developer.rhino3d.com/guides/yak/)
