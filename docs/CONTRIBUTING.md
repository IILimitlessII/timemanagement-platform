# Contributing

## Local setup

Prerequisites:

- .NET SDK 10.x (repo `global.json` pins `10.0.100`)

```bash
dotnet restore TimemanagementPlatform.sln
dotnet build TimemanagementPlatform.sln
dotnet run --project src/TimemanagementPlatform.Web/TimemanagementPlatform.Web.csproj
```

For containerized setup, follow `README-Docker.md`.

## Contribution workflow

1. Create a focused branch per change.
2. Keep commits small and reviewable.
3. Run relevant build/tests before requesting review.
4. Update docs when behavior/configuration changes.

## Database changes

- Add EF migrations for schema updates.
- Keep `README-Database.md` current with migration instructions.

## Pull request checklist

- Project builds successfully.
- Relevant tests pass.
- New settings and env vars are documented.
- Links to docs remain valid.
