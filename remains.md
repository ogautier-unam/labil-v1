# CrisisConnect — Rapport d'audit des fonctionnalités manquantes

**Date :** 2026-03-02 · **Mis à jour :** session 27
**Sources analysées :** Énoncé IHDCM032 · ROADMAP.md · Diagrammes de classes P1–P7 · Code source (`src/`)
**État courant :** 433 tests passants · 0 erreur de build

---

## Légende

| Symbole | Signification |
|---------|---------------|
| 🔴 | Fonctionnalité centrale explicitement requise, absente |
| 🟠 | Modèle domaine ou comportement métier incomplet |
| 🟡 | Détail manquant ou écart mineur |
| 🔵 | Exigence non-fonctionnelle non couverte |
| ✅ | Résolu |

---

## 1. Fonctionnalités centrales 🔴

### ✅ 1.1 Notifications automatiques *(résolu session 22)*

`InitierTransactionCommandHandler` notifie `proposition.CreePar` ; `ConfirmerTransactionCommandHandler` notifie les deux parties ; `AnnulerPanierCommandHandler` notifie les propriétaires d'offres libérées. Deux nouveaux `TypeNotification` ajoutés (`TransactionInitiee`, `TransactionConfirmee`).

---

### ✅ 1.2 Traduction automatique *(résolu session 22)*

`EnvoyerMessageCommandHandler` injecte `IServiceTraduction` et traduit vers `"fr"` si la langue source est différente. `Discussion.AjouterMessage()` accepte `issueTraductionAuto` + `texteOriginal`. Pattern Adapter effectivement utilisé.

---

### ✅ 1.3 Badge d'authenticité (NiveauBadge) *(résolu session 22)*

`Acteur.GetNiveauBadge()` abstract ; `Personne` : badge selon meilleure méthode vérifiée (TresHaute/Haute→Vert, Moyenne→Orange, sinon Rouge) ; `Entite` : Vert si `EstActive`, sinon Rouge.

---

### ✅ 1.4 Modification d'une proposition *(résolu session 23)*

`Proposition.ModifierContenu()` (garde : bloque si EnTransaction ou Cloturee) ; `Offre.Modifier()` + `Demande.Modifier()` ; `UpdateOffreCommand/Handler/Validator` + `UpdateDemandeCommand/Handler/Validator` ; endpoints `PATCH /api/propositions/offres/{id}` et `/demandes/{id}` `[Authorize]`. Formulaires Web : OffreEdit + DemandeEdit (session 25).

---

### ✅ 1.5 Archivage automatique planifié *(résolu session 23)*

`ArchivageAutomatiqueService` (`BackgroundService`, cycle 1 h) : lit `ConfigCatastrophe` active, marque en relance si inactif > `(DelaiArchivage − DelaiRappel)` j, archive si en attente > `DelaiRappelAvantArchivage` j. Enregistré via `AddHostedService<>`.

---

### ✅ 1.6 Recyclage d'une proposition archivée *(résolu session 22)*

`Proposition.Recycler()` (`Archivee → Active`) ; `RecyclerPropositionCommand/Handler/Validator` ; endpoint `PATCH /api/propositions/{id}/recycler` `[Coordinateur,Responsable]` ; `AuditBehaviour` : `RecyclageProposition`.

---

## 2. Comportements métier incomplets 🟠

### ✅ 2.1 Logique OU dans le Composite Demande *(résolu session 22)*

`Demande.ClorerAlternativesOu(sousDemandeSatisfaiteId)` : clore les sœurs d'un parent OU. `ClorePropositionCommandHandler` injecte `IDemandeRepository` et applique la propagation OU.

---

### ✅ 2.2 Relation Offre → Demandes couplées *(résolu session 24)*

`Offre._demandesCouplees` (backing field) + `CouplerDemande(Demande)` ; `OffreConfiguration` : `HasMany.WithMany.UsingEntity("offre_demandes_couplees")` + `HasField` ; `OffreRepository` : `Include(DemandesCouplees)` ; migration `AddOffreDemandesCouplees` ; `OffreDto.DemandesCouplees` + `CreateOffreCommand.DemandeIds`.

---

### 2.3 Types spécialisés de propositions — tous résolus ✅

| Type domaine | État |
|---|---|
| `DemandeQuota` + `IntentionDon` | ✅ résolu session 24 — use cases + API (7 endpoints) + tests |
| `PropositionAvecValidation` | ✅ résolu session 24 — Create/Valider/RefuserValidation + API + tests |
| `DemandeSurCatalogue` + `LigneCatalogue` | ✅ résolu session 26 — use cases + API + Web |
| `DemandeRepartitionGeo` | ✅ résolu session 26 — use cases + API + Web |

---

### ✅ 2.4 Profil acteur *(résolu session 25)*

`GET /api/acteurs/{id}` et `PATCH /api/acteurs/{id}` (session 24) + `Pages/Profil/Index` : consulter et modifier son profil, badge NiveauBadge visible (Vert/Orange/Rouge) (session 25).

---

### ✅ 2.5 Médias attachés aux propositions *(résolu session 27)*

`IMediaRepository` + `MediaRepository` ; `AttacherMediaCommand/Handler/Validator` + `GetMediasByPropositionQuery/Handler` ; `MediasController` : `GET + POST /api/propositions/{id}/medias` ; Web : `Pages/Propositions/Medias.cshtml` (galerie photos + formulaire attach).

---

### ✅ 2.6 `Coordonnees.adresseLibelle` manquant *(résolu session 27)*

`Localisation(lat, lon, adresseLibelle? = null)` — champ optionnel ajouté. `PropositionConfiguration` : colonne `adresse_libelle` (max 500, nullable). Migration `AddLocalisationAdresseLibelle`.

---

### ✅ 2.7 IntentionDon — workflow complet *(résolu session 24)*

`CreateDemandeQuota`, `SoumettreIntentionDon`, `AccepterIntentionDon`, `RefuserIntentionDon`, `ConfirmerIntentionDon` : Command + Handler + Validator × 5. `DemandesQuotaController` : 7 endpoints. 16 tests.

---

### ✅ 2.8 PropositionAvecValidation — workflow complet *(résolu session 24)*

`CreatePropositionAvecValidation`, `ValiderProposition`, `RefuserValidationProposition` : Command + Handler + Validator × 3. Endpoints `POST avec-validation`, `PATCH valider`, `PATCH refuser-validation`. 5 tests.

---

## 3. Endpoints API manquants 🟠

| Endpoint | État |
|---|---|
| `GET /api/acteurs/{id}` | ✅ résolu session 24 |
| `PATCH /api/acteurs/{id}` | ✅ résolu session 24 |
| `DELETE /api/acteurs/{id}` | ✅ résolu session 27 (RGPD — anonymisation) |
| `GET /api/entites/{id}` | ✅ résolu session 23 |
| `PATCH /api/propositions/offres/{id}` | ✅ résolu session 23 |
| `PATCH /api/propositions/demandes/{id}` | ✅ résolu session 23 |
| `POST /api/demandes-quota` | ✅ résolu session 24 |
| `POST /api/demandes-quota/{id}/intentions` | ✅ résolu session 24 |
| `PATCH /api/demandes-quota/{id}/intentions/{iId}/accepter` | ✅ résolu session 24 |
| `PATCH /api/demandes-quota/{id}/intentions/{iId}/refuser` | ✅ résolu session 24 |
| `PATCH /api/demandes-quota/{id}/intentions/{iId}/confirmer` | ✅ résolu session 24 |
| `POST /api/propositions/avec-validation` | ✅ résolu session 24 |
| `PATCH /api/propositions/{id}/valider` | ✅ résolu session 24 |
| `GET + POST /api/propositions/{id}/medias` | ✅ résolu session 27 |
| ~~Écart rôle bascule visibilité~~ | ✅ résolu session 22 — `[Authorize]` simple |

---

## 4. Pages Web manquantes

| Page | État |
|---|---|
| `Pages/Profil/` (Mon profil) | ✅ résolu session 25 |
| `Pages/Entites/Detail.cshtml` | ✅ résolu session 25 |
| `Pages/Propositions/OffreEdit.cshtml` | ✅ résolu session 25 |
| `Pages/Propositions/DemandeEdit.cshtml` | ✅ résolu session 25 |
| Bouton bascule visibilité dans `Discussion.cshtml` | ✅ résolu session 25 |
| `Pages/Propositions/DemandesQuota.cshtml` | ✅ résolu session 25 |
| `Pages/Propositions/AvecValidation.cshtml` | ✅ résolu session 25 |
| Pages DemandeSurCatalogue, DemandeRepartitionGeo | ✅ résolu session 26 |
| `Pages/Propositions/Medias.cshtml` | ✅ résolu session 27 |

---

## 5. Exigences non-fonctionnelles non couvertes 🔵

### NF-02 — Responsive (obligatoire)

Bootstrap 5.3 est en place mais les pages n'ont pas été testées/optimisées pour mobile.

---

### NF-04 — Multilingue (obligatoire)

L'interface Web est uniquement en français. `LanguePreferee` de `Personne` n'est pas utilisée. `NomJson`/`DescriptionJson` des catégories ne sont jamais désérialisés selon la langue.

---

### NF-05 — WCAG (obligatoire)

Accessibilité non vérifiée : `alt` sur les images, `aria-label` sur les boutons, contraste du thème rouge Bootstrap.

---

### ✅ NF-06 — RGPD / Privacy by Design *(résolu session 27)*

`Personne.Anonymiser()` : efface email, nom, prénom, téléphone, adresse, photo, langue, moyens de contact. `SupprimerActeurCommand/Handler/Validator`. `DELETE /api/acteurs/{id}` → 204. Intégrité référentielle préservée (soft delete par pseudonymisation).

---

### ✅ NF-07 — Bande passante minimisée *(résolu session 25)*

`AddResponseCompression(EnableForHttps=true)` + `UseResponseCompression()` dans `Program.cs` API (Brotli + Gzip intégrés ASP.NET Core).

---

### ✅ NF-10 — Documentation anglaise dans `/doc` *(résolu session 27)*

`doc/architecture.md` (Clean Architecture, patterns, domain model) + `doc/api-reference.md` (60+ routes, codes HTTP, rôles) + `doc/setup.md` (prérequis, Quick Start, migrations, troubleshooting).

---

### ✅ NF-11 — Extensibilité *(résolu session 27)*

- **Stratégies de mise en avant** : `IStrategiePriorisation.Nom` ajouté ; 4 implémentations (`anciennete|urgence|region|type`) enregistrées via `IEnumerable<IStrategiePriorisation>` ; `GET /api/propositions/demandes?strategie=urgence` activé.
- **Plugins de matching** : génération de suggestions (Jaccard) hard-codée dans `GenererSuggestionsCommandHandler` — non extensible par plugin.

---

## 6. Écarts mineurs 🟡

| Écart | État | Référence |
|---|---|---|
| `Acteur.getNiveauBadge()` abstraite | ✅ résolu session 22 | Diagramme P1 |
| `Coordonnees.adresseLibelle` | ✅ résolu session 27 | Diagramme P4 |
| Rappel expiration de rôle | ✅ résolu session 27 — `RappelExpirationRoleService` BackgroundService | Énoncé §5 ex.6 |
| Demande récurrente | 🟡 absent | ROADMAP §4.7 |
| Recherche par texte libre | 🟡 seuls `?statut=`, `?urgence=` et `?strategie=` supportés | Énoncé §5 ex.11 |
| Image Docker publiée sur GitHub | 🟡 absent | ROADMAP DoD §8.2 |

---

## Synthèse par priorité

### Priorité haute — ✅ Tous résolus

| # | Fonctionnalité | État |
|---|---|---|
| H1 | Notifications automatiques dans les handlers | ✅ session 22 |
| H2 | Traduction automatique dans `EnvoyerMessageCommandHandler` | ✅ session 22 |
| H3 | Calcul `NiveauBadge` sur `Acteur` | ✅ session 22 |
| H4 | Use case + endpoint modification d'une proposition | ✅ session 23 |
| H5 | `BackgroundService` archivage automatique | ✅ session 23 |
| H6 | Recyclage proposition archivée (`Archivee → Active`) | ✅ session 22 |

### Priorité moyenne — ✅ Tous résolus

| # | Fonctionnalité | État |
|---|---|---|
| M1 | Logique OU dans Composite Demande | ✅ session 22 |
| M2 | Relation `Offre → DemandesCouplees` | ✅ session 24 |
| M3 | Workflow `DemandeQuota` + `IntentionDon` (API + Web) | ✅ session 24-25 |
| M4 | Workflow `PropositionAvecValidation` (API + Web) | ✅ session 24-25 |
| M5 | Profil acteur (API + Web) | ✅ session 24-25 |
| M6 | Médias attachés aux propositions | ✅ session 27 |
| M7 | Correction rôle bascule visibilité discussion | ✅ session 22 |
| M8 | Page detail entité + endpoint `GET /api/entites/{id}` | ✅ session 23-25 |

### Priorité basse — Non-fonctionnel / Qualité

| # | Exigence | État |
|---|---|---|
| L1 | NF-04 Multilingue — désérialisation `NomJson` selon langue | 🔵 non démarré |
| L2 | NF-05 WCAG — audit accessibilité + corrections | 🔵 non démarré |
| L3 | NF-06 RGPD — politique + suppression données | ✅ session 27 (pseudonymisation) |
| L4 | NF-07 Compression réponses API | ✅ session 25 |
| L5 | NF-10 Documentation `/doc` en anglais | ✅ session 27 |
| L6 | Stratégies de mise en avant — branchement effectif | ✅ session 27 (NF-11) |
| L7 | Recherche avancée (fulltext, catégorie, géolocalisation) | 🔵 non démarré |
| L8 | Image Docker publiée sur GitHub Container Registry | 🔵 non démarré |
| L9 | `Coordonnees.adresseLibelle` manquant | ✅ session 27 |
| L10 | Demande récurrente (scénario Grosemilo) | 🟡 non démarré |
| L11 | Rappel expiration de rôle (scheduler) | ✅ session 27 |

---

## Ce qui est conforme ✅

- Architecture Clean Architecture (5 projets, règle de dépendance respectée)
- Pattern CQRS via Mediator 3.x MIT (handlers complets)
- Pattern Composite sur `Demande` (structure ET/OU — logique OU implémentée)
- Pattern Strategy sur `IStrategiePriorisation` (4 implémentations — branchées via `?strategie=`)
- Pattern Adapter sur `IServiceTraduction` (3 adaptateurs — effectivement utilisé dans `EnvoyerMessageCommandHandler`)
- 8 types de `MethodeIdentification` (TPH)
- `ConfigCatastrophe` + `CategorieTaxonomie` extensible dynamiquement
- Journal d'audit structuré (48 opérations mappées dans `AuditBehaviour`)
- JWT + cookies HttpOnly + refresh tokens
- Docker Compose (API + DB + Web)
- 433 tests unitaires (0 échec) — couverture handlers 100%, validators 100%, repos 100%
- Pages Web complètes : Profil, OffreEdit, DemandeEdit, Entités/Detail, DemandesQuota, AvecValidation, bascule visibilité Discussion, Médias
- Compression API (NF-07) : Brotli + Gzip via UseResponseCompression
- Taxonomy Web configurable sans redéploiement
- Mandats + rôles temporels avec portée configurable
- Suggestions d'appariement (score Jaccard + bonus urgence)
- Notifications automatiques (transactions + paniers)
- Traduction automatique des messages (AdaptateurDeepL/LibreTranslate/CorpusInterne)
- Badge d'authenticité calculé dynamiquement (`GetNiveauBadge()`)
- Archivage automatique planifié (`ArchivageAutomatiqueService` BackgroundService)
- Rappel expiration de rôle planifié (`RappelExpirationRoleService` BackgroundService)
- Recyclage de propositions archivées
- Modification de propositions (UpdateOffre + UpdateDemande)
- Workflow DemandeQuota + IntentionDon complet (API + Web)
- Workflow PropositionAvecValidation complet (API + Web)
- Profil acteur GET + PATCH + DELETE/anonymisation RGPD
- Relation Offre → DemandesCouplees (many-to-many)
- Workflow DemandeSurCatalogue + LigneCatalogue complet (use cases + API + Web)
- Workflow DemandeRepartitionGeo complet (use cases + API + Web)
- Médias attachés aux propositions (use cases + API + Web galerie)
- Localisation avec libellé d'adresse (`adresseLibelle?`)
- Documentation anglaise `/doc` (architecture + API reference + setup)
- NF-11 Extensibilité : stratégies de priorisation activables via `?strategie=`
- NF-06 RGPD : droit à l'oubli via `DELETE /api/acteurs/{id}` (pseudonymisation)
