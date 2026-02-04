# McNeel Plugin Templates

A multi-platform template repository for developing plugins across the McNeel ecosystem, designed for spec-driven development with AI coding assistants.

## Supported Platforms

| Platform | Status | Target Framework | Description |
|----------|--------|------------------|-------------|
| **Grasshopper 1** | Ready | .NET 4.8 | Visual programming components for Rhino 8 |
| Grasshopper 2 | Planned | .NET 7 | Next-gen Grasshopper (Rhino 8+) |
| Rhino Plugin | Planned | .NET 7 | Native Rhino commands and features |
| Rhino.Inside Revit | Planned | .NET 4.8 | Rhino geometry in Autodesk Revit |
| Rhino.Inside AutoCAD | Planned | .NET 4.8 | Rhino geometry in AutoCAD |

## Quick Start

### Prerequisites

- Python 3.8+ (for scaffolding)
- Visual Studio 2022 or JetBrains Rider
- Rhino 8 installed
- .NET Framework 4.8 SDK (for GH1)

### Create a New Plugin

```bash
# Clone the repository
git clone https://github.com/yourBim2020/mcneel-plugin-templates.git
cd mcneel-plugin-templates

# Scaffold a new Grasshopper 1 plugin
python scripts/scaffold.py grasshopper1 MyAwesomePlugin

# Or use non-interactive mode with defaults
python scripts/scaffold.py grasshopper1 MyPlugin --non-interactive
```

### List Available Templates

```bash
python scripts/scaffold.py --list
```

## Repository Structure

```
mcneel-plugin-templates/
├── templates/                  # Plugin templates
│   ├── grasshopper1/          # Grasshopper 1 (Rhino 8)
│   ├── grasshopper2/          # Grasshopper 2 (planned)
│   ├── rhino-plugin/          # Rhino commands (planned)
│   ├── rhino-inside-revit/    # Rhino.Inside Revit (planned)
│   └── rhino-inside-autocad/  # Rhino.Inside AutoCAD (planned)
├── specs/                      # OpenSpec YAML definitions
│   └── grasshopper1.yaml      # GH1 platform requirements
├── docs/                       # Documentation
│   └── getting-started.md     # Setup and usage guide
└── scripts/                    # Tooling
    └── scaffold.py            # Project scaffolding utility
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

This repository includes [OpenSpec](https://github.com/Fission-AI/OpenSpec) definitions for each platform. Use them with AI coding assistants for guided, consistent development:

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
4. Update the scaffolding script if needed
5. Submit a pull request

## Resources

- [Rhino Developer Documentation](https://developer.rhino3d.com/)
- [Grasshopper SDK](https://developer.rhino3d.com/guides/grasshopper/)
- [RhinoCommon API](https://developer.rhino3d.com/api/rhinocommon/)
- [Rhino.Inside Documentation](https://www.rhino3d.com/inside/)
- [OpenSpec](https://github.com/Fission-AI/OpenSpec)

## License

MIT License - See [LICENSE](LICENSE) for details.
