#!/usr/bin/env python3
"""
McNeel Plugin Template Scaffolding Tool

Generates new plugin projects from templates by replacing $PLACEHOLDER$ variables.

Usage:
    python scaffold.py <template> <project_name> [output_dir]

Examples:
    python scaffold.py grasshopper1 MyGHPlugin
    python scaffold.py grasshopper1 MyGHPlugin ./projects
"""

import os
import re
import sys
import uuid
import shutil
import argparse
from datetime import datetime
from pathlib import Path


# Template placeholder definitions
PLACEHOLDERS = {
    "$PluginName$": {
        "prompt": "Plugin name (PascalCase)",
        "default": None,  # Required, set from project_name
        "validate": r"^[A-Z][a-zA-Z0-9]*$"
    },
    "$PluginDescription$": {
        "prompt": "Plugin description",
        "default": "A Grasshopper plugin"
    },
    "$AuthorName$": {
        "prompt": "Author name",
        "default": os.environ.get("USERNAME", "Developer")
    },
    "$AuthorEmail$": {
        "prompt": "Author email",
        "default": ""
    },
    "$CompanyName$": {
        "prompt": "Company/Organization name",
        "default": ""
    },
    "$Version$": {
        "prompt": "Initial version",
        "default": "1.0.0"
    },
    "$Year$": {
        "prompt": None,  # Auto-generated
        "default": str(datetime.now().year)
    },
    "$PluginGuid$": {
        "prompt": None,  # Auto-generated
        "default": lambda: str(uuid.uuid4())
    },
    "$AssemblyGuid$": {
        "prompt": None,  # Auto-generated
        "default": lambda: str(uuid.uuid4())
    },
    "$ComponentGuid$": {
        "prompt": None,  # Auto-generated
        "default": lambda: str(uuid.uuid4())
    },
    "$ComponentName$": {
        "prompt": "Component display name",
        "default": None  # Derived from plugin name
    },
    "$ComponentNickname$": {
        "prompt": "Component nickname (1-3 chars)",
        "default": None  # Derived from plugin name
    },
    "$ComponentDescription$": {
        "prompt": "Component description",
        "default": "A sample component"
    },
    "$Category$": {
        "prompt": "Grasshopper tab category",
        "default": "Custom"
    },
    "$Subcategory$": {
        "prompt": "Subcategory within tab",
        "default": "Util"
    },
}


def get_templates_dir() -> Path:
    """Get the templates directory path."""
    script_dir = Path(__file__).parent.resolve()
    return script_dir.parent / "templates"


def list_templates() -> list[str]:
    """List available templates."""
    templates_dir = get_templates_dir()
    if not templates_dir.exists():
        return []
    return [d.name for d in templates_dir.iterdir() if d.is_dir()]


def validate_placeholder(key: str, value: str) -> bool:
    """Validate a placeholder value against its pattern."""
    config = PLACEHOLDERS.get(key, {})
    pattern = config.get("validate")
    if pattern and not re.match(pattern, value):
        return False
    return True


def collect_values(project_name: str, interactive: bool = True) -> dict[str, str]:
    """Collect placeholder values from user or defaults."""
    values = {}

    for key, config in PLACEHOLDERS.items():
        # Handle auto-generated values
        default = config["default"]
        if callable(default):
            values[key] = default()
            continue

        # Handle $PluginName$ specially
        if key == "$PluginName$":
            values[key] = project_name
            continue

        # Handle derived values
        if key == "$ComponentName$" and default is None:
            values[key] = project_name
            continue

        if key == "$ComponentNickname$" and default is None:
            # Take first letters of PascalCase words, up to 3
            nickname = "".join(c for c in project_name if c.isupper())[:3]
            values[key] = nickname or project_name[:3]
            continue

        # Handle values with no prompt (auto-generated)
        if config["prompt"] is None:
            values[key] = default if default else ""
            continue

        # Interactive prompting
        if interactive:
            prompt_text = f"{config['prompt']}"
            if default:
                prompt_text += f" [{default}]"
            prompt_text += ": "

            while True:
                user_input = input(prompt_text).strip()
                value = user_input if user_input else (default or "")

                if validate_placeholder(key, value):
                    values[key] = value
                    break
                else:
                    print(f"  Invalid value. Please try again.")
        else:
            values[key] = default if default else ""

    return values


def replace_in_content(content: str, values: dict[str, str]) -> str:
    """Replace all placeholders in content."""
    for key, value in values.items():
        content = content.replace(key, value)
    return content


def replace_in_path(path: str, values: dict[str, str]) -> str:
    """Replace placeholders in a file/directory path."""
    for key, value in values.items():
        path = path.replace(key, value)
    return path


def scaffold_template(
    template_name: str,
    project_name: str,
    output_dir: Path,
    interactive: bool = True
) -> Path:
    """
    Scaffold a new project from a template.

    Args:
        template_name: Name of the template (e.g., 'grasshopper1')
        project_name: Name for the new project
        output_dir: Directory to create the project in
        interactive: Whether to prompt for values

    Returns:
        Path to the created project directory
    """
    templates_dir = get_templates_dir()
    template_path = templates_dir / template_name

    if not template_path.exists():
        available = list_templates()
        raise ValueError(
            f"Template '{template_name}' not found. "
            f"Available templates: {', '.join(available)}"
        )

    # Collect placeholder values
    print(f"\n📦 Scaffolding '{template_name}' template as '{project_name}'")
    print("=" * 50)
    values = collect_values(project_name, interactive)

    # Create output directory
    project_dir = output_dir / project_name
    if project_dir.exists():
        raise ValueError(f"Directory already exists: {project_dir}")

    project_dir.mkdir(parents=True, exist_ok=True)

    # Process template files
    print(f"\n📁 Creating project structure...")

    for root, dirs, files in os.walk(template_path):
        # Calculate relative path from template root
        rel_root = Path(root).relative_to(template_path)

        # Create directory with placeholder replacement
        dest_dir = project_dir / replace_in_path(str(rel_root), values)
        dest_dir.mkdir(parents=True, exist_ok=True)

        # Process files
        for filename in files:
            src_file = Path(root) / filename
            new_filename = replace_in_path(filename, values)
            dest_file = dest_dir / new_filename

            # Read, replace, and write
            try:
                content = src_file.read_text(encoding="utf-8")
                new_content = replace_in_content(content, values)
                dest_file.write_text(new_content, encoding="utf-8")
                print(f"  ✓ {dest_file.relative_to(project_dir)}")
            except UnicodeDecodeError:
                # Binary file, just copy
                shutil.copy2(src_file, dest_file)
                print(f"  ✓ {dest_file.relative_to(project_dir)} (binary)")

    print(f"\n✅ Project created: {project_dir}")
    print(f"\nNext steps:")
    print(f"  1. cd {project_dir}")
    print(f"  2. Open {project_name}.csproj in Visual Studio or Rider")
    print(f"  3. Build and test in Grasshopper")

    return project_dir


def main():
    parser = argparse.ArgumentParser(
        description="Scaffold a new McNeel plugin from a template",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  %(prog)s grasshopper1 MyPlugin
  %(prog)s grasshopper1 MyPlugin --output ./projects
  %(prog)s --list

Available templates are in the 'templates/' directory.
        """
    )

    parser.add_argument(
        "template",
        nargs="?",
        help="Template name (e.g., grasshopper1, rhino-plugin)"
    )
    parser.add_argument(
        "project_name",
        nargs="?",
        help="Name for the new project (PascalCase)"
    )
    parser.add_argument(
        "--output", "-o",
        type=Path,
        default=Path.cwd(),
        help="Output directory (default: current directory)"
    )
    parser.add_argument(
        "--list", "-l",
        action="store_true",
        help="List available templates"
    )
    parser.add_argument(
        "--non-interactive", "-n",
        action="store_true",
        help="Use defaults without prompting"
    )

    args = parser.parse_args()

    # List templates
    if args.list:
        templates = list_templates()
        if templates:
            print("Available templates:")
            for t in templates:
                print(f"  - {t}")
        else:
            print("No templates found.")
        return 0

    # Validate arguments
    if not args.template or not args.project_name:
        parser.print_help()
        return 1

    # Validate project name
    if not re.match(r"^[A-Z][a-zA-Z0-9]*$", args.project_name):
        print(f"Error: Project name must be PascalCase (e.g., MyPlugin)")
        return 1

    try:
        scaffold_template(
            args.template,
            args.project_name,
            args.output,
            interactive=not args.non_interactive
        )
        return 0
    except ValueError as e:
        print(f"Error: {e}")
        return 1
    except Exception as e:
        print(f"Error: {e}")
        raise


if __name__ == "__main__":
    sys.exit(main())
