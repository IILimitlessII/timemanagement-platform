# Migrations

This project now uses **Postgres-only** migrations for Docker/recruiter demos.

The initial migration set has been generated (`InitialPostgres`) and should be applied with `Database:AutoMigrate=true` and `Database:EnsureCreated=false`.

To add more migrations later:

```bash
dotnet tool restore
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

For the standardized docs set, see `../../../../docs/README-Database.md`.

