# MyApp NuGet Package Developer Guide

## Template Structure

This project is configured as a .NET template and can be distributed via NuGet.

### Important Files

- `.template.config/template.json` - Template configuration
- `.netnew.json` - .NET CLI version information
- `MyApp.nuspec` - NuGet package definition
- `create-package.ps1` - PowerShell script for package creation

## Local Testing

To test the template locally:

```bash
# 1. Create package
dotnet pack MyApp.sln -o dist

# 2. Install template
dotnet new install ./dist/MyApp.Solution.1.0.0.nupkg

# 3. Create new project
dotnet new onion --name TestProject --company TestCo

# 4. Verify it works
cd TestProject
dotnet build
dotnet run

# 5. Uninstall template
dotnet new uninstall MyApp.Solution
```

## Publishing to NuGet

### Prerequisites

1. NuGet API Key (available from nuget.org account)
2. `nuget.exe` installed

### Publishing Steps

```bash
# 1. Update .netnew.json and template.json with version
# 2. Create and publish package
./create-package.ps1 -TemplateVersion "1.0.0" -NuGetApiKey "your-api-key"
```

Or manually:

```bash
# 1. Update NuSpec
# 2. Create package
nuget pack MyApp.nuspec -Version 1.0.0 -OutputDirectory dist

# 3. Publish to NuGet
nuget push dist/MyApp.Solution.1.0.0.nupkg -ApiKey your-api-key -Source https://api.nuget.org/v3/index.json
```

## Template Parameters

The template defines the following parameters:

### ProjectName
- **Description:** Project name (replaces all "MyApp" occurrences)
- **Default:** MyProject
- **Usage:** `--name MyProjectName`

### Company
- **Description:** Company/Organization name
- **Default:** Company
- **Usage:** `--company MyCompany`

## File Name Replacements

All file and folder names with "MyApp" are automatically replaced with the project name:

- `MyApp.Domain/` → `MyProject.Domain/`
- `MyApp.Application/` → `MyProject.Application/`
- `MyApp.Infrastructure/` → `MyProject.Infrastructure/`
- `MyApp.Persistence/` → `MyProject.Persistence/`
- `MyApp.WebAPI/` → `MyProject.WebAPI/`

## Namespace Replacements

C# namespaces are automatically updated in all files:

- `using MyApp.Domain;` → `using MyProject.Domain;`
- `namespace MyApp.WebAPI` → `namespace MyProject.WebAPI`

## Template Development

To modify the template:

1. Make changes to project files
2. Update parameters in `.template.config/template.json`
3. Test locally
4. Recreate package

## Troubleshooting

### Templates cannot be created after installation
```bash
# Clear template cache
dotnet new --search onion --detailed

# Reinstall template
dotnet new uninstall MyApp.Solution
dotnet new install ./dist/MyApp.Solution.1.0.0.nupkg
```

### File names not changing
Verify that `fileRename` property is correctly set in template.json

### Namespaces not changing
Ensure "MyApp" appears in C# files. Check with grep:
```bash
grep -r "MyApp" src/
```

## License

MIT - Use freely as you wish.

## Contact

Please open an issue for questions and suggestions.

