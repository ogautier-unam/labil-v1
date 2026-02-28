# CLAUDE.md — CrisisConnect

**Guide de référence pour les sessions Claude**
**Projet :** CrisisConnect (IHDCM032 — LABIL)
**Stack :** .NET 10 · PostgreSQL · Docker · Clean Architecture · Razor Pages · Bootstrap 5.3

---

## ⚠️ Principes de Travail — LIRE EN PREMIER

1. **Jamais d'affirmation sans preuve.** Dire "j'ai modifié X dans le fichier Y à la ligne Z", pas "c'est fait".
2. **Vérifier avant d'affirmer.** Lire les fichiers, faire des recherches, tester — ne jamais supposer.
3. **Diagnostiquer avant de modifier.** Investiguer la cause racine d'abord. Jamais de modifications à l'aveugle.
4. **Être précis.** Référencer les chemins de fichiers et numéros de ligne dans chaque affirmation.
5. **Documenter ce qui a réellement été fait.** Les messages de commit listent des changements concrets.
6. **Ne jamais push sans demander.** Toujours demander la permission avant `git push`, même si ça semble logique.
7. **Pas d'affirmations absolues.** Éviter "ça va marcher" ou "le build devrait passer" — les résultats externes ne sont pas garantis.

---

## 📋 Contexte du Projet

**CrisisConnect** est une plateforme de gestion de crise permettant de coordonner des bénévoles lors d'événements d'urgence (catastrophes naturelles, crises sanitaires, etc.).

### Acteurs principaux
| Acteur | Rôle |
|--------|------|
| `Citoyen` | Signale un besoin, suit les propositions |
| `Bénévole` | Accepte/refuse des missions, consulte son planning |
| `Coordinateur` | Crée des missions, valide les matchings |
| `Responsable` | Supervision globale, tableau de bord, accès statistiques |

### Fonctionnalités clés
1. **Offres & Demandes** (Proposition abstraite → Offre / Demande avec Composite pattern)
2. **Transactions** (initiation, discussion, confirmation entre acteurs)
3. **Panier** (sélection multi-offres pour une demande)
4. **Authentification & rôles** (JWT, AttributionRole, Mandat temporel)
5. **Notifications typées** (TypeNotification enum, 8 types métier)
6. **Journal d'audit** (EntreeJournal, TypeOperation, toutes actions sensibles tracées)
7. **Intégration services externes** (cartographie, météo) via Adapter pattern

---

## 🛠️ Stack Technique

### Versions exactes
| Technologie | Version | Notes |
|-------------|---------|-------|
| .NET | **10.0** | `dotnet --version` → `10.0.x` |
| ASP.NET Core | **10.0** | Web API REST |
| Entity Framework Core | **10.x** | ORM principal |
| Npgsql.EF Core | **10.x** | Provider PostgreSQL |
| PostgreSQL | **17** | Image Docker `postgres:17-alpine` |
| Docker | 24+ | `docker --version` |
| Docker Compose | v2 | `docker compose` (sans tiret) |
| dotnet-ef (tool) | **10.x** | `dotnet tool update --global dotnet-ef` |
| Bootstrap | **5.3.3** | Bundlé dans `CrisisConnect.Web/wwwroot/lib/bootstrap/` |
| Razor Pages | **10.0** | `CrisisConnect.Web` — front-end ASP.NET Core |

### Packages NuGet principaux
```
# Domain (aucune dépendance externe)
# rien

# Application
MediatR
FluentValidation
AutoMapper

# Infrastructure
Microsoft.EntityFrameworkCore
Npgsql.EntityFrameworkCore.PostgreSQL
Microsoft.EntityFrameworkCore.Design   # requis aussi dans API pour dotnet-ef
EFCore.NamingConventions               # UseSnakeCaseNamingConvention()
Microsoft.AspNetCore.Authentication.JwtBearer
BCrypt.Net-Next

# API
Swashbuckle.AspNetCore                       # Swagger/OpenAPI
Microsoft.EntityFrameworkCore.Design         # requis par dotnet-ef (startup project)
FluentValidation.DependencyInjectionExtensions  # AddValidatorsFromAssembly()
Serilog.AspNetCore                           # Logging structuré
```

---

## 🏗️ Architecture — Clean Architecture

### 4 projets (1 solution)

```
CrisisConnect.sln
├── src/
│   ├── CrisisConnect.Domain/          ← Entités, Value Objects, Interfaces, Enums
│   ├── CrisisConnect.Application/     ← Use Cases, DTOs, CQRS (MediatR), Validators
│   ├── CrisisConnect.Infrastructure/  ← EF Core, Repositories, Services externes
│   └── CrisisConnect.API/             ← Controllers, Middleware, DI, Swagger, Program.cs
└── tests/
    ├── CrisisConnect.Domain.Tests/
    ├── CrisisConnect.Application.Tests/
    └── CrisisConnect.Infrastructure.Tests/
```

### Règle de dépendance (stricte)
```
API → Application → Domain
Infrastructure → Application → Domain
```
- `Domain` : **zéro dépendance** externe
- `Application` : dépend uniquement de `Domain`
- `Infrastructure` : implémente les interfaces de `Domain`/`Application`
- `API` : orchestre tout, inject les implémentations

### Structure interne Domain
```
CrisisConnect.Domain/
├── Entities/
│   ├── Acteur.cs              # abstract (TPH: type_acteur)
│   ├── Personne.cs            # : Acteur
│   ├── Entite.cs              # : Acteur (organisation)
│   ├── Proposition.cs         # abstract (TPH: type_proposition)
│   ├── Offre.cs               # : Proposition
│   ├── Demande.cs             # : Proposition (Composite pattern, SousDemandes)
│   ├── Transaction.cs         # initiation + suivi entre acteurs
│   ├── Discussion.cs          # 1-1 avec Transaction (constructor internal)
│   ├── Message.cs             # appartient a Discussion
│   ├── Panier.cs              # multi-offres pour une demande
│   ├── AttributionRole.cs     # role d'un acteur (temporel)
│   ├── Mandat.cs              # delegation de pouvoir
│   ├── EntreeJournal.cs       # audit log
│   ├── Notification.cs
│   └── RefreshToken.cs
├── ValueObjects/
│   ├── Adresse.cs
│   ├── Localisation.cs        # lat/lon
│   └── PlageTemporelle.cs
├── Enums/
│   ├── StatutProposition.cs   # Active/EnAttenteRelance/EnTransaction/Archivee/Cloturee
│   ├── StatutTransaction.cs   # EnCours/Confirmee/Annulee
│   ├── StatutPanier.cs        # Ouvert/Confirme/Annule
│   ├── Visibilite.cs          # Publique/Privee
│   ├── OperateurLogique.cs    # Simple/Et/Ou (Composite Demande)
│   ├── NiveauUrgence.cs       # Faible/Moyen/Eleve/Critique
│   ├── TypeNotification.cs    # 8 types metier
│   ├── TypeOperation.cs       # 26 types (audit journal)
│   ├── TypeRole.cs            # Contributeur/.../AdminSysteme
│   ├── NiveauBadge.cs
│   ├── StatutRole.cs
│   └── PorteeMandat.cs
├── Interfaces/
│   ├── Repositories/          # IOffreRepository, IDemandeRepository, ITransactionRepository...
│   └── Services/              # INotificationService, IJwtService, IPasswordHasher...
└── Exceptions/
    ├── DomainException.cs
    └── NotFoundException.cs
```

### Structure interne Application
```
CrisisConnect.Application/
├── UseCases/
│   ├── Propositions/
│   │   ├── GetPropositions/
│   │   └── GetPropositionById/
│   ├── Offres/
│   │   └── CreateOffre/       # CreateOffreCommand, Handler, Validator
│   ├── Demandes/
│   │   └── CreateDemande/     # CreateDemandeCommand, Handler, Validator
│   ├── Transactions/
│   │   └── InitierTransaction/
│   ├── Notifications/
│   │   ├── GetNotifications/
│   │   └── MarkAsRead/
│   └── Auth/
│       ├── Register/
│       ├── Login/
│       └── RefreshToken/
├── DTOs/
│   ├── PropositionDto.cs
│   ├── OffreDto.cs
│   ├── DemandeDto.cs
│   ├── TransactionDto.cs
│   ├── NotificationDto.cs
│   └── AuthDto.cs
├── Mappings/
│   └── MappingProfile.cs      # AutoMapper
└── Common/
    ├── Behaviours/
    │   ├── ValidationBehaviour.cs
    │   └── LoggingBehaviour.cs
    └── Interfaces/
        └── ICurrentUserService.cs
```

### Structure interne Infrastructure
```
CrisisConnect.Infrastructure/
├── Persistence/
│   ├── AppDbContext.cs
│   ├── Configurations/        # IEntityTypeConfiguration<T>
│   │   ├── PropositionConfiguration.cs
│   │   └── ...
│   ├── Repositories/
│   │   ├── PropositionRepository.cs
│   │   └── ...
│   └── Migrations/            # EF Core migrations (auto-générées)
├── Services/
│   ├── JwtService.cs
│   ├── NotificationService.cs
│   ├── CartoAdapter.cs        # Adapter pattern (service externe)
│   └── MeteoAdapter.cs
└── DependencyInjection.cs     # Extension AddInfrastructure()
```

---

## 🐳 Docker

### Fichiers Docker
```
CrisisConnect/
├── docker-compose.yml
├── docker-compose.override.yml    # dev local (hot reload, ports)
├── .env                           # variables d'environnement (non commité)
├── .env.example                   # template commité
└── src/CrisisConnect.API/
    └── Dockerfile
```

### docker-compose.yml (production-like)
```yaml
services:
  api:
    build:
      context: .
      dockerfile: src/CrisisConnect.API/Dockerfile
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__Default=${DB_CONNECTION_STRING}
      - Jwt__Secret=${JWT_SECRET}
    depends_on:
      db:
        condition: service_healthy

  db:
    image: postgres:17-alpine
    environment:
      POSTGRES_DB: ${POSTGRES_DB}
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${POSTGRES_USER} -d ${POSTGRES_DB}"]
      interval: 5s
      timeout: 5s
      retries: 5

volumes:
  pgdata:
```

### docker-compose.override.yml (dev)
```yaml
services:
  api:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:8080
    volumes:
      - ./src:/app/src    # hot reload
```

### Dockerfile (multi-stage)
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/CrisisConnect.API/CrisisConnect.API.csproj", "src/CrisisConnect.API/"]
COPY ["src/CrisisConnect.Application/CrisisConnect.Application.csproj", "src/CrisisConnect.Application/"]
COPY ["src/CrisisConnect.Infrastructure/CrisisConnect.Infrastructure.csproj", "src/CrisisConnect.Infrastructure/"]
COPY ["src/CrisisConnect.Domain/CrisisConnect.Domain.csproj", "src/CrisisConnect.Domain/"]
RUN dotnet restore "src/CrisisConnect.API/CrisisConnect.API.csproj"
COPY . .
RUN dotnet publish "src/CrisisConnect.API/CrisisConnect.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CrisisConnect.API.dll"]
```

### .env.example
```env
POSTGRES_DB=crisisconnect
POSTGRES_USER=crisisconnect_user
POSTGRES_PASSWORD=change_me_in_production
DB_CONNECTION_STRING=Host=db;Database=crisisconnect;Username=crisisconnect_user;Password=change_me_in_production
JWT_SECRET=change_me_minimum_32_characters_long
```

---

## 💾 Base de Données (EF Core + PostgreSQL)

### Connection string (appsettings.Development.json)
```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=crisisconnect;Username=crisisconnect_user;Password=change_me_in_production"
  }
}
```

### Conventions de nommage PostgreSQL
- Tables : `snake_case` pluriel → `propositions`, `acteurs`, `matchings`
- Colonnes : `snake_case` → `statut_proposition`, `created_at`
- Configurer via `UseSnakeCaseNamingConvention()` (package EFCore.NamingConventions)

### Configuration EF Core (DbContext)
```csharp
// AppDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    // Configurations dans Persistence/Configurations/*.cs
}
```

### Commandes migrations (depuis racine solution)
```bash
# Créer une migration
dotnet ef migrations add NomMigration \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Appliquer les migrations
dotnet ef database update \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Revenir à une migration précédente
dotnet ef database update NomMigrationCible \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API

# Supprimer dernière migration (non appliquée)
dotnet ef migrations remove \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API
```

---

## 🔐 Authentification JWT

- **Algorithme :** HS256
- **Claims :** `sub` (userId), `role`, `email`, `exp`
- **Durée token :** 1h (access) + 7j (refresh)
- **Stockage côté client :** HttpOnly cookie (pas localStorage)
- Les rôles correspondent aux acteurs : `Citoyen`, `Benevole`, `Coordinateur`, `Responsable`

---

## 📐 Conventions de Code

### Nommage C#
| Élément | Convention | Exemple |
|---------|-----------|---------|
| Classes, interfaces | PascalCase | `PropositionService`, `IMatchingRepository` |
| Méthodes | PascalCase | `GetPropositionByIdAsync()` |
| Variables locales, paramètres | camelCase | `propositionId`, `currentUser` |
| Propriétés | PascalCase | `StatutProposition`, `DateCreation` |
| Constantes | PascalCase | `MaxBenevoles` |
| Champs privés | `_camelCase` | `_repository`, `_logger` |

### Patterns obligatoires
- **CQRS via MediatR** : toute opération = Command (écriture) ou Query (lecture)
- **Repository pattern** : jamais d'accès direct à `AppDbContext` hors Infrastructure
- **Result pattern** ou exceptions domaine : pas de `null` return pour les erreurs
- **Async/await partout** : toutes les méthodes I/O sont asynchrones (`Async` suffix)

### Endpoints REST
```
GET    /api/propositions              # liste (Offres + Demandes)
GET    /api/propositions/{id}         # detail
POST   /api/propositions/offres       # creer une Offre
POST   /api/propositions/demandes     # creer une Demande

POST   /api/transactions              # initier une Transaction

GET    /api/notifications             # liste des notifications du user
PATCH  /api/notifications/{id}/read   # marquer comme lue

POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/refresh
POST   /api/auth/logout
```

### Réponses HTTP standard
| Situation | Code |
|-----------|------|
| Succès lecture | 200 OK |
| Ressource créée | 201 Created + Location header |
| Pas de contenu | 204 No Content |
| Erreur validation | 400 Bad Request + ProblemDetails |
| Non authentifié | 401 Unauthorized |
| Accès refusé | 403 Forbidden |
| Non trouvé | 404 Not Found + ProblemDetails |
| Erreur serveur | 500 Internal Server Error |

---

## 🔧 Commandes Utiles

### Développement
```bash
# Lancer l'environnement complet (API + DB)
docker compose up -d

# Lancer uniquement la DB (dev local sans container API)
docker compose up -d db

# Voir les logs
docker compose logs -f api

# Arrêter
docker compose down

# Arrêter + supprimer les volumes (reset BDD)
docker compose down -v
```

### Build & Tests
```bash
# Build solution complète
dotnet build

# Lancer tous les tests
dotnet test

# Tests avec coverage
dotnet test --collect:"XPlat Code Coverage"

# Lancer l'API en dev (avec hot reload)
dotnet watch run --project src/CrisisConnect.API
```

### EF Core
```bash
# Voir l'état des migrations
dotnet ef migrations list \
  --project src/CrisisConnect.Infrastructure \
  --startup-project src/CrisisConnect.API
```

### Qualité
```bash
# Format code
dotnet format

# Analyse statique
dotnet build /p:TreatWarningsAsErrors=true
```

---

## 📁 Structure Fichiers Racine du Repo

```
CrisisConnect/                     ← racine du repo
├── CrisisConnect.sln
├── CLAUDE.md                      ← ce fichier
├── README.md                      ← documentation projet
├── .gitignore
├── .env.example                   ← template variables d'environnement
├── docker-compose.yml
├── docker-compose.override.yml
├── src/
│   ├── CrisisConnect.API/
│   ├── CrisisConnect.Application/
│   ├── CrisisConnect.Domain/
│   └── CrisisConnect.Infrastructure/
└── tests/
    ├── CrisisConnect.Domain.Tests/
    ├── CrisisConnect.Application.Tests/
    └── CrisisConnect.Infrastructure.Tests/
```

### .gitignore indispensable
```gitignore
# .env (secrets)
.env

# Build
bin/
obj/
*.user

# Rider / VS / VSCode
.idea/
.vs/
.vscode/

# NuGet
*.nupkg
packages/

# Logs
*.log
```

---

## 🎯 Priorités de Développement

### Ordre recommandé
1. **Setup initial** : solution, projets, docker-compose, connexion DB vérifiée
2. **Domain** : entités, value objects, enums, interfaces (aucune dépendance)
3. **Infrastructure/Persistence** : AppDbContext, configurations EF, première migration
4. **Application** : use cases CQRS prioritaires (CreateProposition, GetPropositions, etc.)
5. **API** : controllers, auth JWT, Swagger
6. **Tests** : unitaires Domain/Application, intégration Infrastructure

### Use cases prioritaires (MVP)
1. `RegisterActeur` / `Login` / `RefreshToken`
2. `CreateProposition` / `GetPropositions` / `GetPropositionById`
3. `CreateMission` / `AssignBenevole` (matching simple)
4. `GetNotifications` / `MarkAsRead`

---

## ⚠️ Règles de Travail

1. **Ne jamais commiter `.env`** (contient les secrets)
2. **Toujours ajouter `Async` suffix** sur les méthodes asynchrones
3. **Migrations = 1 par fonctionnalité** (pas de mega-migration)
4. **Pas d'accès direct `AppDbContext`** dans Application ou API
5. **Swagger actif en Development uniquement**
6. **Logs structurés** (Serilog) — jamais de `Console.WriteLine`
7. **Mettre à jour ce CLAUDE.md** si la stack ou l'architecture change

---

## ✅ Bonnes Pratiques

### À faire
- Lire les fichiers avant de les modifier
- Utiliser les outils dédiés (`Read`/`Edit`/`Write`) plutôt que bash pour les opérations sur les fichiers
- Référencer les chemins et numéros de ligne dans chaque affirmation
- Tester les changements avant de commiter
- Utiliser `IHttpClientFactory` pour tous les clients HTTP (CartoAdapter, MeteoAdapter)
- Utiliser le Result pattern ou les exceptions domaine — jamais `null` comme signal d'erreur

### À ne pas faire
- Ne pas push sur GitHub sans permission explicite
- Ne pas faire d'affirmations absolues sur des systèmes externes
- Ne pas utiliser bash pour les opérations sur les fichiers (utiliser `Read`/`Edit`/`Write`)
- Ne pas instancier `HttpClient` directement (risque de socket exhaustion)
- Ne pas accéder à `AppDbContext` hors de la couche Infrastructure
- Ne pas exposer les stack traces dans les réponses API en Production

---

## 🔁 Tâches Courantes

### Ajouter une nouvelle entité
1. Créer l'entité dans `CrisisConnect.Domain/Entities/` (hériter d'une classe de base si applicable)
2. Créer l'interface repository dans `CrisisConnect.Domain/Interfaces/Repositories/`
3. Ajouter le `DbSet<T>` dans `AppDbContext`
4. Créer la configuration EF Core dans `CrisisConnect.Infrastructure/Persistence/Configurations/`
5. Implémenter le repository dans `CrisisConnect.Infrastructure/Persistence/Repositories/`
6. Enregistrer dans `DependencyInjection.cs`
7. Créer la migration : `dotnet ef migrations add AddNomEntite --project src/CrisisConnect.Infrastructure --startup-project src/CrisisConnect.API`
8. Appliquer : `dotnet ef database update ...`

### Ajouter un use case (CQRS)
1. Créer le dossier `CrisisConnect.Application/UseCases/{Feature}/{UseCaseName}/`
2. Créer `{UseCaseName}Command.cs` ou `{UseCaseName}Query.cs` (record, IRequest<T>)
3. Créer `{UseCaseName}Handler.cs` (IRequestHandler<TRequest, TResponse>)
4. Créer `{UseCaseName}Validator.cs` si validation nécessaire (AbstractValidator<T>)
5. Créer le DTO de retour dans `DTOs/` si nécessaire
6. Ajouter le mapping dans `MappingProfile.cs`
7. Appeler depuis le controller via `_mediator.Send(command)`

### Ajouter un endpoint
1. Créer ou ouvrir le controller dans `CrisisConnect.API/Controllers/`
2. Injecter `IMediator` (via constructeur)
3. Appliquer `[Authorize(Roles = "...")]` selon les acteurs autorisés
4. Retourner les codes HTTP standards (voir section Conventions)
5. Documenter avec `/// <summary>` pour Swagger

---

## 🔧 Dépannage

### Warnings CS8604/CS8602 (null reference)
- Vérifier les types nullable (`string?`, `T?`)
- Ajouter des vérifications null avant l'appel : `if (value is not null)`
- Utiliser l'opérateur null-forgiving `!` uniquement si la valeur est certaine non-null

### Migration EF Core échoue
- Vérifier que `AppDbContext` est bien enregistré dans `Program.cs`
- `Microsoft.EntityFrameworkCore.Design` doit être présent dans **les deux** projets : `Infrastructure` ET `API` (startup project)
- Le tool `dotnet-ef` doit être en version **10.x** : `dotnet tool update --global dotnet-ef`
- Toujours spécifier `--project` et `--startup-project` (ne pas omettre)
- Les Value Objects utilisés dans des entités (ex. `Adresse` dans `Personne`) doivent être configurés avec `OwnsOne()` dans leur `IEntityTypeConfiguration`, sinon EF Core tente de les traiter comme entités et lève une erreur de clé primaire manquante

### `[ERR] Failed executing DbCommand` lors de `database update`
- C'est un comportement **normal** au premier `database update` : EF Core cherche la table `__EFMigrationsHistory` qui n'existe pas encore, échoue (loggé ERR), puis la crée et applique les migrations. `Done.` en fin de sortie = succès.

### AutoMapper v16+ : API changée
- Ne plus utiliser : `services.AddAutoMapper(typeof(MappingProfile).Assembly)`
- Utiliser : `services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly))`

### Serilog masque les messages de démarrage ASP.NET Core
- Ajouter l'override `"Microsoft.Hosting.Lifetime": "Information"` dans `appsettings.json` pour voir "Now listening on: ..."
- Ajouter `if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection()` pour éviter le warning HTTPS en dev

### Erreur de connexion PostgreSQL
- Vérifier que le container `db` est healthy : `docker compose ps`
- En dev local sans Docker : vérifier que PostgreSQL tourne sur le port 5432
- La connection string dans `appsettings.Development.json` utilise `localhost`, celle dans Docker utilise `db` (nom du service)

### LINQ avec listes en mémoire
- Ne pas faire `dbSet.Where(x => localList.Contains(x.Id))` directement si la liste est complexe
- Extraire les IDs d'abord : `var ids = list.Select(x => x.Id).ToList()` puis `Where(x => ids.Contains(x.Id))`

### HttpClient dans les Adapters
- Toujours utiliser `IHttpClientFactory` (injecté via DI)
- Enregistrer dans `DependencyInjection.cs` : `services.AddHttpClient<CartoAdapter>()`

### TPH : conflit de MaxLength sur colonne partagee
- Erreur : `'Entite.Nom' and 'Personne.Nom' are both mapped to column 'nom' in 'acteurs', but are configured with different maximum lengths`
- Cause : dans une hiérarchie TPH, deux sous-types configurent la même colonne avec des longueurs différentes
- Fix : aligner le `HasMaxLength` sur la valeur la plus grande dans les deux `IEntityTypeConfiguration`

---

## 📝 Méthodologie Sessions Claude

### En début de session
1. Lire CLAUDE.md en priorité
2. Vérifier `git status` et `docker compose ps`
3. Vérifier les migrations : `dotnet ef migrations list ...`

### Créer une section "Session en cours" en bas du fichier
- Documenter les étapes avec ✅ / ⏳ / ⬜
- En cas d'erreur : noter le message exact + solution trouvée
- En fin de session : simplifier en résumé dans l'historique

### Historique des sessions

#### Session 1 — 2026-02-28 — Setup initial
✅ Solution .NET 10 + 7 projets (4 src, 3 tests) avec références Clean Architecture
✅ Packages NuGet installés (MediatR 14, FluentValidation 12, AutoMapper 16, EF Core 10, Npgsql 10, Serilog 10, Swashbuckle 10)
✅ Structure Domain (entités, value objects, enums, interfaces, exceptions)
✅ Structure Application (CQRS CreateProposition/GetPropositions, behaviours, mappings)
✅ Structure Infrastructure (AppDbContext, configurations EF, repository, DI)
✅ API (Program.cs, PropositionsController, appsettings Serilog, Swagger dev-only)
✅ Docker (Dockerfile sdk:10.0, docker-compose.yml postgres:17-alpine, override.yml)
✅ Migration InitialCreate appliquée (`src/CrisisConnect.Infrastructure/Migrations/`)
✅ API fonctionnelle sur http://localhost:5072 — Swagger sur /swagger

#### Session 2 — 2026-02-28 — Refactoring domaine (alignement diagrammes de classes)
✅ Ajout `class-diagrams/` (9 fichiers .puml PlantUML) + enonce .txt commites
✅ Analyse des ecarts entre implementation session 1 et diagrammes de classes
✅ Refactoring Enums : StatutProposition (5 valeurs), StatutTransaction, Visibilite (2 valeurs)
✅ Nouveaux enums : OperateurLogique, NiveauUrgence, TypeNotification (8), StatutPanier, TypeRole, NiveauBadge, StatutRole, PorteeMandat, TypeOperation (26)
✅ Suppression : StatutMission.cs, StatutMatching.cs
✅ Refactoring entites : Proposition (abstraite), Offre, Demande (Composite), Transaction, Discussion (internal ctor), Message, Panier, Entite, AttributionRole, Mandat, EntreeJournal
✅ Notification mise a jour (TypeNotification, DateEnvoi, RefEntiteId ; suppression Sujet)
✅ Suppression : Mission.cs, Matching.cs
✅ Nouvelles interfaces repo : IOffreRepository, IDemandeRepository, ITransactionRepository, IPanierRepository, IEntiteRepository, IEntreeJournalRepository
✅ Suppression : IMissionRepository, IMatchingRepository
✅ Configurations EF Core : ActeurConfiguration (TPH type_acteur), EntiteConfiguration, PropositionConfiguration (TPH type_proposition + discriminateur Offre/Demande), OffreConfiguration, DemandeConfiguration (Composite self-ref), TransactionConfiguration, DiscussionConfiguration, MessageConfiguration, PanierConfiguration, AttributionRoleConfiguration, MandatConfiguration, EntreeJournalConfiguration
✅ Suppression configs/repos : MissionConfiguration, MatchingConfiguration, MissionRepository, MatchingRepository
✅ Nouveaux repos : OffreRepository, DemandeRepository, TransactionRepository, PanierRepository, EntiteRepository, EntreeJournalRepository
✅ AppDbContext mis a jour (DbSets nouveaux, suppression Mission/Matching)
✅ DependencyInjection.cs mis a jour
✅ Application : CreateOffreCommand/Handler/Validator, CreateDemandeCommand/Handler/Validator, InitierTransactionCommand/Handler/Validator
✅ DTOs : OffreDto, DemandeDto, TransactionDto (suppression MissionDto/MatchingDto)
✅ MappingProfile mis a jour
✅ PropositionsController mis a jour (POST /offres, POST /demandes), TransactionsController cree
✅ Suppression use cases Missions (CreateMission, AssignBenevole)
✅ Anciennes migrations supprimees + nouvelle migration InitialCreate (20260228151932)
✅ Build : 0 erreur — migration appliquee — commit b96071f (96 fichiers)

#### Session 3 — 2026-02-28 — Modele domaine complet (P2/P3/P4/P5/P6)
✅ P5 (Services) : IServiceTraduction, IStrategiePriorisation + 3 Adapters (CorpusInterne, DeepL, LibreTranslate) + 4 Strategies (Anciennete, Urgence, RegionSeverite, Type)
✅ P3 (Config) : ConfigCatastrophe, CategorieTaxonomie (Pattern Composite self-ref)
✅ P4 (Media+Specialises) : Media, DemandeQuota + IntentionDon, DemandeRepartitionGeo, DemandeSurCatalogue + LigneCatalogue, PropositionAvecValidation
✅ P2 (MethodeIdentification) : 8 sous-types TPH (LoginPassword, CarteIdentiteElectronique, VerificationSMS, VerificationBancaire, VerificationFacture, VerificationPhoto, Parrainage, Delegation)
✅ P6 (Suggestion) : SuggestionAppariement
✅ Nouveaux enums : TypeMedia, StatutIntention, StatutLigne, StatutValidation, NiveauFiabilite, TypeFacture, ModeVerification
✅ Interfaces repo : IConfigCatastropheRepository, ICategorieTaxonomieRepository, IMethodeIdentificationRepository, ISuggestionAppariementRepository
✅ EF configs : 11 nouveaux fichiers (Media, MethodeIdentification+Parrainage, ConfigCatastrophe, CategorieTaxonomie, DemandeQuota, IntentionDon, DemandeRepartitionGeo, DemandeSurCatalogue, LigneCatalogue, PropositionAvecValidation, SuggestionAppariement)
✅ PropositionConfiguration : discriminateur etendu a 6 valeurs (+ DemandeQuota, DemandeRepartitionGeo, DemandeSurCatalogue, PropositionAvecValidation)
✅ Repos : ConfigCatastropheRepository, CategorieTaxonomieRepository, MethodeIdentificationRepository, SuggestionAppariementRepository
✅ AppDbContext + DependencyInjection mis a jour
✅ Migration AddDomainModel (20260228155244) appliquee
✅ Build : 0 erreur — commit b52e73e (53 fichiers, 3932 insertions)

#### Session 4 — 2026-02-28 — Use cases GET + Controllers + Projet Web
✅ Fix NotificationDto (Sujet→Type, CreeLe→DateCreation — alignement entite Notification)
✅ GetAllAsync ajouté dans ITransactionRepository + TransactionRepository
✅ Use cases : GetOffres, GetOffreById, GetDemandes, GetDemandeById, GetTransactions, GetTransactionByIdQuery
✅ Use cases lifecycle : ConfirmerTransaction (Clore proposition), AnnulerTransaction (LibererDeTransaction)
✅ Use cases config : GetConfigCatastropheQuery, CreateConfigCatastropheCommand + Validator
✅ ConfigCatastropheDto ajouté ; MappingProfile mis a jour
✅ PropositionsController : GET /offres, GET /offres/{id}, GET /demandes, GET /demandes/{id}
✅ TransactionsController : GET, GET/{id}, PATCH/{id}/confirmer, PATCH/{id}/annuler
✅ ConfigCatastropheController : GET /api/config-catastrophe, POST (Responsable)
✅ Build : 0 erreur — commit 5781248 (29 fichiers, 521 insertions)

#### Session 5 — 2026-02-28 — Front-end Razor Pages + Bootstrap 5.3
✅ Projet CrisisConnect.Web créé (dotnet new razor, net10.0) + ajouté à la solution
✅ Bootstrap 5.3.3 bundlé par le template (wwwroot/lib/bootstrap/) — aucune mise a jour nécessaire
✅ Models/ : OffreModel, DemandeModel, TransactionModel, PropositionModel (types simples, pas de ref Domain)
✅ Services/ApiClient.cs : GetOffresAsync, GetDemandesAsync, GetPropositionsAsync, GetTransactionsAsync, Get*ByIdAsync
✅ Program.cs : AddHttpClient<ApiClient>(BaseAddress depuis ApiSettings:BaseUrl)
✅ appsettings.json : BaseUrl=http://localhost:8080 (prod) / appsettings.Development.json : BaseUrl=http://localhost:5072
✅ _Layout.cshtml : navbar Bootstrap 5.3 (rouge CrisisConnect), dropdown Propositions, liens Transactions/Auth
✅ Pages : Index (tableau de bord 3 cards), Propositions/Index, Propositions/Offres, Propositions/Demandes, Transactions/Index, Auth/Login

#### Session 6 — 2026-02-28 — Docker Web + Paniers + Auth Web fonctionnel
✅ src/CrisisConnect.Web/Dockerfile (multi-stage, sdk:10.0)
✅ docker-compose.yml : service web (port 8081→8080, ApiSettings__BaseUrl=http://api:8080) + Jwt__Issuer/Audience pour api
✅ docker-compose.override.yml : service web dev
✅ .env.example : JWT_ISSUER, JWT_AUDIENCE ajoutés
✅ Panier use cases : CreatePanier, GetPanier, AjouterOffreAuPanier, ConfirmerPanier, AnnulerPanier
✅ PanierDto + MappingProfile mis a jour (Panier → PanierDto avec Offres)
✅ PanierController : GET /api/paniers?proprietaireId, POST, POST /{id}/offres, PATCH /{id}/confirmer, PATCH /{id}/annuler
✅ JwtCookieHandler : DelegatingHandler injectant le Bearer token depuis le claim "access_token" du cookie
✅ ApiClient : LoginAsync, RegisterAsync ajoutés
✅ Program.cs Web : AddAuthentication(Cookie), AddHttpContextAccessor, JwtCookieHandler enregistré
✅ Auth/Login : appel réel à POST /api/auth/login + SignInAsync avec cookie (claims : UserId, Email, Role, access_token)
✅ Auth/Register : inscription + connexion automatique
✅ Auth/Logout : SignOutAsync + redirect Index
✅ _Layout.cshtml : affichage User.Identity.Name + Role si connecté, bouton Déconnexion (form POST), sinon S'inscrire/Connexion
✅ Build : 0 erreur

#### Session 7 — 2026-02-28 — Tests unitaires (Domain + Application)
✅ Domain.Tests : OffreTests (11), DemandeTests (7), TransactionTests (6), PanierTests (10) — 34 tests, 0 échec
✅ Application.Tests : LoginCommandHandlerTests (4), CreatePanierCommandHandlerTests (2), AjouterOffreAuPanierCommandHandlerTests (3) — 9 tests, 0 échec
✅ AutoMapperFixture.cs : ServiceCollection + AddLogging() + AddAutoMapper(cfg => cfg.AddMaps(...))
✅ Packages ajoutés Application.Tests : NSubstitute 5.3.0, Microsoft.Extensions.DependencyInjection 10.0.0, Microsoft.Extensions.Logging 10.0.0
✅ Total : 43 tests, 0 échec (Domain.Tests 33, Application.Tests 9, Infrastructure.Tests 1)

#### Session 8 — 2026-02-28 — Renommage + tests Application (use cases transactions/offres/demandes)
✅ MissionsController.cs renommé en TransactionsController.cs (git mv — contenu inchangé)
✅ RegisterActeurCommandHandlerTests (2 tests : succès + email dupliqué)
✅ CreateOffreCommandHandlerTests (2 tests : sans localisation, avec livraison)
✅ CreateDemandeCommandHandlerTests (2 tests : urgence par défaut, urgence critique)
✅ InitierTransactionCommandHandlerTests (3 tests : succès, proposition introuvable, offre déjà en transaction)
✅ ConfirmerTransactionCommandHandlerTests (2 tests : succès + proposition clôturée, introuvable)
✅ AnnulerTransactionCommandHandlerTests (2 tests : succès + proposition libérée, introuvable)
✅ Total : 56 tests, 0 échec (Domain.Tests 33, Application.Tests 22, Infrastructure.Tests 1)

#### Session 9 — 2026-02-28 — Web : Paniers + Offres interactives
✅ PanierModel ajouté (Models/PanierModel.cs)
✅ ApiClient : CreateOffre, InitierTransaction, GetPanier, CreatePanier, AjouterOffreAuPanier, ConfirmerPanier, AnnulerPanier
✅ Pages/Paniers/Index.cshtml + .cshtml.cs : voir/créer panier, confirmer, annuler (Authorize)
✅ Pages/Propositions/Offres.cshtml.cs : OnPostPublierAsync + OnPostAjouterAuPanierAsync
✅ Pages/Propositions/Offres.cshtml : formulaire "Publier une offre" (collapse) + bouton "+ Panier" par offre active
✅ _Layout.cshtml : lien "Mon panier" dans navbar (visible si connecté)
✅ Build : 0 erreur

#### Session 10 — 2026-02-28 — Web interactif complet + Infrastructure.Tests
✅ ApiClient : ConfirmerTransactionAsync, AnnulerTransactionAsync, CreateDemandeAsync
✅ Transactions/Index.cshtml.cs : OnPostConfirmerAsync + OnPostAnnulerAsync (constantes KeySuccess/KeyError)
✅ Transactions/Index.cshtml : boutons Confirmer/Annuler par transaction EnCours (avec confirm JS)
✅ Demandes.cshtml.cs : OnPostPublierAsync (titre, description, urgence, région)
✅ Demandes.cshtml : formulaire collapse "Publier une demande" + select NiveauUrgence
✅ Infrastructure.Tests : DbContextFactory (InMemory, base isolée par test)
✅ OffreRepositoryTests (4 tests : Add+GetById, GetAll, GetById inexistant, Update statut)
✅ TransactionRepositoryTests (4 tests : Add+GetById+Discussion, GetAll, Update statut, GetByPropositionId)
✅ Packages : Microsoft.EntityFrameworkCore.InMemory 10.0.3 + Microsoft.EntityFrameworkCore 10.0.3
✅ Total : 63 tests, 0 échec (Domain 33, Application 22, Infrastructure 8)
