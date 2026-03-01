# CrisisConnect — API Reference

Base URL: `http://localhost:8080` (development)

All endpoints return JSON. Enum values are serialized as strings.

## Authentication

```
POST /api/auth/register    → 200 { token, refreshToken, role }
POST /api/auth/login       → 200 { token, refreshToken, role }
POST /api/auth/refresh     → 200 { token }
POST /api/auth/logout      → 204  [Authorize]
```

Tokens are JWT (HS256, 1h access + 7d refresh). The Web layer stores them in HttpOnly cookies.

## Propositions

### Offres
```
GET    /api/propositions/offres              → 200 OffreDto[]   (?statut=Active)
GET    /api/propositions/offres/{id}         → 200 OffreDto
POST   /api/propositions/offres             → 201 OffreDto      [Authorize]
PATCH  /api/propositions/offres/{id}        → 200 OffreDto      [Authorize]
```

### Demandes
```
GET    /api/propositions/demandes            → 200 DemandeDto[]  (?statut=&urgence=&strategie=)
GET    /api/propositions/demandes/{id}       → 200 DemandeDto
POST   /api/propositions/demandes           → 201 DemandeDto     [Authorize]
PATCH  /api/propositions/demandes/{id}      → 200 DemandeDto     [Authorize]
```

**Strategie values** (NF-11): `anciennete` | `urgence` | `region` | `type`

### Lifecycle
```
PATCH  /api/propositions/{id}/archiver      → 204  [Coordinateur, Responsable]
PATCH  /api/propositions/{id}/clore         → 204  [Coordinateur, Responsable]
PATCH  /api/propositions/{id}/relance       → 204  [Coordinateur, Responsable]
PATCH  /api/propositions/{id}/reconfirmer   → 204  [Coordinateur, Responsable]
PATCH  /api/propositions/{id}/recycler      → 204  [Coordinateur, Responsable]
```

### Demandes Quota
```
GET    /api/demandes-quota                  → 200 DemandeQuotaDto[]
GET    /api/demandes-quota/{id}             → 200 DemandeQuotaDto
POST   /api/demandes-quota                  → 201 DemandeQuotaDto  [Authorize]
PATCH  /api/demandes-quota/{id}/intentions/{intentionId}/accepter  → 200  [Authorize]
PATCH  /api/demandes-quota/{id}/intentions/{intentionId}/refuser   → 200  [Authorize]
PATCH  /api/demandes-quota/{id}/intentions/{intentionId}/confirmer → 200  [Authorize]
POST   /api/demandes-quota/{id}/intentions  → 201 IntentionDonDto  [Authorize]
```

### Demandes Sur Catalogue
```
GET    /api/demandes-sur-catalogue          → 200 DemandeSurCatalogueDto[]
GET    /api/demandes-sur-catalogue/{id}     → 200 DemandeSurCatalogueDto
POST   /api/demandes-sur-catalogue          → 201 DemandeSurCatalogueDto  [Authorize]
POST   /api/demandes-sur-catalogue/{id}/lignes → 201 DemandeSurCatalogueDto  [Authorize]
```

### Demandes Répartition Géo
```
GET    /api/demandes-repartition-geo        → 200 DemandeRepartitionGeoDto[]
GET    /api/demandes-repartition-geo/{id}   → 200 DemandeRepartitionGeoDto
POST   /api/demandes-repartition-geo        → 201 DemandeRepartitionGeoDto  [Authorize]
```

### Propositions avec Validation
```
POST   /api/propositions/avec-validation    → 201 PropositionAvecValidationDto  [Authorize]
PATCH  /api/propositions/{id}/valider       → 200  [Coordinateur, Responsable]
PATCH  /api/propositions/{id}/refuser-validation → 200  [Coordinateur, Responsable]
```

### Médias
```
GET    /api/propositions/{id}/medias        → 200 MediaDto[]
POST   /api/propositions/{id}/medias        → 201 MediaDto  [Authorize]
```

Body: `{ "url": "https://...", "type": "Photo|Video|Audio|Document" }`

## Transactions
```
GET    /api/transactions                    → 200 TransactionDto[]
GET    /api/transactions/{id}              → 200 TransactionDto
POST   /api/transactions                   → 201 TransactionDto  [Authorize]
PATCH  /api/transactions/{id}/confirmer    → 204  [Authorize]
PATCH  /api/transactions/{id}/annuler      → 204  [Authorize]
GET    /api/transactions/{id}/discussion   → 200 DiscussionData  [Authorize]
POST   /api/transactions/{id}/messages     → 201 MessageDto      [Authorize]
PATCH  /api/transactions/{id}/visibilite   → 200  [Authorize]
```

## Paniers
```
GET    /api/paniers?proprietaireId={guid}  → 200 PanierDto  [Authorize]
POST   /api/paniers                        → 201 PanierDto  [Authorize]
POST   /api/paniers/{id}/offres            → 200 PanierDto  [Authorize]
PATCH  /api/paniers/{id}/confirmer         → 204  [Authorize]
PATCH  /api/paniers/{id}/annuler           → 204  [Authorize]
```

## Acteurs (Profiles)
```
GET    /api/acteurs/{id}                   → 200 PersonneDto  [Authorize]
PATCH  /api/acteurs/{id}                   → 200 PersonneDto  [Authorize]
DELETE /api/acteurs/{id}                   → 204  [Authorize]  ← GDPR right to erasure
```

## Notifications
```
GET    /api/notifications/{destinataireId} → 200 NotificationDto[]  [Authorize]
PATCH  /api/notifications/{id}/read        → 204  [Authorize]
```

## Journal
```
GET    /api/journal/{acteurId}             → 200 EntreeJournalDto[]  [Authorize]
```

## Suggestions
```
GET    /api/suggestions/demande/{id}       → 200 SuggestionAppariementDto[]  [Authorize]
GET    /api/suggestions/pending            → 200 SuggestionAppariementDto[]  [Coordinateur, Responsable]
POST   /api/suggestions/demande/{id}/generer → 201  [Coordinateur, Responsable]
PATCH  /api/suggestions/{id}/acknowledge  → 204  [Authorize]
```

## Administration

### Config Catastrophe
```
GET    /api/config-catastrophe             → 200 ConfigCatastropheDto[]
POST   /api/config-catastrophe             → 201 ConfigCatastropheDto  [Responsable]
PATCH  /api/config-catastrophe/{id}        → 200 ConfigCatastropheDto  [Responsable]
```

### Rôles
```
GET    /api/roles/acteur/{acteurId}        → 200 AttributionRoleDto[]  [Coordinateur, Responsable]
POST   /api/roles                          → 201 AttributionRoleDto    [Coordinateur, Responsable]
PATCH  /api/roles/{id}/revoquer            → 204  [Responsable]
```

### Mandats
```
GET    /api/mandats/mandant/{acteurId}     → 200 MandatDto[]  [Authorize]
POST   /api/mandats                        → 201 MandatDto    [Authorize]
PATCH  /api/mandats/{id}/revoquer          → 204  [Responsable]
```

### Taxonomie
```
GET    /api/taxonomie/config/{configId}    → 200 CategorieTaxonomieDto[]
POST   /api/taxonomie                      → 201 CategorieTaxonomieDto  [Coordinateur, Responsable]
PATCH  /api/taxonomie/{id}/desactiver      → 204  [Coordinateur, Responsable]
```

### Entités (Organisations)
```
GET    /api/entites                        → 200 EntiteDto[]
GET    /api/entites/{id}                   → 200 EntiteDto
POST   /api/entites                        → 201 EntiteDto  [Responsable]
PATCH  /api/entites/{id}/desactiver        → 204  [Responsable]
```

### Méthodes d'Identification
```
GET    /api/methodes-identification/personne/{id} → 200 MethodeIdentificationDto[]  [Authorize]
PATCH  /api/methodes-identification/{id}/verifier → 200  [Coordinateur, Responsable]
```

## HTTP Status Codes

| Status | Meaning |
|--------|---------|
| 200 OK | Success (read/update) |
| 201 Created | Resource created (Location header included) |
| 204 No Content | Success (no body) |
| 400 Bad Request | Validation error (ProblemDetails with field errors) |
| 401 Unauthorized | Missing or invalid JWT |
| 403 Forbidden | Insufficient role |
| 404 Not Found | Resource not found (ProblemDetails) |
| 500 Internal Server Error | Unexpected server error |

## Role Hierarchy

| Role | Access |
|------|--------|
| `Citoyen` | Basic authenticated access |
| `Benevole` | Same as Citoyen |
| `Coordinateur` | + admin features (roles, suggestions, taxonomy) |
| `Responsable` | Full access including delete/revoke operations |
