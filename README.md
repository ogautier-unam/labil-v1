# CrisisConnect

**Plateforme de gestion de crise** — coordination de bénévoles lors d'événements d'urgence (catastrophes naturelles, crises sanitaires, etc.)

Projet académique IHDCM032 — LABIL

---

## Sommaire

- [Prérequis](#prérequis)
- [Démarrage rapide (Docker)](#démarrage-rapide-docker)
- [Développement local](#développement-local)
- [Architecture](#architecture)
- [Fonctionnalités](#fonctionnalités)
- [Tests](#tests)
- [API — Documentation Swagger](#api--documentation-swagger)

---

## Prérequis

| Outil | Version minimale |
|-------|-----------------|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0 |
| [Docker](https://www.docker.com/) | 24+ |
| [Docker Compose](https://docs.docker.com/compose/) | v2 (`docker compose`) |
| [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) | 10.x |

```bash
# Installer / mettre à jour l'outil EF Core CLI
dotnet tool update --global dotnet-ef
```

---

## Démarrage rapide (Docker)

```bash
# 1. Cloner le dépôt
git clone <url-du-repo>
cd labil-v1

# 2. Configurer les variables d'environnement
cp .env.example .env
# Éditer .env : modifier au minimum POSTGRES_PASSWORD et JWT_SECRET

# 3. Lancer les 3 services (DB + API + Web)
docker compose up -d

# 4. Appliquer les migrations (première fois uniquement)
dotnet ef database update \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API
```

Une fois démarré :

| Service | URL |
|---------|-----|
| Interface Web (Razor Pages) | http://localhost:8081 |
| API REST | http://localhost:8080 |
| Swagger / OpenAPI | http://localhost:8080/swagger *(Development uniquement)* |
| PostgreSQL | localhost:5432 |
| Health check | http://localhost:8080/health |

---

## Développement local

### Lancer uniquement la base de données

```bash
docker compose up -d db
```

### Lancer l'API avec hot reload

```bash
dotnet watch run --project src/CrisisConnect.API
# API disponible sur http://localhost:5072
# Swagger : http://localhost:5072/swagger
```

### Lancer le front-end Web

```bash
dotnet watch run --project src/CrisisConnect.Web
# Web disponible sur http://localhost:5073
```

> Le projet Web s'attend à trouver l'API sur `http://localhost:5072` en développement
> (configurable dans `src/CrisisConnect.Web/appsettings.Development.json` → `ApiSettings:BaseUrl`).

### Migrations EF Core

```bash
# Créer une migration
dotnet ef migrations add NomMigration \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Appliquer les migrations
dotnet ef database update \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Lister les migrations
dotnet ef migrations list \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API
```

---

## Architecture

Le projet suit les principes de la **Clean Architecture** avec séparation stricte des couches :

```
CrisisConnect.sln
├── src/
│   ├── CrisisConnect.Domain/          ← Entités, Value Objects, Enums, Interfaces
│   ├── CrisisConnect.Application/     ← Use Cases (CQRS/MediatR), DTOs, Validators
│   ├── CrisisConnect.Infrastructure/  ← EF Core, Repositories, Services externes
│   ├── CrisisConnect.API/             ← Controllers REST, Middleware, Swagger
│   └── CrisisConnect.Web/             ← Interface Razor Pages, ApiClient, Bootstrap 5.3
└── tests/
    ├── CrisisConnect.Domain.Tests/
    ├── CrisisConnect.Application.Tests/
    └── CrisisConnect.Infrastructure.Tests/
```

**Règles de dépendance :**

```
API   → Application → Domain
Infra → Application → Domain
Web   → API REST (via HttpClient — aucune référence directe aux autres projets)
```

### Stack technique

| Couche | Technologies |
|--------|-------------|
| Domaine | C# 13, .NET 10, aucune dépendance externe |
| Application | MediatR 14, FluentValidation 12, AutoMapper 16 |
| Infrastructure | EF Core 10, Npgsql 10, BCrypt.Net, JWT Bearer |
| API | ASP.NET Core 10, Swashbuckle (Swagger), Serilog |
| Web | Razor Pages 10, Bootstrap 5.3.3, Cookie Auth |
| Base de données | PostgreSQL 17 (nommage snake_case via EFCore.NamingConventions) |

---

## Fonctionnalités

### Acteurs

| Rôle | Droits principaux |
|------|------------------|
| `Citoyen` / `Bénévole` | Créer des offres/demandes, initier des transactions |
| `Coordinateur` | Gérer le cycle de vie des propositions, générer des suggestions, attribuer des rôles |
| `Responsable` | Supervision globale, gestion des organisations, mandats, configuration de crise |

### Modules principaux

- **Offres & Demandes** — Proposition abstraite (Composite pattern) avec cycle de vie complet : Active → EnAttenteRelance / EnTransaction → Archivée / Clôturée
- **Transactions** — Initiation, discussion (messages), confirmation ou annulation entre acteurs
- **Panier** — Sélection multi-offres pour une demande ; confirmation groupée
- **Authentification** — JWT (HS256, 1h access + 7j refresh) stocké en cookie HttpOnly ; rôles `Citoyen`, `Benevole`, `Coordinateur`, `Responsable`
- **Notifications** — 8 types métier (`TypeNotification`), marquage comme lue
- **Journal d'audit** — `EntreeJournal` avec `TypeOperation` (28 valeurs), tracé automatique via pipeline MediatR (`AuditBehaviour`)
- **Suggestions d'appariement** — Score Jaccard + bonus urgence/livraison, génération à la demande [Coordinateur+]
- **Configuration de crise** — `ConfigCatastrophe` activable/désactivable, taxonomie des catégories (Composite self-ref)
- **Rôles & Mandats** — Attribution temporelle de rôles, délégation de pouvoir avec portée configurable
- **Méthodes d'identification** — 8 sous-types TPH (LoginPassword, SMS, Bancaire, Photo, Parrainage, etc.)
- **Organisations** — Entités (`Entite` : Acteur TPH) avec activation/désactivation

### Interface Web (Razor Pages)

| Page | Accès |
|------|-------|
| Tableau de bord | Public |
| Offres / Demandes | Public (actions : authentifié) |
| Transactions / Discussion | Authentifié |
| Mon panier | Authentifié |
| Notifications / Journal | Authentifié |
| Suggestions | Authentifié (génération : Coordinateur+) |
| Config. crise | Public (modification : Responsable) |
| Admin → Rôles / Mandats | Coordinateur / Responsable |
| Admin → Taxonomie | Coordinateur / Responsable |
| Admin → Organisations | Responsable |
| Admin → Méthodes d'identification | Coordinateur / Responsable |

---

## Tests

```bash
# Lancer tous les tests
dotnet test

# Avec rapport de couverture
dotnet test --collect:"XPlat Code Coverage"
```

**État actuel :** 283 tests, 0 échec

| Projet | Tests |
|--------|-------|
| `CrisisConnect.Domain.Tests` | 117 |
| `CrisisConnect.Application.Tests` | 114 |
| `CrisisConnect.Infrastructure.Tests` | 52 |

Les tests utilisent **NSubstitute** (mocks), **xUnit** et **EF Core InMemory** (Infrastructure).

---

## API — Documentation Swagger

La documentation interactive Swagger/OpenAPI est disponible en mode `Development` uniquement :

```
http://localhost:5072/swagger   (dev local)
http://localhost:8080/swagger   (Docker)
```

### Aperçu des endpoints (13 controllers)

```
POST /api/auth/{register,login,refresh,logout}

GET|POST /api/propositions/offres          ?statut=
GET|POST /api/propositions/demandes        ?statut=&urgence=
PATCH    /api/propositions/{id}/{archiver,clore,relance,reconfirmer}

GET|POST         /api/transactions
PATCH            /api/transactions/{id}/{confirmer,annuler}
GET|POST|PATCH   /api/transactions/{id}/{discussion,messages,visibilite}

GET|POST|PATCH   /api/paniers
GET|PATCH        /api/notifications/{destinataireId}
GET              /api/journal/{acteurId}
GET|POST|PATCH   /api/suggestions/*
GET|POST|PATCH   /api/config-catastrophe
GET|POST|PATCH   /api/roles/*
GET|POST|PATCH   /api/mandats/*
GET|POST|PATCH   /api/taxonomie/*
GET|POST|PATCH   /api/entites/*
GET|PATCH        /api/methodes-identification/*
GET              /health
```

---

## Variables d'environnement

Copier `.env.example` en `.env` et renseigner les valeurs :

```env
POSTGRES_DB=crisisconnect
POSTGRES_USER=crisisconnect_user
POSTGRES_PASSWORD=<mot-de-passe-fort>
DB_CONNECTION_STRING=Host=db;Database=crisisconnect;Username=crisisconnect_user;Password=<mot-de-passe-fort>
JWT_SECRET=<chaine-aleatoire-minimum-32-caracteres>
JWT_ISSUER=CrisisConnect
JWT_AUDIENCE=CrisisConnect
```

> **Important :** le fichier `.env` est exclu du dépôt (`.gitignore`). Ne jamais le commiter.
