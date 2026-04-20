# TimeManagement Platform - Deployment Guide

## Runtime configuration

Important settings:

- `ASPNETCORE_ENVIRONMENT`
- `ConnectionStrings__DefaultConnection`
- `Database__EnsureCreated`
- `Database__AutoMigrate`
- `Demo__SeedEnabled`
- `DEMO_EMAIL`, `DEMO_PASSWORD`
- `DisableHttpsRedirect`

## Container deployment

```bash
cp docker/.env.example docker/.env
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d --build
```

Start already-built containers:

```bash
docker compose --env-file docker/.env -f docker/docker-compose.yml up -d
```

Default ports:

- App: `8011` (`APP_PORT`)
- Postgres: `8012` (`PG_PORT`)

## HTTPS behavior

- Docker compose sets `DisableHttpsRedirect=true` for local HTTP.
- Non-development deployments enable HSTS by default.

## Production notes

- Use secure database credentials and strong demo credentials or disable demo seeding.
- Decide whether automatic migration is acceptable for your release process.
