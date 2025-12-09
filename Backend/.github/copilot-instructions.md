# GitHub Copilot Instructions for StudentRegistration

## Project Overview
This is a .NET 10 Web API project following **Clean Architecture** principles. It manages student registrations, class enrollments, and credit programs.

## Architecture & Structure
The solution is organized into four layers:
- **StudentRegistration.Api**: Entry point, Controllers, Configuration (`Program.cs`), and Exception Handling.
- **StudentRegistration.Application**: Business logic definitions (Interfaces), DTOs, Validators (FluentValidation), and pure logic services (`TokenService`).
- **StudentRegistration.Domain**: Core entities, Enums, and common base classes (`BaseEntity`).
- **StudentRegistration.Infrastructure**: Data access (`ApplicationDbContext`), Migrations, and Service Implementations (`AuthService`, `StudentService`, `EnrollmentService`).

### Key Patterns
- **Service Implementation Location**: Most business services (`AuthService`, `StudentService`, `EnrollmentService`) are implemented in **Infrastructure**, not Application. Only `TokenService` is in Application.
- **Data Access**: Services inject `ApplicationDbContext` directly. The Repository pattern is **NOT** used.
- **Validation**: `FluentValidation` is used for DTOs. Validators are registered automatically in `Program.cs`.
- **Authentication**: JWT Bearer authentication. `TokenService` generates tokens.
- **Database**: SQL Server LocalDB. Entity configurations are in `ApplicationDbContext.OnModelCreating` using Fluent API.

## Development Workflow
- **Startup**: Running the API (`dotnet run` in `StudentRegistration.Api`) automatically:
  1. Creates the database if it doesn't exist.
  2. Applies pending migrations.
  3. Seeds initial data (Programs, Subjects, Professors, Offerings).
- **Frontend Integration**: CORS is configured to allow `http://localhost:4200` (Angular).

## Coding Conventions
- **DTOs**: Use Records or Classes in `Application/DTOs`. Group by feature (e.g., `DTOs/Auth`, `DTOs/Student`).
- **Controllers**: Keep thin. Delegate logic to Services. Return `ActionResult<T>`.
- **Entities**: Inherit from `BaseEntity` (adds `Id`, `CreatedAt`, `UpdatedAt`).
- **Dependency Injection**: Register services as `Scoped` in `Program.cs`.

## Critical Files
- `StudentRegistration.Api/Program.cs`: Service registration, DB config, Auth setup.
- `StudentRegistration.Infrastructure/Data/ApplicationDbContext.cs`: Entity configurations.
- `StudentRegistration.Infrastructure/Services/*`: Core business logic implementations.
- `StudentRegistration.Application/Validators/*`: Input validation rules.

## Common Tasks
- **Adding a new Entity**:
  1. Create class in `Domain/Entities`.
  2. Add `DbSet` to `ApplicationDbContext`.
  3. Configure in `OnModelCreating`.
  4. Create Migration: `dotnet ef migrations add Name -p StudentRegistration.Infrastructure -s StudentRegistration.Api`.
- **Adding a Service**:
  1. Define Interface in `Application/Interfaces`.
  2. Implement in `Infrastructure/Services`.
  3. Register in `Program.cs`.
