# TimeManagement Platform - Docker

Run the app with PostgreSQL, migration-first startup, and optional seeded demo login.

## Quick start

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

Open `http://localhost:8011` (or your `APP_PORT` value).

## Start existing containers (detached)

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

## Environment variables

| Variable | Purpose |
| --- | --- |
| `DB_NAME` | Database name (default `timemanagement`) |
| `DB_USER` | Database user (default `timemanagement`) |
| `DB_PASSWORD` | Database password |
| `APP_PORT` | Host app port (default `8011`) |
| `PG_PORT` | Host PostgreSQL port (default `8012`) |
| `DEMO_EMAIL` | Seeded demo user email |
| `DEMO_PASSWORD` | Seeded demo user password |

## Startup behavior

On first startup in Docker:

- PostgreSQL waits for healthy state.
- The app applies migrations (`Database__AutoMigrate=true`).
- Demo user seeding runs when `Demo__SeedEnabled=true`.

Compose sets `DisableHttpsRedirect=true` for local HTTP operation in containers.

## Notes

- A no-op `IEmailSender` is registered so Identity flows run without SMTP setup.
- You can override all defaults through `docker/.env`.

