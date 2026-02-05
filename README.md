# McNeel Plugin Templates

A multi-platform template repository for developing plugins across the McNeel ecosystem, designed for spec-driven development with AI coding assistants.

## Supported Platforms

| Platform | Status | Target Framework | Description |
|----------|--------|------------------|-------------|
| **Grasshopper 1** | Ready | .NET 4.8 | Visual programming components for Rhino 8 |
| **Grasshopper 2** | Ready | .NET 7 | Next-gen Grasshopper components (Rhino 8/9) |
| **Rhino Plugin** | Ready | .NET 7 | Native Rhino commands and features |
| Rhino.Inside Revit | Planned | .NET 4.8 | Rhino geometry in Autodesk Revit |
| Rhino.Inside AutoCAD | Planned | .NET 4.8 | Rhino geometry in AutoCAD |

## Quick Start

### Prerequisites

- .NET 8.0 SDK (for scaffolding tool)
- Visual Studio 2022 or JetBrains Rider
- Rhino 8 installed
- .NET Framework 4.8 SDK (for GH1 plugins)
- .NET 7 SDK (for GH2 and Rhino plugins)

### Create a New Plugin

```bash
# Clone the repository
git clone https://github.com/yourBim2020/mcneel-plugin-templates.git
cd mcneel-plugin-templates

# Scaffold a new Grasshopper 1 plugin (interactive)
dotnet run --project tools/Scaffolder -- new grasshopper1 MyAwesomePlugin

# Scaffold a new Grasshopper 2 plugin
dotnet run --project tools/Scaffolder -- new grasshopper2 MyGH2Plugin

# Scaffold a new Rhino plugin
dotnet run --project tools/Scaffolder -- new rhino MyRhinoPlugin

# Or use non-interactive mode with defaults
dotnet run --project tools/Scaffolder -- new grasshopper1 MyPlugin --interactive false

# Specify output directory
dotnet run --project tools/Scaffolder -- new grasshopper1 MyPlugin --output ./projects
```

### Scaffolder Commands

```bash
# List available templates
dotnet run --project tools/Scaffolder -- list

# Show template info (reads OpenSpec YAML)
dotnet run --project tools/Scaffolder -- info grasshopper1

# Get help
dotnet run --project tools/Scaffolder -- --help
```

### Build a Standalone Executable

```bash
cd tools/Scaffolder
dotnet publish -c Release -r win-x64 --self-contained

# Executable will be in bin/Release/net8.0/win-x64/publish/scaffolder.exe
```

## Repository Structure

```
mcneel-plugin-templates/
├── templates/                  # Plugin templates
│   ├── grasshopper1/          # Grasshopper 1 (Rhino 8)
│   ├── grasshopper2/          # Grasshopper 2 (Rhino 8/9)
│   ├── rhino/                 # Rhino plugin (Rhino 8)
│   ├── rhino-inside-revit/    # Rhino.Inside Revit (planned)
│   └── rhino-inside-autocad/  # Rhino.Inside AutoCAD (planned)
├── specs/                      # OpenSpec YAML definitions
│   ├── grasshopper1.yaml      # GH1 platform requirements
│   ├── grasshopper2.yaml      # GH2 platform requirements
│   └── rhino.yaml             # Rhino plugin platform requirements
├── docs/                       # Documentation
│   └── getting-started.md     # Setup and usage guide
└── tools/
    └── Scaffolder/             # C# scaffolding CLI tool
```

## Template Placeholders

All templates use `$PLACEHOLDER$` style variables that are replaced during scaffolding:

| Placeholder | Description | Example |
|-------------|-------------|---------|
| `$PluginName$` | Plugin name (PascalCase) | `GeometryTools` |
| `$PluginDescription$` | Brief description | `Geometry utilities` |
| `$AuthorName$` | Developer name | `John Doe` |
| `$CompanyName$` | Company name | `Acme Inc` |
| `$Version$` | Semantic version | `1.0.0` |
| `$PluginGuid$` | Unique plugin identifier | Auto-generated |
| `$Category$` | Grasshopper tab | `Custom` |
| `$Subcategory$` | Tab subcategory | `Util` |

See individual template READMEs for full placeholder lists.

## Spec-Driven Development

This repository includes [OpenSpec](https://github.com/Fission-AI/OpenSpec) definitions for each platform. The scaffolder reads these specs to provide platform information:

```bash
# View platform requirements from spec
dotnet run --project tools/Scaffolder -- info grasshopper1
```

Use with AI coding assistants for guided, consistent development:

```bash
# Initialize OpenSpec in your project
openspec init --tools claude

# Start a new feature
/opsx:new

# Have the AI implement based on platform specs
/opsx:apply
```

The specs in `specs/` define:
- Runtime requirements and dependencies
- Project structure conventions
- Naming conventions
- Common pitfalls and solutions

## Contributing

Contributions welcome! To add a new template:

1. Create a directory in `templates/`
2. Add template files with `$PLACEHOLDER$` variables
3. Create a corresponding spec in `specs/`
4. Submit a pull request

## Resources

- [Rhino Developer Documentation](https://developer.rhino3d.com/)
- [Grasshopper SDK](https://developer.rhino3d.com/guides/grasshopper/)
- [RhinoCommon API](https://developer.rhino3d.com/api/rhinocommon/)
- [Rhino.Inside Documentation](https://www.rhino3d.com/inside/)
- [OpenSpec](https://github.com/Fission-AI/OpenSpec)

## License

MIT License - See [LICENSE](LICENSE) for details.
