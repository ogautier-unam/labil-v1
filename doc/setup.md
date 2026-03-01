# CrisisConnect — Setup Guide

## Prerequisites

| Tool | Version | Check |
|------|---------|-------|
| .NET SDK | 10.0+ | `dotnet --version` |
| Docker | 24+ | `docker --version` |
| Docker Compose | v2 | `docker compose version` |
| dotnet-ef | 10.x | `dotnet ef --version` |

### Install / Update dotnet-ef
```bash
dotnet tool update --global dotnet-ef
```

## Quick Start (Docker)

```bash
# 1. Clone the repository
git clone <repo-url>
cd labil-v1

# 2. Create environment file
cp .env.example .env
# Edit .env with your secrets (JWT_SECRET must be >= 32 characters)

# 3. Start all services (DB + API + Web)
docker compose up -d

# 4. Check services are healthy
docker compose ps

# 5. Access the application
# Web UI:  http://localhost:8081
# API:     http://localhost:8080
# Swagger: http://localhost:8080/swagger  (Development only)
```

## Local Development (without Docker API)

```bash
# 1. Start only the database
docker compose up -d db

# 2. Apply migrations
dotnet ef database update \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# 3. Run the API (hot reload)
dotnet watch run --project src/CrisisConnect.API

# 4. Run the Web frontend (in another terminal)
dotnet watch run --project src/CrisisConnect.Web
```

## Environment Variables

Create `.env` from `.env.example`:

```env
POSTGRES_DB=crisisconnect
POSTGRES_USER=crisisconnect_user
POSTGRES_PASSWORD=<your-password>
DB_CONNECTION_STRING=Host=db;Database=crisisconnect;Username=crisisconnect_user;Password=<your-password>
JWT_SECRET=<minimum-32-character-secret>
JWT_ISSUER=CrisisConnect
JWT_AUDIENCE=CrisisConnectUsers
```

**Never commit `.env`** — it is in `.gitignore`.

## Database Migrations

```bash
# List existing migrations
dotnet ef migrations list \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Create a new migration
dotnet ef migrations add <MigrationName> \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Apply pending migrations
dotnet ef database update \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Roll back to a specific migration
dotnet ef database update <TargetMigration> \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API
```

## Build & Test

```bash
# Build entire solution
dotnet build

# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Build only (no restore)
dotnet build --no-restore
```

## Project Structure

```
labil-v1/
├── CrisisConnect.sln
├── CLAUDE.md              ← AI assistant instructions
├── doc/                   ← This documentation
│   ├── architecture.md
│   ├── api-reference.md
│   └── setup.md
├── src/
│   ├── CrisisConnect.Domain/
│   ├── CrisisConnect.Application/
│   ├── CrisisConnect.Infrastructure/
│   ├── CrisisConnect.API/
│   └── CrisisConnect.Web/
├── tests/
│   ├── CrisisConnect.Domain.Tests/
│   ├── CrisisConnect.Application.Tests/
│   └── CrisisConnect.Infrastructure.Tests/
├── docker-compose.yml
├── docker-compose.override.yml
├── .env.example
└── .gitignore
```

## Default User Registration

The API does not seed default users. Register via:
- **Web UI**: `http://localhost:8081/Auth/Register`
- **API**: `POST /api/auth/register`

```json
{
  "email": "admin@example.com",
  "motDePasse": "Password123!",
  "prenom": "Admin",
  "nom": "Responsable",
  "role": "Responsable"
}
```

Available roles: `Citoyen` | `Benevole` | `Coordinateur` | `Responsable`

## Docker Services

| Service | Port | Description |
|---------|------|-------------|
| `db` | 5432 | PostgreSQL 17-alpine |
| `api` | 8080 | ASP.NET Core REST API |
| `web` | 8081 | ASP.NET Core Razor Pages |

```bash
# View API logs
docker compose logs -f api

# Stop everything
docker compose down

# Stop + reset database (WARNING: destroys all data)
docker compose down -v
```

## Health Check

The API exposes a health endpoint for Docker/Kubernetes:

```
GET http://localhost:8080/health
→ 200 OK  (when healthy)
```

## Troubleshooting

### `dotnet ef` command not found
```bash
dotnet tool install --global dotnet-ef
# or
dotnet tool update --global dotnet-ef
```

### PostgreSQL connection refused
```bash
# Check container is running and healthy
docker compose ps
docker compose logs db
```

### Migration fails on first run
The `[ERR] Failed executing DbCommand` on first `database update` is **normal** — EF Core
creates the `__EFMigrationsHistory` table on the first run. Look for `Done.` at the end.

### API returns 401 on all requests
Check that `JWT_SECRET` in `.env` matches between runs and is at least 32 characters long.
