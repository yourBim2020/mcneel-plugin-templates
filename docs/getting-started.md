# Getting Started with McNeel Plugin Development

This guide walks you through setting up your development environment and creating your first McNeel plugin.

## Development Environment Setup

### Required Software

1. **Rhino 8** - The host application
   - Download from [rhino3d.com](https://www.rhino3d.com/download/)
   - A license is required for testing (evaluation available)

2. **Visual Studio 2022** or **JetBrains Rider**
   - Visual Studio: Install ".NET desktop development" workload
   - Include ".NET Framework 4.8 SDK" component

3. **Python 3.8+** - For the scaffolding tool
   - Download from [python.org](https://www.python.org/downloads/)

4. **.NET SDKs**
   - .NET Framework 4.8 SDK (for Grasshopper 1)
   - .NET 7 SDK (for Grasshopper 2 / Rhino plugins)

### Verify Installation

```bash
# Check Python
python --version

# Check .NET
dotnet --list-sdks
```

## Creating Your First Grasshopper Plugin

### Step 1: Scaffold the Project

```bash
cd mcneel-plugin-templates
python scripts/scaffold.py grasshopper1 MyFirstPlugin
```

You'll be prompted for:
- Plugin description
- Author information
- Component category and subcategory

### Step 2: Open in Your IDE

Open `MyFirstPlugin/MyFirstPlugin.csproj` in Visual Studio or Rider.

### Step 3: Build

Build the project in **Debug** mode. The DLL will be automatically copied to:
```
%APPDATA%\Grasshopper\Libraries\MyFirstPlugin\
```

### Step 4: Test in Grasshopper

1. Launch **Rhino 8**
2. Type `Grasshopper` in the command line
3. Find your component under the category you specified
4. Drop it on the canvas and test!

## Understanding the Template Structure

```
MyFirstPlugin/
├── MyFirstPlugin.csproj           # Project file
├── MyFirstPluginComponent.cs      # Your component logic
├── MyFirstPluginInfo.cs           # Plugin metadata
├── Properties/
│   └── AssemblyInfo.cs            # Assembly attributes
└── Resources/                      # Icons and assets
```

### Key Files Explained

#### `MyFirstPluginComponent.cs`

This is where your component logic lives:

```csharp
protected override void SolveInstance(IGH_DataAccess DA)
{
    // 1. Get input data
    GeometryBase geometry = null;
    DA.GetData(0, ref geometry);

    // 2. Process
    var result = ProcessGeometry(geometry);

    // 3. Set output
    DA.SetData(0, result);
}
```

#### `MyFirstPluginInfo.cs`

Defines how your plugin appears in Grasshopper:

```csharp
public override string Name => "MyFirstPlugin";
public override string Description => "What it does";
public override Guid Id => new Guid("...");  // Never change this!
```

## Adding More Components

1. Create a new `.cs` file in your project
2. Inherit from `GH_Component`
3. **Generate a new unique GUID** for `ComponentGuid`
4. Implement the required methods
5. Build and test

```csharp
public class AnotherComponent : GH_Component
{
    public AnotherComponent()
        : base("Another", "An", "Description", "Category", "Subcategory")
    { }

    // Always generate a NEW GUID for each component
    public override Guid ComponentGuid => new Guid("NEW-GUID-HERE");

    // ... implement methods
}
```

## Debugging

### Visual Studio

1. Set breakpoints in your code
2. **Debug > Attach to Process**
3. Select `Rhino.exe`
4. Use your component in Grasshopper to hit breakpoints

### Rider

1. Set breakpoints
2. **Run > Attach to Process**
3. Select `Rhino.exe`
4. Debug as usual

## Common Issues

### Component Doesn't Appear

- Check `GH_Exposure` isn't set to `hidden`
- Verify the DLL is in the Libraries folder
- Restart Grasshopper completely (close and reopen)

### Build Errors About Missing References

- Ensure Grasshopper NuGet package is restored
- Check target framework is `net48`

### Runtime Errors

- Make sure `CopyLocalLockFileAssemblies` is `false`
- Verify `ExcludeAssets="runtime"` on package references

### GUID Conflicts

- Never copy a component's GUID
- Always generate new unique GUIDs
- Use Visual Studio: **Tools > Create GUID**

## Publishing Your Plugin

### Food4Rhino

1. Create an account at [food4rhino.com](https://www.food4rhino.com/)
2. Package your DLL in a ZIP
3. Submit with description, screenshots, and version info

### Yak Package Manager

```bash
# Create manifest
yak spec

# Build package
yak build

# Push to server
yak push
```

## Next Steps

- Explore the [RhinoCommon API](https://developer.rhino3d.com/api/rhinocommon/)
- Study [Grasshopper SDK samples](https://github.com/mcneel/rhino-developer-samples)
- Check the OpenSpec definitions for platform conventions
- Join the [Rhino Developer Forum](https://discourse.mcneel.com/c/rhino-developer/)

## Platform Comparison

| Feature | Grasshopper 1 | Grasshopper 2 | Rhino Plugin |
|---------|---------------|---------------|--------------|
| Framework | .NET 4.8 | .NET 7 | .NET 7 |
| UI | Component-based | Component-based | Commands |
| Entry Point | GH_Component | GH_Component | Rhino.PlugIns.PlugIn |
| Distribution | GHA/Yak | Yak | RHP/Yak |
| Rhino Version | 7/8 | 8+ | 8+ |
