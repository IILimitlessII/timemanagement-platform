# TimeManagement Platform - Architecture

## Solution layout

- `src/TimemanagementPlatform.Web` - ASP.NET Core MVC + Identity application.
- `src/TimemanagementPlatform.Application` - application-layer logic.
- `src/TimemanagementPlatform.Domain` - domain model layer.
- `src/TimemanagementPlatform.Infrastructure` - infrastructure concerns.
- `tests` - automated tests.

## Current layering note (pre-split)

The Application/Domain/Infrastructure projects are currently transition shells for the cleaned portfolio layout.  
`Domain` and `Infrastructure` still compile selected source files from `Web` via `Compile Include` entries in their `.csproj` files.  
This keeps the working runtime intact for now and avoids high-risk refactoring in the pre-publish pass.

## Web project structure

Within `src/TimemanagementPlatform.Web`:

- `Controllers` and `Views` - MVC features.
- `Areas/Identity` - authentication/account pages.
- `Data` and `Data/Migrations` - EF Core context and migrations.
- `Models` - domain/view models for modules, semesters, and tracking.
- `Services` - integration helpers (including identity email behavior).
- `wwwroot` - static assets.

## Runtime flow

1. Configure PostgreSQL DbContext from `DefaultConnection`.
2. Configure Identity and MVC services.
3. Initialize database at startup (`EnsureCreated` or `Migrate`).
4. Optionally seed demo user and sample portfolio data.
5. Start middleware pipeline and MVC routes.
