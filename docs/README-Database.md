# TimeManagement Platform - Database Guide

## Database engine

- Primary engine: PostgreSQL.
- Docker service: `timemanagementdb`.

## Connection and startup settings

Key settings:

- `ConnectionStrings:DefaultConnection`
- `Database:EnsureCreated` (typically `false` for migration-first flows)
- `Database:AutoMigrate` (typically `true`)

Current default configuration is migration-first (`EnsureCreated=false`, `AutoMigrate=true`).

## Migration workflow

From `src/TimemanagementPlatform.Web`:

```bash
dotnet tool restore
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Docker database variables

- `DB_NAME`
- `DB_USER`
- `DB_PASSWORD`
- `PG_PORT`

## Demo seed behavior

When enabled (`Demo:SeedEnabled` or `Demo__SeedEnabled`), startup can create a demo user and starter portfolio data using:

- `DEMO_EMAIL`
- `DEMO_PASSWORD`
