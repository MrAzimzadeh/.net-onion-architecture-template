# 🧅 Onion Architecture Template

A production-ready **Onion Architecture** template for building scalable, maintainable .NET 9.0 applications following **Clean Architecture** principles with **CQRS**, **Domain-Driven Design (DDD)**, and modern best practices.

[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Architecture](https://img.shields.io/badge/architecture-Onion-green.svg)](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/)

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Features](#-features)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Using as a Template](#-using-as-a-template)
- [Dependency Injection & IoC](#-dependency-injection--ioc)
- [How It Works](#-how-it-works)
- [Configuration](#-configuration)
- [Development](#-development)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

This template implements **Onion Architecture** (also known as Clean Architecture), which emphasizes:

- **Separation of Concerns**: Each layer has a distinct responsibility
- **Dependency Inversion**: Dependencies point inward toward the domain
- **Testability**: Business logic is isolated and easily testable
- **Maintainability**: Changes in one layer don't affect others
- **Scalability**: Easy to extend and modify

### Core Principles

1. **Domain-Centric Design**: Business logic is at the center
2. **Framework Independence**: Core business logic doesn't depend on frameworks
3. **Database Independence**: Can switch databases without affecting business logic
4. **UI Independence**: Business logic doesn't know about the presentation layer
5. **Testable**: Business rules can be tested without UI, database, or external services

---

## 🏗️ Architecture

The architecture follows the **Onion Architecture** pattern with clear layer separation:

```
┌─────────────────────────────────────────────────────────┐
│                   Presentation Layer                     │
│                    (WebAPI, UI)                          │
├─────────────────────────────────────────────────────────┤
│                 Infrastructure Layer                     │
│         (External Services, Email, Auth, etc.)           │
├─────────────────────────────────────────────────────────┤
│                  Persistence Layer                       │
│          (Database, Repositories, Migrations)            │
├─────────────────────────────────────────────────────────┤
│                  Application Layer                       │
│        (Use Cases, DTOs, Interfaces, CQRS)               │
├─────────────────────────────────────────────────────────┤
│                    Domain Layer                          │
│              (Entities, Business Rules)                  │
└─────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

#### 1. **Domain Layer** (Core)
- Contains enterprise business rules and entities
- No dependencies on other layers
- Pure business logic without framework concerns
- Entities: `User`, `Role`, `Policy`, `Tenant`, etc.

#### 2. **Application Layer** (Core)
- Contains application business rules
- Defines interfaces for external services
- Implements CQRS pattern with MediatR
- Features: Commands, Queries, DTOs, Validators
- Dependencies: Domain Layer only

#### 3. **Persistence Layer** (Infrastructure)
- Data access implementation
- Uses **Dapper** for queries (performance)
- Uses **Entity Framework Core** for migrations
- Implements Repository and Unit of Work patterns
- PostgreSQL database with FluentMigrator

#### 4. **Infrastructure Layer** (Infrastructure)
- External service implementations
- JWT Authentication & Authorization
- Mail Service (with RabbitMQ)
- File Storage (Local, Azure, MinIO)
- Third-party integrations

#### 5. **Presentation Layer**
- ASP.NET Core Web API
- Controllers, Middleware, Filters
- Swagger/OpenAPI documentation
- SignalR real-time communication

---

## ✨ Features

### Core Features

- ✅ **Onion Architecture** with clear layer separation
- ✅ **CQRS Pattern** using MediatR
- ✅ **Repository Pattern** with Unit of Work
- ✅ **Domain-Driven Design** principles
- ✅ **Dependency Injection** throughout all layers
- ✅ **FluentValidation** for input validation
- ✅ **AutoMapper** for object mapping

### Authentication & Authorization

- 🔐 **JWT Token-based Authentication**
- 🔐 **Dynamic Policy-based Authorization**
- 🔐 **Role-based Access Control (RBAC)**
- 🔐 **Multi-tenant Support**
- 🔐 **Device Session Management**
- 🔐 **External Login Support**

### Data Access

- 🗄️ **Dapper** for high-performance queries
- 🗄️ **Entity Framework Core** for migrations
- 🗄️ **PostgreSQL** database
- 🗄️ **FluentMigrator** for version-controlled migrations
- 🗄️ **CQRS** with separate Read/Write repositories
- 🗄️ **Database Triggers** with LISTEN/NOTIFY

### Infrastructure Services

- 📧 **Email Service** with RabbitMQ queue
- 📧 **Bulk Email Support** with attachments
- 📁 **File Storage** (Local, Azure Blob, MinIO)
- 🔔 **SignalR** for real-time notifications
- 🐰 **RabbitMQ** message bus
- 📊 **OpenTelemetry** for observability
- 📊 **Jaeger** distributed tracing

### API Features

- 📝 **Swagger/OpenAPI** documentation
- 🌍 **Localization** (multi-language support)
- 🚦 **Rate Limiting**
- 🛡️ **Global Exception Handling**
- 📋 **Structured Logging** with Serilog
- 🔄 **CORS** configuration

---

## 🛠️ Technology Stack

### Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET | 9.0 | Runtime framework |
| C# | 12.0 | Programming language |
| ASP.NET Core | 9.0 | Web framework |

### Libraries & Packages

#### Application Layer
- **MediatR** (12.2.0) - CQRS implementation
- **AutoMapper** (13.0.1) - Object mapping
- **FluentValidation** (11.9.0) - Input validation

#### Persistence Layer
- **Dapper** (2.1.35) - Micro ORM for queries
- **Entity Framework Core** (9.0.0) - Migrations
- **Npgsql** (9.0.0) - PostgreSQL provider
- **FluentMigrator** (5.2.0) - Database migrations

#### Infrastructure Layer
- **JWT Bearer** (9.0.6) - Authentication
- **MailKit** - Email service
- **RabbitMQ.Client** - Message queue
- **Azure.Storage.Blobs** - Cloud storage

#### Presentation Layer
- **Serilog** (8.0.0) - Structured logging
- **Swashbuckle** (6.5.0) - Swagger/OpenAPI
- **OpenTelemetry** (1.10.0) - Observability
- **SignalR** - Real-time communication

### External Services

- **PostgreSQL** - Primary database
- **RabbitMQ** - Message broker
- **Jaeger** - Distributed tracing
- **Redis** (optional) - Caching

---

## 📁 Project Structure

```
OnionArch-Template/
│
├── src/
│   ├── Core/                                    # Core layers (no external dependencies)
│   │   ├── MyApp.Domain/                # Domain entities and business rules
│   │   │   ├── Entities/                        # Domain entities
│   │   │   │   ├── User.cs
│   │   │   │   ├── Role.cs
│   │   │   │   ├── Policy.cs
│   │   │   │   ├── Tenant.cs
│   │   │   │   └── Common/                      # Base entities
│   │   │   └── MyApp.Domain.csproj
│   │   │
│   │   └── MyApp.Application/           # Application business logic
│   │       ├── Common/                          # Shared application code
│   │       │   ├── Interfaces/                  # Service interfaces
│   │       │   ├── DTOs/                        # Data transfer objects
│   │       │   ├── Enums/                       # Application enums
│   │       │   └── Services/                    # Common services
│   │       ├── Features/                        # CQRS features
│   │       │   ├── Auth/                        # Authentication features
│   │       │   │   ├── Commands/                # Write operations
│   │       │   │   ├── Queries/                 # Read operations
│   │       │   │   └── Validators/              # Input validators
│   │       │   ├── Roles/                       # Role management
│   │       │   ├── Mail/                        # Email features
│   │       │   └── Storage/                     # File storage
│   │       ├── Resources/                       # Localization resources
│   │       ├── Triggers/                        # Entity triggers
│   │       ├── DependencyInjection.cs           # Application DI
│   │       └── MyApp.Application.csproj
│   │
│   ├── Infrastructure/                          # External concerns
│   │   ├── MyApp.Persistence/           # Data access
│   │   │   ├── Database/                        # Database context
│   │   │   │   ├── AppDbContext.cs              # EF Core context
│   │   │   │   ├── DapperContext.cs             # Dapper context
│   │   │   │   └── Migrations/                  # FluentMigrator migrations
│   │   │   ├── Repositories/                    # Repository implementations
│   │   │   │   ├── Read/                        # Query repositories (CQRS)
│   │   │   │   └── Write/                       # Command repositories (CQRS)
│   │   │   ├── Workers/                         # Background workers
│   │   │   ├── DependencyInjection.cs           # Persistence DI
│   │   │   └── MyApp.Persistence.csproj
│   │   │
│   │   ├── MyApp.Infrastructure/        # External services
│   │   │   ├── Authentication/                  # JWT implementation
│   │   │   ├── Services/                        # Service implementations
│   │   │   │   ├── MailService.cs               # Email service
│   │   │   │   ├── JwtTokenService.cs           # Token generation
│   │   │   │   └── Storage/                     # File storage providers
│   │   │   ├── Messaging/                       # RabbitMQ implementation
│   │   │   ├── BackgroundServices/              # Hosted services
│   │   │   ├── InfrastructureDependencyInjection.cs
│   │   │   └── MyApp.Infrastructure.csproj
│   │   │
│   │   └── MyApp.SignalR/               # Real-time communication
│   │       ├── Hubs/                            # SignalR hubs
│   │       ├── DependencyInjection.cs           # SignalR DI
│   │       └── MyApp.SignalR.csproj
│   │
│   └── Presentation/                            # User interface layer
│       └── MyApp.WebAPI/                # REST API
│           ├── Controllers/                     # API endpoints
│           ├── Middlewares/                     # Custom middleware
│           ├── Extensions/                      # Extension methods
│           ├── Program.cs                       # Application entry point
│           ├── appsettings.json                 # Configuration
│           └── MyApp.WebAPI.csproj
│
├── .template.config/                            # Template configuration
│   ├── template.json                            # Template metadata
│   ├── TEMPLATE_GUIDE.md                        # Template usage guide
│   └── DEVELOPER_GUIDE.md                       # Developer documentation
│
├── docker-compose.yml                           # Docker services
├── MyApp.sln                            # Solution file
├── LICENSE                                      # MIT License
└── README.md                                    # This file
```

---

## 🚀 Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [PostgreSQL](https://www.postgresql.org/download/) 12+ or Docker
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for RabbitMQ & Jaeger)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/), or [JetBrains Rider](https://www.jetbrains.com/rider/)

### Local Development Setup

#### 1. Clone the Repository

```bash
git clone https://github.com/MrAzimzadeh/OnionArch-Template.git
cd OnionArch-Template
```

#### 2. Start External Services (Docker)

Start RabbitMQ and Jaeger using Docker Compose:

```bash
docker-compose up -d
```

This will start:
- **RabbitMQ**: Message broker (ports 5672, 15672)
- **Jaeger**: Distributed tracing UI (port 16686)

#### 3. Configure Database Connection

Update the connection string in `src/Presentation/MyApp.WebAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=MyAppDb;Username=your_user;Password=your_password"
  }
}
```

#### 4. Restore Dependencies

```bash
dotnet restore
```

#### 5. Run Database Migrations

The application automatically creates the database and runs migrations on startup. Alternatively, you can run migrations manually:

```bash
cd src/Presentation/MyApp.WebAPI
dotnet run
```

#### 6. Run the Application

```bash
cd src/Presentation/MyApp.WebAPI
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

#### 7. Access Services

- **API**: https://localhost:5001/swagger
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **Jaeger UI**: http://localhost:16686

---

## 🎨 Using as a Template

This project is designed to be used as a **dotnet template** for creating new projects.

### Option 1: Install from Local Source

#### Step 1: Install the Template

Navigate to the template directory and install it locally:

```bash
cd /path/to/OnionArch-Template
dotnet new install ./
```

#### Step 2: Create a New Project

Create a new project using the template:

```bash
dotnet new onion --name MyAwesomeProject --company MyCompany
```

**Parameters:**
- `--name` or `-n`: Your project name (replaces all "MyApp" occurrences)
- `--company` or `-c`: Your company/organization name (optional)

**Example:**

```bash
dotnet new onion --name Acme.ECommerce --company Acme
```

This will:
- ✅ Create a new solution with your project name
- ✅ Rename all namespaces (MyApp → Acme.ECommerce)
- ✅ Rename all folders and files
- ✅ Update all project references
- ✅ Update configuration files

#### Step 3: Navigate and Restore

```bash
cd MyAwesomeProject
dotnet restore
dotnet build
```

### Option 2: Install from NuGet (Future)

Once published to NuGet:

```bash
dotnet new install MyApp.Solution
dotnet new onion --name MyProject --company MyCompany
```

### Option 3: Manual Clone and Rename

1. Clone the repository
2. Manually rename all occurrences of "MyApp" to your project name
3. Update namespaces, folder names, and project references

### Uninstall Template

To remove the installed template:

```bash
dotnet new uninstall MyApp.Solution
```

Or if installed locally:

```bash
dotnet new uninstall /path/to/OnionArch-Template
```

---

## 🔌 Dependency Injection & IoC

This template follows the **Dependency Inversion Principle** with a clean IoC (Inversion of Control) container setup.

### How IoC Works in This Template

Each layer registers its dependencies in a dedicated `DependencyInjection.cs` file:

#### 1. **Application Layer** (`Application/DependencyInjection.cs`)

```csharp
public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    // MediatR for CQRS
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    
    // AutoMapper for object mapping
    services.AddAutoMapper(assembly);
    
    // FluentValidation for input validation
    services.AddValidatorsFromAssembly(assembly);
    
    // Entity Triggers
    services.AddScoped<IEntityTrigger, UserTrigger>();
    
    // Localization Service
    services.AddScoped<ILocalizationService, LocalizationService>();
    
    return services;
}
```

**Registered Services:**
- **MediatR**: Command/Query handlers
- **AutoMapper**: DTO mappings
- **FluentValidation**: Input validators
- **Localization**: Multi-language support
- **Entity Triggers**: Domain event handlers

#### 2. **Persistence Layer** (`Persistence/DependencyInjection.cs`)

```csharp
public static IServiceCollection AddPersistenceServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    
    // EF Core DbContext (for migrations only)
    services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
    
    // Dapper for queries
    services.AddSingleton<DapperContext>();
    services.AddSingleton<IDbConnectionFactory, PostgresConnectionFactory>();
    
    // FluentMigrator for migrations
    services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(assembly).For.Migrations());
    
    // CQRS Repositories
    services.AddScoped<IUserReadRepository, UserReadRepository>();
    services.AddScoped<IUserWriteRepository, UserWriteRepository>();
    
    // Unit of Work
    services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
    
    // Background Worker for DB notifications
    services.AddHostedService<PostgresNotificationWorker>();
    
    return services;
}
```

**Registered Services:**
- **DbContext**: EF Core for migrations
- **Dapper**: High-performance queries
- **Repositories**: CQRS read/write repositories
- **Unit of Work**: Transaction management
- **Background Workers**: Database LISTEN/NOTIFY

#### 3. **Infrastructure Layer** (`Infrastructure/InfrastructureDependencyInjection.cs`)

```csharp
public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    // JWT Authentication
    services.AddAuthentication("Bearer")
        .AddScheme<AuthenticationSchemeOptions, ProjectAuthHandler>("Bearer", null);
    
    // JWT Token Service
    services.AddScoped<IJwtTokenService, JwtTokenService>();
    
    // Authentication & Password
    services.AddScoped<IAuthHelper, AuthHelper>();
    services.AddScoped<IPasswordHandler, PasswordHandler>();
    
    // Dynamic Authorization
    services.AddScoped<IPolicyChecker, PolicyChecker>();
    services.AddScoped<IAuthorizationHandler, PolicyAuthorizationHandler>();
    services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
    
    // Mail Service
    services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
    services.AddTransient<IMailService, MailService>();
    
    // RabbitMQ Message Bus
    services.AddSingleton<IMessageBus, RabbitMQBus>();
    services.AddHostedService<MailConsumer>();
    
    return services;
}
```

**Registered Services:**
- **Authentication**: JWT token generation and validation
- **Authorization**: Dynamic policy-based authorization
- **Mail Service**: Email sending with queue
- **Message Bus**: RabbitMQ for async messaging
- **Background Services**: Mail consumer

#### 4. **SignalR Layer** (`SignalR/DependencyInjection.cs`)

```csharp
public static IServiceCollection AddSignalRServices(this IServiceCollection services)
{
    services.AddSignalR();
    return services;
}
```

### Main Application Composition (`Program.cs`)

All layers are composed in the `Program.cs` file:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Serilog & OpenTelemetry
builder.ConfigureSerilog();
builder.AddCustomOpenTelemetry();

// Layer Registration (Order matters!)
builder.Services.AddApplicationServices();           // 1. Application
builder.Services.AddPersistenceServices(config);     // 2. Persistence
builder.Services.AddInfrastructureServices(config);  // 3. Infrastructure
builder.Services.AddSignalRServices();               // 4. SignalR

// Additional configurations
builder.Services.AddCustomSwagger();
builder.Services.AddCustomRateLimiting();
builder.Services.AddLocalizationConfig();
builder.Services.AddExceptionConfig();
builder.Services.AddStorage(StorageType.Local);

var app = builder.Build();

// Middleware pipeline
app.UseExceptionConfig();
app.UseCustomSwagger();
app.UseHttpsRedirection();
app.UseCustomRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.UseLocalizationConfig();
app.MapControllers();
app.MapHub<UserHub>("/hubs/user");
app.UseAppCors();

app.Run();
```

### Dependency Flow

```
┌─────────────────────────────────────────────────────────┐
│                      Program.cs                          │
│                  (Composition Root)                      │
└────────────────────┬────────────────────────────────────┘
                     │
        ┌────────────┼────────────┬────────────┐
        │            │            │            │
        ▼            ▼            ▼            ▼
  Application   Persistence  Infrastructure  SignalR
  Layer DI      Layer DI     Layer DI        Layer DI
        │            │            │            │
        └────────────┴────────────┴────────────┘
                     │
                     ▼
              IoC Container
           (Built-in .NET DI)
```

### Key IoC Principles Used

1. **Interface Segregation**: Small, focused interfaces
2. **Dependency Inversion**: Depend on abstractions, not concretions
3. **Single Responsibility**: Each service has one clear purpose
4. **Lifetime Management**: Proper scoping (Singleton, Scoped, Transient)
5. **Constructor Injection**: Dependencies injected via constructors

---

## ⚙️ How It Works

### Request Flow

Here's how a typical request flows through the application:

```
1. HTTP Request
   ↓
2. Controller (Presentation Layer)
   ↓
3. MediatR Command/Query (Application Layer)
   ↓
4. Validator (FluentValidation)
   ↓
5. Handler (Application Layer)
   ↓
6. Repository (Persistence Layer)
   ↓
7. Database (PostgreSQL via Dapper)
   ↓
8. Response DTO (AutoMapper)
   ↓
9. HTTP Response
```

### Example: User Login Flow

#### 1. **Controller** (`Presentation/WebAPI/Controllers/AuthController.cs`)

```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginCommand command)
{
    var result = await _mediator.Send(command);
    return Ok(result);
}
```

#### 2. **Command** (`Application/Features/Auth/Commands/LoginCommand.cs`)

```csharp
public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
```

#### 3. **Validator** (`Application/Features/Auth/Validators/LoginCommandValidator.cs`)

```csharp
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
```

#### 4. **Handler** (`Application/Features/Auth/Handlers/LoginCommandHandler.cs`)

```csharp
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserReadRepository _userRepository;
    private readonly IPasswordHandler _passwordHandler;
    private readonly IJwtTokenService _jwtService;

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        // 1. Get user from database
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        // 2. Verify password
        if (!_passwordHandler.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials");
        
        // 3. Generate JWT token
        var token = _jwtService.GenerateToken(user);
        
        // 4. Return response
        return new LoginResponse(token, user.Id);
    }
}
```

#### 5. **Repository** (`Persistence/Repositories/Read/UserReadRepository.cs`)

```csharp
public class UserReadRepository : IUserReadRepository
{
    private readonly DapperContext _context;

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = "SELECT * FROM users WHERE email = @Email";
        return await _context.Connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }
}
```

### CQRS Pattern

The template implements **CQRS (Command Query Responsibility Segregation)**:

#### Commands (Write Operations)

```csharp
// Create User Command
public record CreateUserCommand(string Name, string Email) : IRequest<Guid>;

// Handler
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserWriteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        
        await _repository.AddAsync(user);
        await _unitOfWork.CommitAsync();
        
        return user.Id;
    }
}
```

#### Queries (Read Operations)

```csharp
// Get User Query
public record GetUserQuery(Guid Id) : IRequest<UserDto>;

// Handler
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserReadRepository _repository;
    private readonly IMapper _mapper;

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        return _mapper.Map<UserDto>(user);
    }
}
```

### Database Migrations

The template uses **FluentMigrator** for version-controlled migrations:

```csharp
[Migration(20240101000001)]
public class CreateUsersTable : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("email").AsString(255).NotNullable().Unique()
            .WithColumn("password_hash").AsString(500).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}
```

### Real-time Notifications (SignalR)

```csharp
// Hub
public class UserHub : Hub
{
    public async Task NotifyUserCreated(Guid userId)
    {
        await Clients.All.SendAsync("UserCreated", userId);
    }
}

// Client connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/user")
    .build();

connection.on("UserCreated", (userId) => {
    console.log(`New user created: ${userId}`);
});
```

### Message Queue (RabbitMQ)

```csharp
// Publish message
public class MailService : IMailService
{
    private readonly IMessageBus _messageBus;

    public async Task SendEmailAsync(MailRequest request)
    {
        await _messageBus.PublishAsync("mail.send", request);
    }
}

// Consume message
public class MailConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageBus.SubscribeAsync<MailRequest>("mail.send", async (message) =>
        {
            // Send email logic
            await SendEmailAsync(message);
        });
    }
}
```

---

## 🔧 Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=YourDb;Username=user;Password=pass"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-min-32-characters",
    "Issuer": "YourAPI",
    "Audience": "YourClient",
    "ExpiredHours": 24
  },
  "MailSettings": {
    "Mail": "your-email@gmail.com",
    "DisplayName": "Your App",
    "Password": "your-app-password",
    "Host": "smtp.gmail.com",
    "Port": 587
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "Cors": {
    "Origins": "http://localhost:3000;http://localhost:5173"
  }
}
```

### Environment Variables

You can override settings using environment variables:

```bash
export ConnectionStrings__DefaultConnection="Host=prod-db;Database=ProdDb;..."
export JwtSettings__SecretKey="production-secret-key"
```

### Storage Configuration

Choose your storage provider in `Program.cs`:

```csharp
// Local file storage
builder.Services.AddStorage(StorageType.Local);

// Azure Blob Storage
builder.Services.AddStorage(StorageType.Azure);

// MinIO
builder.Services.AddStorage(StorageType.MinIO);
```

---

## 👨‍💻 Development

### Adding a New Feature

#### 1. Create Entity (Domain Layer)

```csharp
// src/Core/MyApp.Domain/Entities/Product.cs
public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

#### 2. Create DTOs (Application Layer)

```csharp
// src/Core/MyApp.Application/Common/DTOs/ProductDto.cs
public record ProductDto(Guid Id, string Name, decimal Price);
```

#### 3. Create Repository Interface (Application Layer)

```csharp
// src/Core/MyApp.Application/Common/Interfaces/Repositories/IProductRepository.cs
public interface IProductReadRepository
{
    Task<ProductDto?> GetByIdAsync(Guid id);
}
```

#### 4. Implement Repository (Persistence Layer)

```csharp
// src/Infrastructure/MyApp.Persistence/Repositories/Read/ProductReadRepository.cs
public class ProductReadRepository : IProductReadRepository
{
    private readonly DapperContext _context;

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM products WHERE id = @Id";
        return await _context.Connection.QueryFirstOrDefaultAsync<ProductDto>(sql, new { Id = id });
    }
}
```

#### 5. Register in DI

```csharp
// src/Infrastructure/MyApp.Persistence/DependencyInjection.cs
services.AddScoped<IProductReadRepository, ProductReadRepository>();
```

#### 6. Create Query/Command (Application Layer)

```csharp
// src/Core/MyApp.Application/Features/Products/Queries/GetProductQuery.cs
public record GetProductQuery(Guid Id) : IRequest<ProductDto>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IProductReadRepository _repository;

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken ct)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}
```

#### 7. Create Controller (Presentation Layer)

```csharp
// src/Presentation/MyApp.WebAPI/Controllers/ProductsController.cs
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var result = await _mediator.Send(new GetProductQuery(id));
        return Ok(result);
    }
}
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### Code Quality

```bash
# Format code
dotnet format

# Analyze code
dotnet build /p:TreatWarningsAsErrors=true
```

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 📞 Support

For questions, issues, or feature requests:

- **GitHub Issues**: [Create an issue](https://github.com/MrAzimzadeh/OnionArch-Template/issues)
- **Discussions**: [GitHub Discussions](https://github.com/MrAzimzadeh/OnionArch-Template/discussions)

---

## 🙏 Acknowledgments

- Inspired by [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) by Robert C. Martin
- [Onion Architecture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/) by Jeffrey Palermo
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html) by Martin Fowler

---

## 🌟 Star History

If you find this template useful, please consider giving it a ⭐ on GitHub!

---

**Built with ❤️ by [MrAzimzadeh](https://github.com/MrAzimzadeh)**# .Net-Onion-Example-Template
# .net-onion-architecture-template
