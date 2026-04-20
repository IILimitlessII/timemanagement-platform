# TimeManagement Platform

Cleaned project layout:

- `src/` runtime projects (`TimemanagementPlatform.Web`, `Application`, `Domain`, `Infrastructure`)
- `tests/` test project
- `docker/` Docker compose and env files
- `docs/` setup and operational notes

## Prerequisites

- .NET SDK 10.x (`global.json` pins `10.0.100`)
- Docker + Docker Compose (for container workflow)

## Quick start

From this folder:

```bash
dotnet restore TimemanagementPlatform.sln
dotnet build TimemanagementPlatform.sln
dotnet run --project src/TimemanagementPlatform.Web/TimemanagementPlatform.Web.csproj
```

## Docker quick start

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

App URL: `http://localhost:8011` (default)

## Docker start (already built)

When images are already built and you only need detached startup:

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

## Demo credentials

- Email: `demo@timemanagement.local`
- Password: `Password@123`

You can override these with `DEMO_EMAIL` and `DEMO_PASSWORD` in `docker/.env`.

## Documentation

See `docs/README.md` for the standardized documentation index.
