# CrisisConnect — Roadmap Projet

**Cours** : IHDCM032 – Laboratoire d'ingénierie du logiciel
**Promoteur** : V. Englebert, Prof. – Université de Namur
**Année** : 2025-2026
**Sources** : Énoncé (13 fév. 2026) · LABIL.pdf · mdl-méthode.pdf (20 fév. 2026) · Scrum.pdf

> *« La méthode et la qualité du résultat importent plus que sa taille. »* — LABIL.pdf

---

## Table des matières

1. [Contexte & Vision](#1-contexte--vision)
2. [Acteurs, Rôles & Terminologie](#2-acteurs-rôles--terminologie)
3. [Exigences fonctionnelles détaillées](#3-exigences-fonctionnelles-détaillées)
4. [Scénarios concrets de l'énoncé](#4-scénarios-concrets-de-lénoncé)
5. [Contraintes non-fonctionnelles](#5-contraintes-non-fonctionnelles)
6. [Architecture & Conception](#6-architecture--conception)
7. [Organisation & Méthode Rabot](#7-organisation--méthode-rabot)
8. [Rôles Scrum & Definition of Done](#8-rôles-scrum--definition-of-done)
9. [Délivrables par sprint — Templates & Exemples](#9-délivrables-par-sprint--templates--exemples)
10. [Phases de développement](#10-phases-de-développement)
11. [User Stories exemples (CrisisConnect)](#11-user-stories-exemples-crisisconnect)
12. [Décisions technologiques à prendre](#12-décisions-technologiques-à-prendre)
13. [Évaluation](#13-évaluation)
14. [Points de vigilance](#14-points-de-vigilance)
15. [Ressources disponibles](#15-ressources-disponibles)

---

## 1. Contexte & Vision

### 1.1 Genèse du projet *(énoncé §1)*

Les **inondations belges de juillet 2021** ont causé plus de 40 morts et des milliards de dégâts en Wallonie. Elles ont mis en lumière un paradoxe : une aide citoyenne spontanée et massive s'est manifestée (particuliers, entreprises, associations), mais **aucune infrastructure numérique** ne permettait de la coordonner efficacement. La coordination s'est opérée sur les réseaux sociaux, par des initiatives locales et individuelles — sans système, sans traçabilité, sans matching intelligent.

**CrisisConnect** est la réponse à ce vide. C'est une plateforme de médiation open source, déployable par toute entité étatique ou caritative, pour tout type de catastrophe naturelle (inondation, incendie, séisme, tsunami, pandémie...), dès lors qu'une infrastructure de communication minimale existe (ADSL, 4/5G, Starlink).

### 1.2 Mission du système

```
  +---------------------------------------------------------------+
  |                    CRISE NATURELLE                            |
  |                                                               |
  |  Sinistres, collectivites        Benevoles, organisations     |
  |  Personnes dans le besoin        Particuliers genereux        |
  |                                                               |
  |       DEMANDES ---------> CrisisConnect <---------  OFFRES    |
  |                                 |                             |
  |                            MATCHING                           |
  |                    (Discussion, Transaction)                  |
  +---------------------------------------------------------------+
```

La plateforme **ne remplace pas l'État** : elle complète les services officiels (112, 1722, assurances, démarches administratives sont hors périmètre).

### 1.3 Caractéristiques fondamentales

| Dimension | Description |
|-----------|-------------|
| **Open source** | Appropriable par des entités étatiques ou caritatives ; licence la plus permissive possible |
| **Générique** | Configurable pour tout type de catastrophe, tout pays |
| **Extensible** | Taxonomie, plugins de matching, types de propositions, modes d'auth |
| **Confiance** | Identification multi-niveaux + badges d'authenticité (vert/orange/rouge) |
| **Accessible** | WCAG, responsive, bande passante minimisée |
| **Multilingue** | Configuration par zone + choix individuel de langue |
| **Gratuit** | Aucune contribution financière ; pas de publicité commerciale |

---

## 2. Acteurs, Rôles & Terminologie

### 2.1 Les cinq rôles *(énoncé §3.1)*

Les rôles sont **cumulables** sur un même acteur. Un participant au projet peut porter plusieurs rôles simultanément.

| Rôle | Abréviation | Qui ? | Responsabilités clés |
|------|-------------|-------|----------------------|
| **Contributeur** | — | Développeur tiers (GitHub) | Contribue au code source via PR ; pas d'accès système |
| **Utilisateur** | U | Sinistré ou aidant | Dépose offres/demandes, participe aux transactions, gère son profil |
| **Admin Installeur** | AI | Technicien déploiement | Installe la plateforme, configure l'infrastructure, gère les composants logiciels |
| **Admin Catastrophe** | AC | Expert métier contexte | Configure la plateforme selon la catastrophe (taxonomie, langues, identifications, entités) |
| **Admin Système** | AS | Administrateur quotidien | Gère les comptes, modère, supervise le journal, ajoute/supprime entités |

> **Note de conception** : les rôles AI, AC, AS sont distincts par leurs privilèges mais peuvent être tenus par les mêmes personnes physiques. La modélisation doit refléter cette séparation des responsabilités.

### 2.2 Types d'acteurs *(énoncé §5 ex.4)*

```
Acteur (abstrait)
  +-- Personne physique
  |     +-- Peut avoir un ou plusieurs roles (AttributionRole)
  |     +-- Peut mandater un tiers (Mandat -> autre Acteur)
  |     +-- S'authentifie par >= 1 MethodeIdentification
  +-- Entite (organisme reconnu par l'AC)
        +-- Exemples : MSF, Croix-Rouge, ASBL, scouts, commune
        +-- Dispose d'une page de presentation publique
        +-- Rattachee a une Personne responsable formellement identifiee
```

### 2.3 Glossaire *(énoncé §3.2)*

| Terme | Définition |
|-------|-----------|
| **Proposition** | Terme générique désignant une Offre **ou** une Demande |
| **Offre** | Aide proposée spontanément par un utilisateur |
| **Demande** | Aide sollicitée par un utilisateur |
| **Entité** | Organisme reconnu dont la présence est validée par l'AC |
| **État référent** | Entité politique et juridique couvrant la zone sinistrée (une seule par déploiement) |
| **Transaction** | Engagement en cours entre parties sur une proposition |
| **Panier** | Ensemble cohérent d'offres sélectionnées par un utilisateur |
| **Badge** | Indicateur visuel de fiabilité de l'identité (vert/orange/rouge) |
| **Mandat** | Délégation formelle permettant à un tiers d'agir au nom d'un acteur |

---

## 3. Exigences fonctionnelles détaillées

### 3.1 Gestion de la plateforme (configuration) *(énoncé §5 ex.1-6)*

#### Ex.1 — Installation par l'AI
L'AI configure la plateforme afin qu'elle soit **opérationnelle** et installe tous les composants logiciels nécessaires. C'est le point d'entrée unique avant tout déploiement.

#### Ex.2 — Configuration par l'AC
L'AC configure la plateforme selon la catastrophe en cours :
- **Taxonomie** des propositions (catégories extensibles)
- **Langues actives** dans la zone (pas uniquement les langues officielles — dialectes exclus)
- **Modes d'identification** acceptés (selon infrastructure disponible dans la zone)
- **Catalogues** de produits/offres types
- **Délai d'archivage** des propositions (paramètre *n* jours)

#### Ex.3 — Taxonomie extensible
La taxonomie (cf. §5.3) est un point de départ, **extensible par l'AC** sans redéploiement. Les nouvelles catégories et sous-catégories doivent pouvoir être ajoutées dynamiquement.

#### Ex.4 — Gestion des types d'acteurs
- Personnes, Entités, Personnes avec rôles accrédités par une Entité
- Chaque Entité est associée à une Personne **formellement identifiée** ayant autorité pour accréditer des rôles

#### Ex.5 — Gestion des entités
L'AS et/ou l'AC peuvent ajouter ou supprimer une entité **au cours du temps**, y compris en cours de catastrophe.

#### Ex.6 — Rôles limités dans le temps
- Les rôles peuvent être accordés pour une **durée limitée** et/ou **reconductibles**
- Mécanisme de **rappel automatique** avant expiration d'un rôle non reconduit

### 3.2 Identification & Authentification *(énoncé §5 ex.7-8)*

#### Ex.7 — Identification multi-modale

Ces méthodes ne s'appliquent qu'à la **création de compte** ou au **rétablissement de confiance** (pas à chaque connexion) :

| Méthode | Fiabilité estimée | Disponibilité en crise |
|---------|------------------|----------------------|
| Carte d'identité électronique (eID) | Très haute | Limitée (perte possible) |
| Opération bancaire (virement de 1 ct) | Haute | Bonne si réseau bancaire opérationnel |
| SMS / messagerie sur numéro mobile | Moyenne-haute | Bonne si réseau mobile opérationnel |
| Facture d'électricité récente | Moyenne | Limitée (documents souvent perdus) |
| Numéro de téléphone fixe | Moyenne | Variable |
| Délégation par tiers identifié | Faible (explicitement) | Très bonne — dernier recours |
| Parrainage par *n* utilisateurs identifiés | Variable selon *n* | Bonne |

> **Enjeu critique** : des sinistrés peuvent avoir **perdu tous leurs documents** (portefeuille, téléphone, papiers). La plateforme doit pouvoir accueillir ces personnes tout en maintenant un niveau de confiance traçable.

#### Ex.8 — Login/Password
Minimum absolu, obligatoire. D'autres modalités de connexion quotidienne peuvent être ajoutées (SSO, biométrie, etc.).

### 3.3 Mandats & Badges *(énoncé §5 ex.9, §5 ex.14)*

#### Ex.9 — Mandat à tiers
Un utilisateur peut donner un **mandat formel** à un tiers pour opérer sur la plateforme en son nom :
- Information **publique** et publiée en temps opportun
- Portée du mandat à définir (toutes opérations ? lecture seule ? catégorie spécifique ?)
- Flexibilité requise par la situation de crise + traçabilité des responsabilités

#### Ex.14 — Badges d'authenticité
Mécanisme visuel permettant de **cerner rapidement la fiabilité d'une identité** :

```
  [VERT]    Identite verifiee par methode fiable (eID, bancaire)
  [ORANGE]  Identite verifiee par methode moins formelle (SMS, facture)
  [ROUGE]   Identite non verifiee ou verifiee par delegation seulement
```

La modalité d'authentification est connue pour chaque utilisateur et visible sur son profil.

### 3.4 Journal, Profils & Entités *(énoncé §5 ex.10-13)*

#### Ex.10 — Journal d'audit
Journal de **toutes les opérations** réalisées sur la plateforme :
- Structuré pour permettre une **analyse automatique** (pas un simple log texte)
- Doit couvrir : créations/modifications de propositions, transactions, connexions, modifications de rôles, mandats, etc.

#### Ex.11 — Recherche & Filtres
La plateforme peut héberger un **nombre considérable de propositions**. L'interface doit :
- Offrir filtres, critères, recherches simples et complexes
- Géolocalisation et localisation sur carte
- Affichage **souple et intelligent** s'adaptant dynamiquement aux critères (pas de "noyade" dans les résultats)

#### Ex.12 — Profils utilisateurs
Profil contenant des informations **utiles aux objectifs** de la plateforme (pas un réseau social) :
- Adresse, moyens de contact, photo
- Badge d'authenticité visible

#### Ex.13 — Pages d'entités
Les entités validées disposent d'une **page de présentation synthétique** adaptée au contexte :
- Comment faire un don
- Moyens de contact
- Nature de leurs contributions

### 3.5 Traduction *(énoncé §5 ex.15-16)*

#### Ex.15 — Traduction automatique
- Textes disponibles via **corpus interne** (défaut) ou **traducteur automatique**
- **Mention obligatoire** : l'utilisateur sait si le texte est traduit automatiquement
- La **version originale** reste toujours consultable

#### Ex.16 — Indépendance du fournisseur
L'architecture doit être **indépendante** du choix de l'opérateur de traduction :
- Interface d'abstraction obligatoire (patron Strategy ou similaire)
- Permet de changer de fournisseur (DeepL, Google Translate, LibreTranslate...) sans modifier le reste du système

### 3.6 Offres & Demandes *(énoncé §5.1)*

#### Cycle de vie d'une proposition

```
          [CREATION]
               |
               v
          [ACTIVE] --------------------------------- Edition possible
               |
     +---------+------------------+
     |                            |
     v                            v
[EN_TRANSACTION]         [EN_ATTENTE_RELANCE] --- (rappel proprietaire)
  Edition bloquee                 |         |
     |                  (confirme)|  (delai |
     v                            v  ecoule)|
[CLOTUREE] <---------       [ACTIVE]    [ARCHIVEE] --> recyclable
  Invisible users         (reactif)      sans limite
  Reste archivee
```

#### Offres *(énoncé §5.1.1)*

- Déposable par toute **personne identifiée**
- Formulaires pré-conçus adaptés au type d'offre (champs spécifiques selon la catégorie)
- Données possibles : **géolocalisation, photos, vidéos, enregistrements** (smartphone)
- Une offre peut être déposée conjointement avec des **demandes associées** (ex. offrir une étable + demander transport + fourrage)
  - Une offre satisfaite à 75% peut voir ses demandes non satisfaites **propulsées en avant** sur la plateforme

#### Demandes *(énoncé §5.1.2)*

- Déposable par toute **personne identifiée**
- Stratégie de **mise en avant extensible** (plugins) :
  - Par ancienneté
  - Par degré d'urgence
  - Par type
  - Par région selon la sévérité de la catastrophe
- Demande **composite** (pattern Composite) :

```
  Demande composite (ET) :
    +-- J'ai besoin d'une etable pour mes betes
    +-- ET d'un transporteur
  => Les deux doivent etre satisfaites

  Demande composite (OU) :
    +-- J'ai besoin d'un appartement
    +-- OU d'une maison
  => La cloture d'une branche entraine la cloture de l'autre
  => Cloture recursive sur les demandes complexes imbriquees
```

### 3.7 Propositions spécifiques *(énoncé §5.1.3)*

Ces types étendent la plateforme avec des **traitements spécifiques d'encodage et de matching** :

| Type | Cas d'usage réel | Fonctionnalité clé |
|------|-----------------|-------------------|
| **Demande sous quota** | Convoi Toufeutouflam (6×90m³) | Enregistrement d'intentions + validation selon capacité + confirmation des promesses + notification accept/refus |
| **Répartition géo RH** | Nettoyage Rochebranle (bénévoles) | Outil GIS pour visualiser les demandes géolocalisées et répartir les bénévoles dans le temps et l'espace |
| **Validation tiers** | Aide psychologique, aide à l'enfance | Tiers de confiance (entité reconnue) valide les offres avant qu'elles soient visibles aux sinistrés |
| **Besoins sur catalogues** | École de Clairval (matériel pédagogique) | Intégration catalogue e-commerce, listing chiffré actualisé en temps réel, possibilité d'achat en ligne |

### 3.8 Matching *(énoncé §5.2)*

#### Discussion public/privé *(ex.1)*

```
  [Utilisateur A] --> (Discussion publique) --> [Plateforme] --> [Utilisateur B]
                           "Votre frigo a-t-il un freezer ?"

  [Utilisateur B] bascule en prive :
  [Utilisateur A] <-- (Discussion privee)  <-- [Utilisateur B]
                           "Voici mon 0477/666.666"
```
Les deux parties peuvent basculer public <-> privé à tout moment.

#### Panier *(ex.2-3)*
- Sélection d'un **ensemble cohérent** d'offres complémentaires
- La plateforme **ne supervise pas** la cohérence (responsabilité de l'utilisateur)
- Annulation après confirmation → offres **remises disponibles** + **notification automatique** aux propriétaires

#### Suggestion automatique *(ex.4)*
- Quand les données le permettent (géolocalisation, nature) : suggestion d'appariements offres <-> demandes
- Critères : proximité géographique, correspondance de catégorie, disponibilité

### 3.9 Taxonomie de base *(énoncé §5.3)*

Taxonomie **librement inspirée de [1]**, adaptée aux **besoins individuels** (pas les interventions de masse des services de secours).

| # | Catégorie | Exemples concrets |
|---|-----------|------------------|
| 1 | **Logement** | Hébergement famille, chambre d'appoint, mobile home, tente |
| 2 | **Évacuation** | Transport personnes, logistique déménagement, camion |
| 3 | **Soins personnels** | Médicaments (continuité), produits hygiène, lunettes, appareils auditifs |
| 4 | **Informations contextualisées** | Décrue de la rivière X, progression incendie vers Y, coupure eau à Z |
| 5 | **Offre/demande d'emploi** | Mission bénévole, remplacement temporaire |
| 6 | **Aide financière** | Don monétaire, micro-prêt, collecte |
| 7 | **Assistance médicale** | Soutien psychologique, médecin de garde (non-urgent) |
| 8 | **Fournitures** | Mobilier, électroménager (frigo, plaques), vêtements, GSM, ordinateur, kayak, zodiac |
| 9 | **Aide logistique** | Nettoyage, maçonnerie, bâchage, débourbement |

> Taxonomie **ni exhaustive ni normative** — extensible par l'AC en cours de déploiement.

---

## 4. Scénarios concrets de l'énoncé

L'énoncé fournit **8 scénarios** qui doivent guider la conception. Chacun illustre un aspect particulier du système.

### 4.1 Recherche de proches *(§5.4.1)*
> **Contexte** : Marcel cherche ses enfants disparus après une inondation.
>
> **Propositions impliquées** : Demande d'aide à la localisation, photos + description, diffusion multi-canaux.
>
> **Aspects à modéliser** : Formulaire avec photos/vidéos, champ "personne disparue", diffusion géolocalisée, réponses publiques.
>
> **Catégorie** : Informations contextualisées (#4) voire Logement (#1) si retrouvée.

### 4.2 Élevage — offre + demandes couplées *(§5.4.2)*
> **Contexte** : Inondation à Liège, ~100 bovins à déménager urgemment.
> - Bernard (Tournai) : héberge 50 bovins, **a besoin de** fourrage + transport
> - Charline (Redu) : héberge 40 bovins, **dispose d'un camion**, **a besoin de** fourrage
>
> **Propositions impliquées** :
> ```
> [Offre Bernard] hebergement 50 bovins
>   +-- [Demande couplee] transport des bovins depuis Liege
>   +-- [Demande couplee] fourrage pour n semaines
>
> [Offre Charline] hebergement 40 bovins + transport
>   +-- [Demande couplee] fourrage pour n semaines
> ```
>
> **Aspects à modéliser** : Offre couplée à des demandes, gestion de la satisfaction partielle (75% → propulsion des demandes restantes).

### 4.3 École de Clairval *(§5.4.3)*
> **Contexte** : École primaire sinistrée, matériel pédagogique détruit, travaux de réfection nécessaires.
>
> **Propositions impliquées** :
> - Demande (ET) : bénévoles nettoyage + électricien + menuisier
> - Demande sur catalogue : craies, tables, papeterie, ordinateurs (liste précise, quantités)
>
> **Aspects à modéliser** : Demande composite ET, intégration catalogue e-commerce (§5.1.3), entité reconnue (école).

### 4.4 LaBricole — répartition équitable *(§5.4.4)*
> **Contexte** : Chaîne de magasins offre des palettes d'outils, veut une répartition équitable par les villes selon les besoins.
>
> **Aspects à modéliser** : Offre sous quota (capacité des palettes), répartition géographique GIS, entité (LaBricole), coordination avec entités municipales.

### 4.5 Rochebranle — collecte d'urgence *(§5.4.5)*
> **Contexte** : Tremblement de terre, communications rompues, habitants démunis. Le bourgmestre (centre de crise) organise une collecte.
>
> **Propositions impliquées** : Demandes tentes, eau potable, conserves, lampes, réchauds, gaz.
>
> **Aspects à modéliser** : Acteur en situation de crise avec infrastructure limitée, rôle AC pour configuration rapide, bande passante minimisée, demandes sous quota (capacité d'accueil).

### 4.6 Toufeutouflam — convoi sous quota *(§5.4.6)*
> **Contexte** : Ville jumelée (Tincelle) organise un convoi avec l'entreprise Turbo (6×90m³). Appréhende d'être débordée.
>
> **Propositions impliquées** : Demande sous quota (6 camions × 90m³) pour vêtements, conserves, tentes.
>
> **Scénario détaillé** :
> 1. Tincelle crée une "Demande sous quota" avec capacité 540m³ et date limite D
> 2. Les aidants enregistrent leurs **intentions de don** (nature + volume estimé)
> 3. Le système valide les intentions selon la capacité disponible
> 4. Notification : "votre don est accepté" ou "dépassement capacité, dépôt reporté"
> 5. Confirmation des promesses (qui a effectivement déposé ?)
>
> **Aspects à modéliser** : Demande sous quota, gestion intentions + confirmation, notifications automatiques.

### 4.7 Grosemilo — demande composite récurrente *(§5.4.7)*
> **Contexte** : Association "sguatteri" distribue des repas chauds chaque week-end. Ils ont la main d'œuvre et les compétences, besoin de matériel.
>
> **Besoin hebdomadaire** (demande composite ET) :
> ```
> [Demande sguatteri (ET)] :
>   +-- 15 points de chauffe (rechauds gaz OU barbecues avec combustible)
>   +-- 1 camion refrigere
>   +-- Tables et chaises pliantes
>   +-- Tentes type militaire (pour cantines par mauvais temps)
> ```
>
> **Aspects à modéliser** : Demande composite ET avec sous-demande OU, récurrence (chaque week-end), entité reconnue (association).

### 4.8 Doodoocity — 40 000 personnes sans gaz *(§5.4.8)*
> **Contexte** : Incident technique en plein hiver, 40 000 personnes privées de gaz. Besoins multiples et variés.
>
> **Propositions impliquées** :
> - Demandes : couvertures/couettes, plaques chauffantes, ateliers artisans (boulangerie), réchauds gaz
> - Entraide : amener eau chaude aux personnes âgées, vérifications régulières
>
> **Aspects à modéliser** : Volume massif de propositions (filtres/recherche critique), entraide directe entre personnes, répétition de besoins similaires (agrégation intelligente ?), contexte hivernal urbain.

---

## 5. Contraintes non-fonctionnelles

### 5.1 Tableau de synthèse *(énoncé §2, §4)*

| ID | Contrainte | Priorité | Détail & Implications |
|----|------------|----------|----------------------|
| **NF-01** | Open source | Obligatoire | Licence permissive ; aucune dépendance propriétaire payante ; publiable |
| **NF-02** | Responsive | Obligatoire | Interface fonctionnelle sur mobile et desktop |
| **NF-03** | Application mobile | Optionnel | Android/iOS possible en projet conjoint |
| **NF-04** | Multilingue | Obligatoire | Configuration par zone ; registre standardisé (ISO 639) ; pas de dialectes |
| **NF-05** | WCAG | Obligatoire | Accessibilité maximale ; outils de validation disponibles (voir §2) |
| **NF-06** | RGPD | Obligatoire | Données de sinistrés — conformité dès la conception (Privacy by Design) |
| **NF-07** | Bande passante | Obligatoire | Minimisée — réseau dégradé possible (zone sinistrée) |
| **NF-08** | Gratuité | Obligatoire | Aucune contribution financière ; publicité interdite sauf philanthropique |
| **NF-09** | Tests | Obligatoire | Suite de tests documentés obligatoire |
| **NF-10** | Documentation | Obligatoire | En anglais, dans `/doc` sur GitHub, dès le premier sprint |
| **NF-11** | Extensibilité | Obligatoire | Nouveaux types de propositions, plugins de matching, taxonomie |
| **NF-12** | Sécurité | Obligatoire | Journal d'audit, séparation des rôles, protection données sinistrés |
| **NF-13** | Disponibilité | Élevée | Infrastructure déployable rapidement en contexte de crise |

### 5.2 Scénarios NFR prioritaires (à éliciter)

Ces scénarios sont à documenter formellement selon la méthode IALTEM (mdl-méthode §2.3) :

| Scénario | Stimulus | Réponse attendue |
|----------|----------|-----------------|
| **Charge massive** | 40 000 utilisateurs (Doodoocity) créent des propositions simultanément | Temps de réponse < 3s ; pas de perte de données |
| **Bande passante dégradée** | Réseau 2G uniquement (zone sinistrée) | Interface allégée ; images compressées ; mode offline possible |
| **Perte de document** | Sinistré sans aucun papier d'identité | Peut quand même créer un compte (délégation/parrainage) |
| **Ajout taxonomie en urgence** | AC veut ajouter "kayak/zodiac" pendant les inondations | Ajout sans redéploiement, disponible en <5 min |
| **Changement traducteur** | Fournisseur de traduction indisponible | Bascule sur autre fournisseur sans interruption de service |

---

## 6. Architecture & Conception

### 6.1 Vue d'ensemble architecturale (à affiner en Phase 2)

```
  +--------------------------------------------------------------------+
  |                              CLIENTS                               |
  |  +------------+  +-------------+  +-----------------------+        |
  |  |Web Browser |  |Mobile App   |  |Admin Interface        |        |
  |  |(Responsive)|  |(Android/iOS)|  |(AI / AC / AS)         |        |
  |  +------------+  +-------------+  +-----------------------+        |
  +--------------------------------------------------------------------+
                                | HTTPS / API REST
  +-----------------------------v--------------------------------------+
  |                            API GATEWAY                             |
  |                 (Authentification, Rate Limiting)                  |
  +---------------+---------------+-------------+----------------------+
  |Propositions   |Matching       |Identite     |Config & Admin        |
  |& Taxonomie    |& Panier       |& Auth       |(AI/AC/AS)            |
  +---------------+---------------+-------------+----------------------+
  |                        SERVICES TRANSVERSES                        |
  |  +----------+  +----------+  +---------------+                     |
  |  |Traduction|  |GIS/Carte |  |Notifications  |                     |
  |  |(abstrait)|  |          |  |               |                     |
  |  +----------+  +----------+  +---------------+                     |
  +--------------------------------------------------------------------+
  |                         COUCHE DE DONNEES                          |
  |  +-------------------+  +----------------------------+             |
  |  |Base de donnees    |  |Journal d'audit (structure) |             |
  |  |(propositions,     |  |(toutes operations)         |             |
  |  | acteurs, mandats) |  |                            |             |
  |  +-------------------+  +----------------------------+             |
  +--------------------------------------------------------------------+
```

> Cette architecture est une **proposition initiale**. Elle sera affinée en Phase 2 à partir du modèle de robustesse.

### 6.2 Diagramme de classes

Le diagramme de classes est découpé en **9 fichiers** pour une meilleure lisibilité sur A4 :

| Fichier | Contenu |
|---------|---------|
| `class-diagram.puml` | Diagramme complet (référence, ~500 lignes) |
| `class-diagram-overview.puml` | Vue d'ensemble — tous les packages, noms uniquement |
| `class-diagram-p1-acteurs.puml` | P1 : Acteurs & Rôles (détail complet) |
| `class-diagram-p2-auth.puml` | P2 : Authentification & Identification |
| `class-diagram-p3-config.puml` | P3 : Configuration & Taxonomie |
| `class-diagram-p4-propositions.puml` | P4 : Propositions / Offres / Demandes |
| `class-diagram-p5-services.puml` | P5 : Services & Stratégies (Strategy + Adapter) |
| `class-diagram-p6-matching.puml` | P6 : Matching & Transactions |
| `class-diagram-p7-notif-journal.puml` | P7+P8 : Notifications + Journal & Audit |

> Chaque diagramme par package montre le package en détail complet, avec les **références externes** (cases grises, sans membres) pour situer les dépendances inter-packages.

Le fichier `class-diagram.puml` est une base de travail à affiner sprint par sprint. Il comporte **7 packages** et **~40 classes/interfaces/enums**.

| Package | Classes & interfaces clés | Source |
|---------|--------------------------|--------|
| **Acteurs & Rôles** | `Acteur`, `Personne`, `Entite`, `AttributionRole`, `Mandat` + enums `StatutRole`, `PorteeMandat`, `NiveauBadge` | §3, §5 ex.4-6, §5 ex.9 |
| **Authentification** | `MethodeIdentification` (abstract) + **8 sous-classes** dont `VerificationPhoto` ; enum `NiveauFiabilite` | §5 ex.7-8, §5 ex.14 |
| **Configuration & Taxonomie** | `ConfigCatastrophe`, `CategorieTaxonomie` (self-référence) | §5 ex.1-3, §5.3 |
| **Propositions** | `Proposition` (abstract), `Offre`, `Demande` (Composite) + 4 types spécifiques ; `IntentionDon`, `LigneCatalogue`, `Coordonnees`, `Media` | §5.1, §5.1.1-3 |
| **Services & Stratégies** | `StrategiePriorisation` (interface) + 4 implémentations ; `ServiceTraduction` (interface) + 3 adaptateurs — **Pattern Strategy & Adapter** | §5 ex.15-16, §5.1.2 ex.1 |
| **Matching & Transactions** | `Transaction`, `Discussion`, `Message`, `Panier`, `SuggestionAppariement` | §5.2 |
| **Notifications** | `Notification` + enum `TypeNotification` (8 types) | §5 ex.6, §5.1.1, §5.2 ex.3 |
| **Journal & Audit** | `EntreeJournal` + enum `TypeOperation` (30 opérations) | §5 ex.10 |

### 6.3 Points de conception critiques

| Point | Problème | Pistes |
|-------|----------|--------|
| **Demande composite** | Pattern Composite (ET/OU) — clôture récursive | Pattern Composite GoF ; clôture en cascade |
| **Stratégie de mise en avant** | Extensible par plugins | Pattern Strategy ; interface `DemandePrioritizationStrategy` |
| **Interface traduction** | Indépendance fournisseur | Pattern Strategy ou Adapter ; `TranslationService` abstrait |
| **Taxonomie extensible** | Ajout sans redéploiement | Table hiérarchique en BD ; API de gestion taxonomie |
| **Badge d'authenticité** | Calculé ou déclaré ? | Méthode de calcul à définir selon méthodes d'identification |
| **Panier** | Cohérence non supervisée | Simple agrégation d'offres ; transaction temporaire |
| **Journal structuré** | Format pour analyse automatique | JSON structuré ; événements typés |

---

## 7. Organisation & Méthode Rabot

### 7.1 Organisation pratique *(LABIL.pdf)*

| Élément | Détail |
|---------|--------|
| **Groupes** | Taille définie par le professeur |
| **Forum** | 1 forum commun à tous les groupes (Webcampus) — questions au prof |
| **Code & doc** | GitHub — dépôt nommé **`IHDCM032-2025-GROUPE-X`** *(mdl-méthode §1.3)* |
| **Rapports** | Webcampus/Devoir |
| **Deadline sprints** | Lendemain du sprint à **23:59** *(LABIL.pdf)* |
| **Surveillance** | Webcampus et mails — informations importantes diffusées là |

### 7.2 La méthode du Rabot *(mdl-méthode §5.1, Scrum.pdf)*

> Imaginez que le projet soit conduit selon la méthode Waterfall et que tous les artefacts soient produits — de l'analyse au code. Ces artefacts, disposés horizontalement selon leur causalité, constituent **la poutre à raboter**. À chaque sprint, le rabot enlève une couche.

```
ARTEFACTS (la "poutre") :

  [EA]-[User Stories]-[Use Cases]-[Robustesse]-[Composants]-[Classes]-[Code]-[Tests]
  <---------------------------- Couverture ---------------------------------------->

  Sprint 1 :  [=================================]
              <- Analyse large avant tout ->
              (use cases exhaustifs, peu de profondeur)

  Sprint 2 :           [========================================]
                       <- Verticalite = valeur livree ->
                       (portee partielle, jusqu'a l'implementation)

  Sprint n+1 : Peut revenir sur et affiner les artefacts de Sprint n (iteration)
```

**Recommandations pratiques** *(mdl-méthode §5.1)* :
1. **Sprint 1** : Inventaire exhaustif des use cases — ne pas plonger directement dans l'implémentation
2. **Regroupement** : Organiser les use cases par thématiques (propositions, matching, administration, identification...)
3. **Dépendances** : Éliciter les préconditions, priorités, criticité entre use cases
4. **Priorisation** : Maximiser valeur/effort avant de passer à la conception

### 7.3 Événements Scrum *(Scrum.pdf, LABIL.pdf)*

| Événement | Fréquence | Animé par | Points clés |
|-----------|-----------|-----------|-------------|
| **Stand-up** | 1-2× / semaine | Scrum Master | Fait ? / Fera ? / Bloquants ? / Infos utiles ? — *Si j'ai un problème, j'en parle tout de suite ou je me tais à jamais* |
| **Sprint Planning** | Début de sprint | Équipe | Objectif du sprint, Sprint Backlog, plan de réalisation |
| **Sprint Review** | Fin de sprint | **Équipe** (vous animez) | Rappel objectif, demo du livré, ne montrer que l'abouti, questions stakeholders — *top chrono* |
| **Rétrospective** | Fin de sprint, après Review | Scrum Master | Ce qui a bien/mal fonctionné, mesures pour sprint suivant, rotation SM ? |

### 7.4 Stand-up — format recommandé

```
Pour chaque membre, en 2-3 minutes :
  [+] Qu'est-ce que j'ai fait depuis le dernier stand-up ?
  [>] Qu'est-ce que je vais faire jusqu'au prochain ?
  [!] Ai-je un point bloquant ? (traite hors stand-up)
  [i] Y a-t-il une information utile a partager ?
```

### 7.5 Sprint Review — format recommandé *(LABIL.pdf)*

```
Structure d'une Sprint Review (~20-30 min) :
  1. Rappel de l'objectif du sprint (2 min)
  2. Presentation des artefacts valides (5-10 min)
     => Ne montrer QUE ce qui est abouti
  3. Demonstration du systeme (ou video) (5-10 min)
  4. Questions des stakeholders (5-10 min)
     => Prevoir ce temps !
  5. Collecte des remarques
```

---

## 8. Rôles Scrum & Definition of Done

### 8.1 Les rôles *(Scrum.pdf)*

#### Product Owner
- Maximise la **valeur du produit**
- Gère le **Product Backlog** : formuler l'Objectif de Produit, créer/ordonner/rendre transparent le backlog
- Compétences : vision produit, connaissance métier CrisisConnect, élicitation exigences, ouverture au changement
- Dans ce projet : le Product Owner représente les parties prenantes (sinistrés, bénévoles, entités, admins)

#### Scrum Master (Facilitateur)
- **N'est pas un chef de projet** — pas de hiérarchie dans l'équipe
- Accompagne l'autogestion et la pluridisciplinarité
- Élimine les obstacles (*LABIL.pdf* : "Si j'ai un problème, j'en parle tout de suite")
- Assure que les événements Scrum ont lieu et respectent les timebox
- Compétences : connaissance Scrum, communication, médiation, maîtrise du temps
- **Roulement possible** — à discuter en rétrospective

#### Developers
- Codent, documentent, analysent, conçoivent, testent, déploient
- Créent le Sprint Backlog et adaptent le plan chaque jour
- Adhèrent à la Definition of Done

### 8.2 Definition of Done (DoD) — template à adapter

La DoD est définie par l'équipe **en début de projet** et enrichie chaque sprint. Voici un template de départ :

```
DEFINITION OF DONE - CrisisConnect (v0, a adapter par l'equipe)

Fonctionnalite :
  [ ] Code implemente et commite sur la branche de sprint
  [ ] Tests unitaires ecrits et passants (couverture > x%)
  [ ] Tests d'integration ecrits et passants
  [ ] Revue de code par au moins 1 autre membre
  [ ] Documentation (Javadoc/OpenAPI) mise a jour
  [ ] Lie a un Use Case et/ou User Story documente(e)

Modelisation :
  [ ] Diagramme de classes mis a jour si la conception a change
  [ ] Traces design -> code visibles dans le rapport

Livraison :
  [ ] Image Docker construite et publiee sur GitHub
  [ ] README /doc mis a jour en anglais
  [ ] Demo realisable sur l'image Docker

Qualite :
  [ ] WCAG verifie sur les nouvelles interfaces
  [ ] Aucune donnee personnelle exposee sans consentement (RGPD)
  [ ] Journal d'audit alimente pour les nouvelles operations
```

---

## 9. Délivrables par sprint — Templates & Exemples

**Deadline : lendemain du sprint à 23:59** — sur Webcampus (rapports) et GitHub (code + Docker).

### 9.1 Rapport Solution

**Objectif** : Décrire ce qui a été analysé/conçu/implémenté dans ce sprint, avec les motivations.

**Structure type** :

```
RAPPORT SOLUTION -- Sprint N
Groupe : IHDCM032-2025-GROUPE-X
Date : [lendemain sprint]
Membres : [noms]

1. Objectif du sprint
   Rappel de l'objectif defini en Sprint Planning.

2. Exigences traitees
   Reference aux User Stories / Use Cases couverts ce sprint.
   Exemple :
     US-07 : En tant que sinistre, je veux deposer une demande composite...
     UC-03 : Gerer une demande composite (ET/OU)

3. Modeles produits
   - Diagramme EA (si sprint 1) -> DB-MAIN
   - Use Cases mis a jour
   - Diagramme de robustesse pour UC-03
   - Diagramme de classes du composant "Propositions"
   [Graphiques vectoriels obligatoires -- zoom PDF]

4. Choix architecturaux & motivations
   Exemple :
     Pattern Composite pour les demandes composites.
     Avantages : recursivite naturelle, cloture propagee simplement.
     Desavantages : complexite de l'implementation, profondeur potentiellement infinie.

5. Retours sur l'architecture & choix techniques
   Ce qui a bien fonctionne / ce qui serait a revoir.

6. Prochaines etapes (annonce)
```

> **Attention** : Utiliser la **page de garde Webcampus**. Graphismes en **format vectoriel** (SVG, PDF — pas de screenshots PNG pixelisés).

**Exemples de modèles attendus par phase** :

| Sprint | Modèles typiques |
|--------|-----------------|
| Sprint 1 | Glossaire, modèle EA (DB-MAIN), liste Use Cases, premières US |
| Sprint 2 | Diagramme de robustesse, scénarios d'interaction, maquettes IHM |
| Sprint 3+ | Diagramme de classes affiné, spec API (OpenAPI), schéma BD, code documenté |

### 9.2 Rapport Projet

**Objectif** : Décrire comment le groupe a travaillé — méthode, organisation, rétrospective.

**Structure type** :

```
RAPPORT PROJET -- Sprint N
Groupe : IHDCM032-2025-GROUPE-X
Date : [lendemain sprint]

1. Methode utilisee
   Description de comment Scrum/Rabot a ete applique ce sprint.
   Adaptations faites par rapport au sprint precedent.

2. Rapport Backlog
   2.1 Backlog en fin de Sprint N-1
       | ID    | User Story / Tache      | Statut      | Responsable | Points |
       |-------|-------------------------|-------------|-------------|--------|
       | US-01 | Deposer une offre       | Done        | Alice       | 5      |
       | US-02 | Rechercher propositions | Done        | Bob         | 3      |
       | US-07 | Demande composite       | In Progress | Carol       | 8      |

   2.2 Backlog en fin de Sprint N
       | ID    | User Story / Tache      | Statut      | Responsable | Points |
       |-------|-------------------------|-------------|-------------|--------|
       | US-07 | Demande composite       | Done        | Carol       | 8      |
       | US-08 | Cloture recursive       | Done        | Carol+Bob   | 5      |
       | US-12 | Badge authenticite      | Todo        | Alice       | 8      |

3. Comptes rendus (points saillants uniquement -- pas les PV complets)
   3.1 Stand-ups (semaines N et N+1)
       - Stand-up 12/03 : Point bloquant sur l'API de traduction -> decision d'utiliser
         LibreTranslate en fallback. Alice prend la main.
       - Stand-up 14/03 : US-07 complete cote backend, frontend en cours.

   3.2 Sprint Review
       - Demonstration : depot d'une demande composite ET (bovins + transport)
       - Remarque stakeholder : formulaire trop complexe pour mobile -> a simplifier
       - Artefacts valides : modele de robustesse UC-03

   3.3 Retrospective
       - Ce qui a bien fonctionne : code review systematique, a evite 2 bugs majeurs
       - Ce qui n'a pas bien fonctionne : estimations trop optimistes sur US-07
       - Mesure : decomposer les US en taches < 4h avant de les estimer

4. Retours sur la methode
   Points positifs / negatifs de Scrum/Rabot ce sprint.
```

### 9.3 Rapport Individuel

**Template fourni sur Webcampus** — à remplir par chaque membre.

**Contenu typique attendu** :
```
NOM, Prenom -- Sprint N

1. Contributions ce sprint
   - Taches realisees (US/taches avec references)
   - Temps estime vs temps reel

2. Ce que j'ai appris
   - Technologies, methodes, outils

3. Difficultes rencontrees
   - Techniques, organisationnelles

4. Mon regard sur la dynamique de groupe
   - Ce qui fonctionne bien / a ameliorer

5. Objectifs pour le prochain sprint
```

### 9.4 Image Docker

- **Publiée sur GitHub** : registry Docker associé au dépôt `IHDCM032-2025-GROUPE-X`
- L'image doit permettre de lancer une démo fonctionnelle du système
- Basée sur le `docker-compose.yml` fourni (à adapter selon les choix technologiques)
- **Tag de version** recommandé : `sprint-N` ou tag sémantique

**Exemple de commande de lancement attendue** :
```bash
# L'evaluateur doit pouvoir faire :
docker pull ghcr.io/ihdcm032-2025-groupe-x/crisisconnect:sprint-3
docker compose up
# Puis acceder a http://localhost:8080
```

---

## 10. Phases de développement

La méthode Rabot organise ces phases de manière **itérative et non séquentielle**. L'agenda précis est fourni par le professeur. Chaque phase peut être revisitée lors d'un sprint ultérieur.

### Phase 0 — Mise en place

**Objectif** : L'équipe est opérationnelle, les fondations sont posées.

- [ ] Lire l'énoncé CrisisConnect dans son intégralité (**tous les membres** — individuellement)
- [ ] Mettre en commun les questions/ambiguïtés → poster sur le **forum Webcampus**
- [ ] Signer et déposer la **Charte d'engagement** (délai absolu, non négociable)
- [ ] Réaliser l'**exercice Spring Boot**
- [ ] Créer le dépôt GitHub : **`IHDCM032-2025-GROUPE-X`**
- [ ] Se mettre en contact avec l'assistant pour la marche à suivre *(mdl-méthode §1.3)*
- [ ] Réunion informelle de découverte :
  - Présentations (préférences IT, hobbies, qualités/faiblesses)
  - "Mode d'emploi" de chacun (susceptible mais je me soigne, aime les échéances fermes, etc.)
  - Choisir le 1er **Scrum Master**
- [ ] Désigner le **Product Owner**
- [ ] Planifier les rencontres régulières et choisir la plateforme d'échanges (Discord, Teams, Slack...)
- [ ] Élaborer la **Definition of Done (DoD)** initiale (cf. template §8.2)
- [ ] Configurer le dépôt GitHub (branches, protection main, PR obligatoires)
- [ ] Créer la structure `/doc` sur GitHub (documentation anglaise stakeholders)

**Délivrables Phase 0** :
- Charte signée → Webcampus
- Exercice Spring Boot → Webcampus
- Dépôt GitHub initialisé

---

### Phase 1 — Analyse

*(mdl-méthode §2 — obligatoire, pénalités en cas de non-respect)*

#### 1.1 Compréhension de l'énoncé *(mdl-méthode §2.1)*

**Modèle Entité/Association** :
- Outil obligatoire : **DB-MAIN** (notation cours BD — pas UML ici)
- Interprétation **conceptuelle** du domaine, indépendante de la solution technique
- Entités à modéliser : Acteur, Personne, Entite, Proposition, Offre, Demande, Transaction, ConfigCatastrophe, CategorieTaxonomie, Mandat, EntreeJournal...

**Exemple partiel de glossaire attendu** :

| Terme | Définition dans le contexte CrisisConnect | Ambiguïtés |
|-------|------------------------------------------|-----------|
| Proposition | Offre ou Demande déposée sur la plateforme | — |
| Transaction | Engagement en cours entre parties — quand démarre-t-elle ? | À clarifier |
| Mandat | Délégation formelle — toutes opérations ou catégorie spécifique ? | À décider |
| Badge | Indicateur fiabilité identité — calculé automatiquement ou assigné manuellement ? | À décider |
| Clôture récursive | Propagation automatique vers les sous-demandes | Comment s'arrête la récursion ? |

**Ambiguïtés à documenter** (pour chacune : objet, alternatives, préférence équipe + motivations) :

| Ambiguïté | Alternatives | Recommandation |
|-----------|-------------|----------------|
| Quand une transaction démarre-t-elle ? | (a) Quand un utilisateur exprime son intérêt ; (b) Quand les deux parties s'accordent | (b) — plus robuste |
| Un acteur peut-il avoir plusieurs mandants ? | (a) Un seul ; (b) Plusieurs | À clarifier avec le prof |
| La suggestion automatique (§5.2 ex.4) est-elle obligatoire ? | Exigence ou suggestion ? | À clarifier |
| Que se passe-t-il si une offre couplée à des demandes est clôturée ? | (a) Les demandes restent actives ; (b) Clôture cascade | (a) probable — demandes indépendantes |

- [ ] Produire le **diagramme EA** (DB-MAIN)
- [ ] Rédiger le **glossaire** des termes nécessitant définition explicite
- [ ] Documenter les **ambiguïtés** avec alternatives et préférences
- [ ] Produire les **modèles d'activités ou BPMN** pour les flux métier principaux (ex. cycle de vie d'une proposition, processus de matching, processus d'identification)

**Délivrable 1.1** : Modèle EA + glossaire + liste ambiguïtés + BPMN/activités

---

#### 1.2 Analyse fonctionnelle — User Stories & Use Cases *(mdl-méthode §2.2)*

**Format User Story** :
> *"As a [persona], I want [action] so that [goal]."*

Qualités **INVEST** requises :

| Lettre | Qualité | Explication |
|--------|---------|-------------|
| **I** | Independent | Pas de dépendance forte avec d'autres US |
| **N** | Negotiable | Le comment est négociable, pas le quoi |
| **V** | Valuable | Apporte de la valeur à un acteur réel |
| **E** | Estimable | L'équipe peut estimer l'effort |
| **S** | Small | Réalisable en un sprint |
| **T** | Testable | On sait comment vérifier qu'elle est Done |

**Dimensions à évaluer pour chaque US** :

| Dimension | Échelle | Description |
|-----------|---------|-------------|
| Valeur métier | 1-10 | Importance pour les parties prenantes |
| Risque technique | Bas/Moyen/Élevé | Incertitude technologique |
| Risque métier | Bas/Moyen/Élevé | Ambiguïté de l'exigence |
| Dépendances | Lien vers autres US | Préconditions |
| Effort estimé | Story Points | Complexité relative |

**Use Case — documentation minimale** :

```
UC-XX : [Nom explicite -- verbe infinitif + complement, point de vue acteur principal]

Acteur principal   : [Role]
Acteurs secondaires: [Roles]
Objectif           : [But de l'acteur]
Pre-conditions     : [Ce qui doit etre vrai avant]
Post-conditions    : [Ce qui est vrai apres succes]
Scenario nominal   :
  1. ...
  2. ...
Scenarios alternatifs / exceptions :
  3a. ...
```

- [ ] Produire la liste de **User Stories** avec dimensions (cf. §11 pour exemples)
- [ ] Produire le **modèle de Use Cases** initial
- [ ] Documenter les **dépendances entre use cases** (graphe de précédence)

**Délivrable 1.2** : Liste US avec dimensions ; modèle Use Cases

---

#### 1.3 Exigences non-fonctionnelles *(mdl-méthode §2.3)*

- [ ] Éliciter les exigences non-fonctionnelles par **scénarios** (méthode IALTEM)
- [ ] Sélectionner les scénarios les plus **critiques** à préciser (pas d'exhaustivité absolue)
- [ ] Pour CrisisConnect, prioriser : bande passante, WCAG, RGPD, extensibilité taxonomie

**Délivrable 1.3** : Scénarios NFR documentés

---

#### 1.4 Modèle de robustesse *(mdl-méthode §2.4, §6)*

Le **modèle de robustesse** (Ivar Jacobson, 1991) affine les use cases en identifiant les responsabilités :

| Stéréotype | Symbole | Rôle dans CrisisConnect |
|------------|---------|------------------------|
| **Acteur** | bonhomme | Utilisateur, AC, AS, AI, Entité |
| **Interface** | rectangle arrondi | Formulaire dépôt offre, carte GIS, page profil, interface admin |
| **Contrôleur** | cercle avec flèche | PropositionController, MatchingController, AuthController, TaxonomieController |
| **Repository** | cylindre | PropositionRepository, ActeurRepository, JournalRepository |

**Relations autorisées** (règle stricte de Jacobson) :
```
  Acteur --> Interface      [OK]
  Interface --> Controleur  [OK]
  Controleur --> Controleur [OK]  (deviendront des call & return entre composants)
  Controleur --> Repository [OK]
  Controleur --> Interface  [OK]  (systeme prend l'initiative - notification)

  Acteur --> Controleur     [INTERDIT]
  Acteur --> Repository     [INTERDIT]
  Interface --> Repository  [INTERDIT]
```

**Outil recommandé** : Visual Paradigm (stéréotypes Entity/Control/Boundary sur diagramme de classes)

- [ ] Affiner chaque use case en **diagramme de robustesse**
- [ ] Ajouter les **scénarios d'interaction** (diagrammes de séquence ou langage naturel structuré)
- [ ] Compléter si besoin : BPMN, statecharts, maquettes IHM

**Délivrable 1.4** : Modèle de robustesse + scénarios + maquettes IHM si nécessaire

---

### Phase 2 — Conception

*(mdl-méthode §3)*

#### 2.1 Vision architecturale *(mdl-méthode §3.1)*

- [ ] Produire un **modèle de composants**
  - Chaque élément du diagramme de robustesse → attribué à un composant
  - Interfaces entre composants définies (API internes)
- [ ] Si plusieurs visions envisagées : présenter chacune + **motiver le choix** (avantages/désavantages)
- [ ] Définir les **tactiques architecturales** pour les NFR critiques (ex. cache pour bande passante, abstraction pour traduction)

**Délivrable 2.1** : Modèle de composants avec motivations des choix

---

#### 2.2 Conception détaillée *(mdl-méthode §3.2)*

| Type de composant | Artefact de conception attendu |
|-------------------|-------------------------------|
| **Métier** | Diagramme de classes (+ états / séquences pour aspects critiques) |
| **Repository** | Modèle EA conceptuel (DB-MAIN) **ou** diagramme de classes si ORM |
| **Frontière système<->système** | Spécification API (OpenAPI/Swagger) |
| **Frontière système<->humain** | Sketching d'interface graphique (maquettes) |

> Note : le `class-diagram.puml` est une première ébauche — l'affiner composant par composant lors de cette phase.

- [ ] Affiner le diagramme de classes par composant
- [ ] Définir le schéma de base de données (modèle EA conceptuel en DB-MAIN)
- [ ] Spécifier les API REST (OpenAPI/Swagger)
- [ ] Produire les maquettes IHM pour les cas d'usage principaux

**Délivrable 2.2** : Diagrammes de classes affinés, schéma BD, spec API, maquettes IHM

---

### Phase 3 — Implémentation

*(mdl-méthode §4)*

#### 3.1 Motivation des choix technologiques *(mdl-méthode §4.1)*

- [ ] Choisir et **justifier** chaque technologie (avantages/désavantages par rapport au contexte CrisisConnect)
- [ ] Documenter dans le Rapport Solution avec tableau comparatif (cf. §12)

> *« Les armes sont au choix, à condition de rester conforme aux attentes des stakeholders et d'être capables de les justifier. »* *(LABIL.pdf)*

#### 3.2 Mise en œuvre *(mdl-méthode §4.2)*

- [ ] Implémenter les composants conformément aux choix technologiques
- [ ] Respecter les liens de **traçabilité** entre modèles et code (mentionner dans les rapports)
- [ ] Publier une **image Docker** sur GitHub (délivrable obligatoire à chaque sprint)

#### 3.3 Tests *(mdl-méthode §4.3)*

- [ ] Tests selon les critères de la **Definition of Done** (DoD)
- [ ] Suite de tests documentés (obligation de l'énoncé)
- [ ] Tests unitaires, d'intégration, d'acceptabilité (scénarios NFR)

---

## 11. User Stories exemples (CrisisConnect)

Ces User Stories sont des **exemples de départ** à partir de l'énoncé — à enrichir, affiner et prioriser par l'équipe.

### 11.1 Module Propositions

| ID | User Story | Acteur | Valeur | Effort |
|----|------------|--------|--------|--------|
| **US-01** | En tant qu'utilisateur, je veux déposer une offre avec photos et géolocalisation, afin que les sinistrés puissent trouver facilement mon aide. | Utilisateur | 9 | M |
| **US-02** | En tant qu'utilisateur, je veux déposer une demande simple, afin d'exprimer mon besoin d'aide. | Utilisateur | 9 | S |
| **US-03** | En tant qu'utilisateur, je veux créer une demande composite ET (ex. étable + transport), afin de décrire un besoin multi-composantes. | Utilisateur | 7 | L |
| **US-04** | En tant qu'utilisateur, je veux éditer ma proposition si elle n'est pas en transaction, afin de la maintenir à jour. | Utilisateur | 6 | S |
| **US-05** | En tant qu'utilisateur, je veux clôturer une proposition, afin de signaler qu'elle n'est plus disponible. | Utilisateur | 7 | S |
| **US-06** | En tant qu'AC, je veux configurer le délai d'archivage (n jours), afin de maintenir la plateforme propre selon le contexte. | AC | 6 | S |
| **US-07** | En tant que plateforme, je dois rappeler aux propriétaires de reconfirmer leurs propositions avant archivage, afin d'éviter les informations obsolètes. | Système | 7 | M |

### 11.2 Module Matching

| ID | User Story | Acteur | Valeur | Effort |
|----|------------|--------|--------|--------|
| **US-10** | En tant qu'utilisateur, je veux initier une discussion publique sur une proposition, afin de poser des questions ouvertes. | Utilisateur | 8 | M |
| **US-11** | En tant qu'utilisateur, je veux basculer ma discussion en mode privé, afin de partager des coordonnées confidentielles. | Utilisateur | 8 | S |
| **US-12** | En tant qu'utilisateur, je veux sélectionner plusieurs offres dans un panier, afin de constituer un ensemble cohérent d'aides. | Utilisateur | 7 | M |
| **US-13** | En tant que plateforme, je dois notifier les propriétaires d'offres quand un panier les contenant est annulé, afin qu'ils sachent que leurs offres sont à nouveau disponibles. | Système | 8 | M |
| **US-14** | En tant qu'utilisateur, je veux voir des suggestions d'appariement (offres <-> demandes géolocalisées), afin de trouver plus rapidement une correspondance. | Utilisateur | 9 | XL |

### 11.3 Module Identification & Profils

| ID | User Story | Acteur | Valeur | Effort |
|----|------------|--------|--------|--------|
| **US-20** | En tant que sinistré sans document, je veux créer un compte par délégation d'un tiers identifié, afin d'accéder à la plateforme même après avoir tout perdu. | Utilisateur | 10 | L |
| **US-21** | En tant qu'utilisateur, je veux m'identifier par SMS, afin de vérifier mon identité sans carte d'identité physique. | Utilisateur | 8 | M |
| **US-22** | En tant qu'utilisateur, je veux voir le badge d'authenticité d'un autre utilisateur, afin d'évaluer la fiabilité de son identité. | Utilisateur | 9 | S |
| **US-23** | En tant qu'utilisateur, je veux donner un mandat à un tiers pour agir en mon nom, afin de déléguer la gestion de mes propositions si je suis indisponible. | Utilisateur | 6 | L |

### 11.4 Module Administration

| ID | User Story | Acteur | Valeur | Effort |
|----|------------|--------|--------|--------|
| **US-30** | En tant qu'AC, je veux configurer la taxonomie (ajouter/modifier des catégories), afin d'adapter la plateforme à la catastrophe en cours sans redéploiement. | AC | 9 | M |
| **US-31** | En tant qu'AC, je veux ajouter ou supprimer une entité reconnue, afin de gérer la liste des organisations fiables. | AC | 8 | S |
| **US-32** | En tant qu'AS, je veux consulter le journal d'audit structuré, afin de détecter des comportements suspects. | AS | 7 | M |
| **US-33** | En tant qu'AC, je veux définir les modes d'identification acceptés pour cette catastrophe, afin d'adapter la sécurité au contexte. | AC | 8 | M |

### 11.5 Module Traduction

| ID | User Story | Acteur | Valeur | Effort |
|----|------------|--------|--------|--------|
| **US-40** | En tant qu'utilisateur, je veux choisir ma langue d'interface parmi celles disponibles, afin de naviguer dans ma langue maternelle. | Utilisateur | 9 | M |
| **US-41** | En tant qu'utilisateur, je veux voir si un texte est traduit automatiquement et accéder à l'original, afin d'évaluer la fiabilité de la traduction. | Utilisateur | 7 | S |
| **US-42** | En tant qu'AS, je veux changer de fournisseur de traduction sans modifier le code, afin d'assurer la disponibilité du service. | AS | 8 | L |

---

## 12. Décisions technologiques à prendre

L'énoncé est **technologiquement neutre**. Chaque choix doit être **justifié dans le Rapport Solution** avec avantages/désavantages dans le contexte de CrisisConnect.

### D1 — Backend

| Option | Avantages dans ce contexte | Inconvénients |
|--------|--------------------------|---------------|
| **Java + Spring Boot** | Connu via TD, riche écosystème, JPA/Hibernate | Verbeux, démarrage lent |
| **Node.js (Express/NestJS)** | Léger, async natif, bon pour API REST | Moins structuré, typage optionnel |
| **Python (Django/FastAPI)** | Rapide à prototyper, ML si suggestion matching | Performance moindre sous charge |
| **Go** | Très performant, binaire natif | Courbe apprentissage, moins de frameworks |

**Critères clés** : compétences de l'équipe, richesse de l'écosystème pour les besoins spécifiques (GIS, traduction, auth multi-modale).

### D2 — Base de données

| Option | Avantages | Inconvénients | Adéquation CrisisConnect |
|--------|-----------|---------------|--------------------------|
| **PostgreSQL** | Relationnel robuste, JSONB, PostGIS (géoloc) | Schéma rigide | Très bonne — PostGIS pour géolocalisation |
| **MySQL/MariaDB** | Répandu, simple | Moins de types avancés | Bonne |
| **MongoDB** | Schéma flexible (taxonomie extensible) | Moins de transactions ACID | Partielle — taxonomie flexible mais relations complexes |
| **Hybride PG + Redis** | Cache + BD relationnelle | Complexité infrastructure | Bonne si performance requise |

**Recommandation** : PostgreSQL (+ PostGIS pour les fonctionnalités de géolocalisation).

### D3 — Frontend

| Option | Avantages | Inconvénients | Adéquation |
|--------|-----------|---------------|-----------|
| **React** | Très répandu, riche écosystème i18n | Courbe apprentissage | Bonne |
| **Vue.js** | Plus accessible, progressive | Écosystème plus petit | Bonne |
| **Angular** | Framework complet, TypeScript natif | Lourd, verbeux | Bonne mais overhead |
| **PWA (progressive)** | Mobile + offline + responsive en un | Plus complexe à tester | Excellente pour CrisisConnect (bande passante) |
| **Svelte** | Léger (important pour bande passante) | Moins répandu | Très bonne |

**Critères clés** : WCAG, multilingue (i18n), bande passante minimisée, responsive.

### D4 — Modes d'identification à implémenter (prioritisation)

| Mode | Priorité suggérée | Justification |
|------|------------------|---------------|
| Login/Password | **P0 — obligatoire** | Minimum exigé par l'énoncé |
| SMS/OTP | **P1 — élevée** | Disponible même en crise, couvre la majorité |
| eID (carte d'identité électronique) | **P2 — haute** | Fiable mais infrastructure requise |
| Parrainage (n utilisateurs) | **P2 — haute** | Cas sinistré sans document |
| Opération bancaire | **P3 — moyenne** | Dépend infrastructure bancaire |
| Délégation par tiers | **P3 — moyenne** | Dernier recours, fiabilité faible |
| Photo carte identité | **P4 — basse** | Modération manuelle requise |

### D5 — Service de traduction

| Option | Avantages | Inconvénients | Licence |
|--------|-----------|---------------|---------|
| **LibreTranslate** | Open source, hébergeable localement | Qualité moindre | AGPL-3.0 |
| **DeepL API** | Qualité excellente | Propriétaire, payant au-delà du quota | Commerciale |
| **Google Translate API** | Large couverture linguistique | Propriétaire, payant | Commerciale |
| **Argos Translate** | Open source, offline possible | Qualité variable | MIT |

**Contrainte** : Interface d'abstraction **obligatoire** (énoncé §5 ex.16). Implémenter avec un pattern Strategy ou Adapter.

### D6 — Infrastructure & Déploiement

| Option | Avantages | Adéquation |
|--------|-----------|-----------|
| **Docker Compose** | Simple, cohérent avec TDs, image Docker délivrable | Excellente |
| **Kubernetes (k8s)** | Scalabilité, résilience | Overkill pour ce projet académique |
| **Docker Swarm** | Compromis entre les deux | Possible mais complexité non justifiée |

**Recommandation** : Docker Compose (cohérent avec l'image Docker délivrable et les TDs).

### D7 — Outil de modélisation

| Outil | Avantages | Inconvénients | Recommandé pour |
|-------|-----------|---------------|-----------------|
| **Visual Paradigm** | Supporte robustesse (E/C/B), tous diagrammes | Commercial (étudiant gratuit limité) | Modèle de robustesse |
| **PlantUML** | Textuel, versionnable dans Git | Pas de modèle robustesse natif | Diagrammes de classes, séquence |
| **Draw.io** | Gratuit, intuitif, export vectoriel | Manuel, pas de validation | Maquettes, composants |
| **DB-MAIN** | Obligatoire pour le modèle EA | Uniquement EA | Modèle EA conceptuel |
| **StarUML** | Complet, prix raisonnable | Payant | Alternative à Visual Paradigm |

**Recommandation** : Visual Paradigm pour le modèle de robustesse + PlantUML pour les diagrammes de classes (versionnable) + DB-MAIN pour le modèle EA.

---

## 13. Évaluation *(LABIL.pdf)*

### 13.1 Critères

- Basée sur **présentations orales**, **rapports**, et **démonstrations** du système
- Cotation **a priori pour le groupe**, ajustée **individuellement** selon l'implication
- L'UE **doit être réussie en 1ère session**
  - 2e session réservée aux circonstances exceptionnelles + implication positive démontrée

### 13.2 Ce qui est évalué (déduction des slides et méthode)

| Dimension | Indicateurs |
|-----------|-------------|
| **Qualité de l'analyse** | Modèle EA, glossaire, use cases, robustesse — fidélité à l'énoncé |
| **Qualité de la conception** | Diagrammes de classes, architecture, API, motivations des choix |
| **Implémentation** | Fonctionnalités livrées, qualité du code, tests, image Docker |
| **Méthode** | Rigueur Scrum/Rabot, backlog, stand-ups, rétro documentés |
| **Communication** | Clarté des présentations orales, organisation de la Review |
| **Documentation** | Rapports structurés, graphismes vectoriels, doc anglaise sur GitHub |
| **Collaboration** | Implication individuelle, gestion de groupe (rapport individuel) |

### 13.3 Pièges à éviter

- Implémenter sans avoir analysé (sauter Phase 1)
- Graphismes pixelisés (screenshots au lieu de vectoriel)
- PV complets dans les rapports (points saillants uniquement)
- Techno sans justification dans le rapport
- Image Docker manquante ou non fonctionnelle
- Documentation uniquement en français (/doc doit être en anglais)

---

## 14. Points de vigilance

### Vigilance absolue (pénalités explicites)

| Point | Source | Détail |
|-------|--------|--------|
| **Charte d'engagement** | LABIL.pdf | Délai absolu, non négociable — signer dès que possible |
| **Exercice Spring Boot** | LABIL.pdf | Validation technique initiale avant implémentation |
| **Modèle EA obligatoire** | mdl-méthode §2.1 | DB-MAIN, notation cours BD — pénalités si absent |
| **User Stories + Use Cases** | mdl-méthode §2.2 | Obligatoires — pénalités si absents |

### Vigilance technique (exigences énoncé)

| Point | Source | Détail |
|-------|--------|--------|
| **Taxonomie extensible** | énoncé §5 ex.3 | L'AC doit pouvoir ajouter des catégories **sans redéploiement** |
| **Clôture récursive** | énoncé §5.1 ex.3 | Demande composite ET/OU → propagation automatique |
| **Indépendance traduction** | énoncé §5 ex.16 | Interface d'abstraction obligatoire — pas de hard-coding fournisseur |
| **Multilinguisme** | énoncé §2, §4 | Prévoir i18n **dès l'architecture** (pas en fin de projet) |
| **RGPD** | énoncé §4 | Privacy by Design — données sinistrés sensibles |
| **WCAG** | énoncé §2 | Non optionnel — valider avec les outils WCAG |
| **Bande passante** | énoncé §2 | Minimisée — images compressées, lazy loading, API efficace |
| **Journal structuré** | énoncé §5 ex.10 | Format pour analyse automatique — pas un simple log texte |

### Vigilance organisationnelle (méthode)

| Point | Source | Détail |
|-------|--------|--------|
| **Modèle de robustesse** | mdl-méthode §2.4, §6 | Outil de transition analyse → conception — ne pas sauter |
| **Justification des choix** | mdl-méthode §4.1 | Avantages/désavantages **pour chaque technologie** dans le rapport |
| **Image Docker** | LABIL.pdf | Délivrable obligatoire à **chaque sprint** — pas seulement à la fin |
| **Page de garde rapports** | LABIL.pdf | Utiliser le template Webcampus |
| **Graphismes vectoriels** | LABIL.pdf | SVG/PDF — pas de screenshots PNG pixelisés en zoom |
| **Documentation anglais** | énoncé §2 | Dès le premier sprint, dans `/doc` sur GitHub |
| **Gestion de groupe** | LABIL.pdf | Principal défi selon le prof — méta-règles assertivité essentielles |

### Rappel méta-règles *(LABIL.pdf)*

> - Soyez assertifs : clair, posé, constructif — "Je pense que..." plutôt que "Tu as tort"
> - Ne laissez personne sur le côté de la route — avancer ensemble, même plus lentement
> - En cas de difficulté relationnelle : "Comment pouvons-nous surmonter cette difficulté ?" — "Que puis-JE faire ?"
> - Si j'ai un problème, j'en parle **tout de suite** ou je me tais à jamais

---

## 15. Ressources disponibles

### 15.1 Documents du cours

| Fichier | Contenu | Priorité |
|---------|---------|----------|
| `2025-2026 - énoncé IHDCM032 - CrisisConnect.pdf` | **Cahier des charges — source de vérité absolue** | Indispensable |
| `mdl-méthode.pdf` | **Méthodologie complète** : EA, US, UC, robustesse, Rabot, conception, implémentation | Indispensable |
| `Scrum.pdf` | **Présentation Scrum** : rôles, événements, backlog, méthode Rabot (slides) | Indispensable |
| `LABIL.pdf` | Organisation du cours, délivrables, méta-règles, évaluation | Indispensable |
| `docker-compose.yml` | Exemple Docker Compose fourni (à adapter) | Utile |
| `TD_SpringBoot.pdf` | Introduction Spring Boot + Java 21 + Maven | Utile (si Spring) |
| `Fiche GIT.pdf` | Référence Git complète | Référence |
| `Fiche Docker.pdf` | Référence Docker complète | Référence |
| `IHDCM032 - TP sur Git et GitHub et Docker.pdf` | TP pratique Git / Docker | Référence |

### 15.2 GitHub — Structure recommandée

```
IHDCM032-2025-GROUPE-X/
    README.md                  # Presentation du projet (EN)
    docker-compose.yml         # Lancement du systeme
    doc/                       # Documentation stakeholders (EN)
        architecture.md
        api-spec.yaml          # OpenAPI/Swagger
        deployment.md
        user-guide.md
    src/                       # Code source
        backend/
        frontend/
    tests/                     # Suite de tests documentes
    deliverables/              # Rapports (ou via Webcampus)
        sprint-1/
            rapport-solution.pdf
            rapport-projet.pdf
            rapport-individuel-[nom].pdf
        sprint-2/
            ...
```

---

## Références

[1] Chou, C.-H., Zahedi, F. M., and Zhao, H. — *Ontology for developing web sites for natural disaster management: Methodology and implementation.* IEEE Transactions on Systems, Man, and Cybernetics - Part A: Systems and Humans 41, 1 (2011), 50-62.

[2] Beck, K. et al. — *Manifesto for Agile Software Development.* agilemanifesto.org (2001).

[3] Jacobson, I. — *Object-Oriented Software Engineering: A Use Case Driven Approach.* Addison-Wesley (1991).

[4] W3C — *Web Content Accessibility Guidelines (WCAG).* https://www.w3.org/WAI/standards-guidelines/wcag/

---

*Document généré à partir des sources officielles du cours IHDCM032 — 2025-2026.*
*Dernière mise à jour : 2026-02-21.*
