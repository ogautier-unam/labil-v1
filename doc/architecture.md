# CrisisConnect — Architecture

## Overview

CrisisConnect follows **Clean Architecture** with strict unidirectional dependencies:

```
Web (Razor Pages)
    ↓ HTTP
API (ASP.NET Core)
    ↓
Application (CQRS)
    ↓
Domain (Entities, Interfaces)
    ↑
Infrastructure (EF Core, Services)
```

## Projects

| Project | Role | Key dependencies |
|---------|------|-----------------|
| `CrisisConnect.Domain` | Entities, Value Objects, Interfaces, Enums | None |
| `CrisisConnect.Application` | Use Cases (CQRS), DTOs, Validators, Mappers | Domain |
| `CrisisConnect.Infrastructure` | EF Core, Repositories, Background Services | Application, Domain |
| `CrisisConnect.API` | Controllers, Middleware, DI, Swagger | Application, Infrastructure |
| `CrisisConnect.Web` | Razor Pages, ApiClient, Bootstrap 5.3 | API (via HTTP) |

## Design Patterns

### CQRS (Command Query Responsibility Segregation)
- All write operations use `IRequest<T>` Commands (e.g., `CreateOffreCommand`)
- All read operations use `IRequest<T>` Queries (e.g., `GetOffresQuery`)
- Implemented with **Mediator 3.x** (martinothamar, MIT license)

### Repository Pattern
- One interface per aggregate in `Domain/Interfaces/Repositories/`
- One implementation per aggregate in `Infrastructure/Persistence/Repositories/`
- Direct `AppDbContext` access is forbidden outside Infrastructure

### Strategy Pattern (Prioritization)
- `IStrategiePriorisation` in Domain defines the contract
- 4 implementations in Infrastructure: `anciennete`, `urgence`, `region`, `type`
- Activated via `GET /api/propositions/demandes?strategie=urgence`

### Composite Pattern (Demandes)
- `Demande` can contain sub-demands (`SousDemandes`)
- `OperateurLogique` enum: `Simple | Et | Ou`
- `Ou` operator triggers automatic sibling closure on satisfaction

### Adapter Pattern (External Services)
- `IServiceTraduction` → `AdaptateurCorpusInterne` | `AdaptateurDeepL` | `AdaptateurLibreTranslate`
- `IStrategiePriorisation` → 4 prioritization strategies

### TPH (Table Per Hierarchy)
- `Acteur` → `Personne` | `Entite` (discriminator: `type_acteur`)
- `Proposition` → `Offre` | `Demande` | `DemandeQuota` | `DemandeRepartitionGeo` | `DemandeSurCatalogue` | `PropositionAvecValidation` (discriminator: `type_proposition`)
- `MethodeIdentification` → 8 subtypes (discriminator: `type_methode`)

## Pipeline Behaviors (Application)

| Behavior | Order | Role |
|----------|-------|------|
| `LoggingBehaviour` | 1st | Logs request before and after |
| `ValidationBehaviour` | 2nd | Runs FluentValidation, throws on failure |
| `AuditBehaviour` | 3rd | Persists `EntreeJournal` for 48 tracked commands |

## Background Services (Infrastructure)

| Service | Cycle | Purpose |
|---------|-------|---------|
| `ArchivageAutomatiqueService` | 1 hour | Archives inactive propositions per `ConfigCatastrophe` delays |
| `RappelExpirationRoleService` | 1 hour | Sends notifications for roles expiring within 7 days |

## Domain Model Summary

```
Acteur (abstract, TPH)
├── Personne  — natural person (GDPR: Anonymiser())
└── Entite    — organization

Proposition (abstract, TPH)
├── Offre               — what is offered
├── Demande             — what is needed (Composite pattern)
├── DemandeQuota        — quota-based demand with IntentionDon workflow
├── DemandeRepartitionGeo — geographic resource distribution
├── DemandeSurCatalogue — catalog-based demand with LigneCatalogue items
└── PropositionAvecValidation — requires coordinator approval

Transaction           — links Offre ↔ Demande
Discussion            — 1-to-1 with Transaction (auto-translation)
Panier                — multi-offre basket for a Demande
SuggestionAppariement — AI-scored matching suggestions (Jaccard)
```
