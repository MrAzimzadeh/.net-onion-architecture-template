# MyApp NuGet Template Guide

## Installation

To install the template from NuGet:

```bash
dotnet new install MyApp.Solution
```

## Usage

To create a new project:

```bash
dotnet new onion --name MyProjectName --company MyCompanyName
```

Parameters:
- `--name`: Project name (default: MyProject). This name will replace all "MyApp" occurrences.
- `--company`: Company/Organization name (default: MyCompany). This name will replace "Company.Project" occurrences.

Example:

```bash
dotnet new onion --name Acme.ECommerce --company Acme
```

This command will:
- Rename all folders (MyApp → Acme.ECommerce)
- Update all C# namespaces
- Fix project references
- Adapt configuration files

## Project Structure

```
MyProjectName/
├── src/
│   ├── Core/
│   │   ├── MyProjectName.Domain/
│   │   └── MyProjectName.Application/
│   ├── Infrastructure/
│   │   ├── MyProjectName.Infrastructure/
│   │   └── MyProjectName.Persistence/
│   └── Presentation/
│       └── MyProjectName.WebAPI/
├── docs/
├── docker-compose.yml
├── MyProjectName.sln
└── README.md
```

## Next Steps

1. Configure database connection in `appsettings.json`
2. Secure your JWT key
3. Customize `docker-compose.yml` for your needs
4. Add domain-specific models and features

## Support

For more information, see [docs/](../../docs/) folder.

## License

MIT

