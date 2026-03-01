using System.Net.Http.Json;
using CrisisConnect.Web.Models;

namespace CrisisConnect.Web.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    // ── Auth ──────────────────────────────────────────────────────────────────

    public async Task<AuthResponseModel?> LoginAsync(string email, string motDePasse, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email, MotDePasse = motDePasse }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AuthResponseModel>(ct);
    }

    public async Task<AuthResponseModel?> RegisterAsync(
        string email, string motDePasse, string role, string prenom, string nom,
        string? telephone = null, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register",
            new { Email = email, MotDePasse = motDePasse, Role = role, Prenom = prenom, Nom = nom, Telephone = telephone }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AuthResponseModel>(ct);
    }

    // ── Offres ────────────────────────────────────────────────────────────────

    public Task<IReadOnlyList<OffreModel>?> GetOffresAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<OffreModel>>("api/propositions/offres", ct);

    public Task<OffreModel?> GetOffreByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<OffreModel>($"api/propositions/offres/{id}", ct);

    // ── Demandes ──────────────────────────────────────────────────────────────

    public Task<IReadOnlyList<DemandeModel>?> GetDemandesAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<DemandeModel>>("api/propositions/demandes", ct);

    public Task<DemandeModel?> GetDemandeByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<DemandeModel>($"api/propositions/demandes/{id}", ct);

    // ── Propositions ──────────────────────────────────────────────────────────

    public Task<IReadOnlyList<PropositionModel>?> GetPropositionsAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<PropositionModel>>("api/propositions", ct);

    // ── Propositions lifecycle ────────────────────────────────────────────────

    public async Task<bool> ArchiverPropositionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/propositions/{id}/archiver", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ClorePropositionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/propositions/{id}/clore", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RelancerPropositionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/propositions/{id}/relance", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ReconfirmerPropositionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/propositions/{id}/reconfirmer", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Offres (création) ─────────────────────────────────────────────────────

    public async Task<OffreModel?> CreateOffreAsync(
        string titre, string description, Guid creePar, bool livraisonIncluse = false,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/propositions/offres",
            new { Titre = titre, Description = description, CreePar = creePar, LivraisonIncluse = livraisonIncluse }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<OffreModel>(ct);
    }

    // ── Transactions ──────────────────────────────────────────────────────────

    public Task<IReadOnlyList<TransactionModel>?> GetTransactionsAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<TransactionModel>>("api/transactions", ct);

    public Task<TransactionModel?> GetTransactionByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<TransactionModel>($"api/transactions/{id}", ct);

    public async Task<TransactionModel?> InitierTransactionAsync(Guid propositionId, Guid initiateurId, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/transactions",
            new { PropositionId = propositionId, InitiateurId = initiateurId }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<TransactionModel>(ct);
    }

    public async Task<bool> ConfirmerTransactionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/transactions/{id}/confirmer", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AnnulerTransactionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/transactions/{id}/annuler", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<DemandeModel?> CreateDemandeAsync(
        string titre, string description, Guid creePar, string urgence, string? regionSeverite = null,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/propositions/demandes",
            new { Titre = titre, Description = description, CreePar = creePar, Urgence = urgence, RegionSeverite = regionSeverite }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DemandeModel>(ct);
    }

    // ── Paniers ───────────────────────────────────────────────────────────────

    public Task<PanierModel?> GetPanierAsync(Guid proprietaireId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<PanierModel>($"api/paniers?proprietaireId={proprietaireId}", ct);

    public async Task<PanierModel?> CreatePanierAsync(Guid proprietaireId, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/paniers", new { ProprietaireId = proprietaireId }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<PanierModel>(ct);
    }

    public async Task<PanierModel?> AjouterOffreAuPanierAsync(Guid panierId, Guid offreId, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync($"api/paniers/{panierId}/offres", new { OffreId = offreId }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<PanierModel>(ct);
    }

    public async Task<bool> ConfirmerPanierAsync(Guid panierId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/paniers/{panierId}/confirmer", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AnnulerPanierAsync(Guid panierId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/paniers/{panierId}/annuler", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Notifications ─────────────────────────────────────────────────────────

    public Task<IReadOnlyList<NotificationModel>?> GetNotificationsAsync(Guid destinataireId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<NotificationModel>>($"api/notifications/{destinataireId}", ct);

    public async Task<bool> MarkNotificationAsReadAsync(Guid notificationId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/notifications/{notificationId}/read", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Config Catastrophe ────────────────────────────────────────────────────

    public Task<ConfigCatastropheModel?> GetConfigCatastropheAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<ConfigCatastropheModel>("api/config-catastrophe", ct);

    public async Task<ConfigCatastropheModel?> CreateConfigCatastropheAsync(
        string nom, string description, string zoneGeographique, string etatReferent,
        int delaiArchivageJours = 30, int delaiRappelAvantArchivage = 7,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/config-catastrophe", new
        {
            Nom = nom,
            Description = description,
            ZoneGeographique = zoneGeographique,
            EtatReferent = etatReferent,
            DelaiArchivageJours = delaiArchivageJours,
            DelaiRappelAvantArchivage = delaiRappelAvantArchivage
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ConfigCatastropheModel>(ct);
    }

    public async Task<ConfigCatastropheModel?> UpdateConfigCatastropheAsync(
        Guid id, int delaiArchivageJours, int delaiRappelAvantArchivage, bool estActive,
        CancellationToken ct = default)
    {
        var response = await _http.PatchAsJsonAsync($"api/config-catastrophe/{id}", new
        {
            DelaiArchivageJours = delaiArchivageJours,
            DelaiRappelAvantArchivage = delaiRappelAvantArchivage,
            EstActive = estActive
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ConfigCatastropheModel>(ct);
    }

    // ── Journal d'audit ───────────────────────────────────────────────────────

    public Task<IReadOnlyList<EntreeJournalModel>?> GetEntreesJournalAsync(Guid acteurId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<EntreeJournalModel>>($"api/journal/{acteurId}", ct);

    // ── Suggestions d'appariement ─────────────────────────────────────────────

    public Task<IReadOnlyList<SuggestionAppariementModel>?> GetSuggestionsByDemandeAsync(Guid demandeId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<SuggestionAppariementModel>>($"api/suggestions/demande/{demandeId}", ct);

    public Task<IReadOnlyList<SuggestionAppariementModel>?> GetSuggestionsPendingAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<SuggestionAppariementModel>>("api/suggestions/pending", ct);

    public async Task<bool> AcknowledgeSuggestionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/suggestions/{id}/acknowledge", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<IReadOnlyList<SuggestionAppariementModel>?> GenererSuggestionsAsync(
        Guid demandeId, CancellationToken ct = default)
    {
        var response = await _http.PostAsync($"api/suggestions/demande/{demandeId}/generer", null, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<SuggestionAppariementModel>>(ct);
    }

    // ── Rôles ─────────────────────────────────────────────────────────────────

    public Task<IReadOnlyList<AttributionRoleModel>?> GetRolesActeurAsync(Guid acteurId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<AttributionRoleModel>>($"api/roles/acteur/{acteurId}", ct);

    public async Task<AttributionRoleModel?> AttribuerRoleAsync(
        Guid acteurId, string typeRole, DateTime dateDebut, DateTime? dateFin = null,
        bool reconductible = false, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/roles", new
        {
            ActeurId = acteurId,
            TypeRole = typeRole,
            DateDebut = dateDebut,
            DateFin = dateFin,
            Reconductible = reconductible
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AttributionRoleModel>(ct);
    }

    public async Task<bool> RevoquerRoleAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/roles/{id}/revoquer", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Mandats ───────────────────────────────────────────────────────────────

    public Task<IReadOnlyList<MandatModel>?> GetMandatsAsync(Guid mandantId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<MandatModel>>($"api/mandats/mandant/{mandantId}", ct);

    public async Task<MandatModel?> CreerMandatAsync(CreerMandatRequest req, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/mandats", new
        {
            req.MandantId,
            req.MandataireId,
            req.Portee,
            req.Description,
            req.EstPublic,
            req.DateDebut,
            req.DateFin
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MandatModel>(cancellationToken: ct);
    }

    public async Task<bool> RevoquerMandatAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/mandats/{id}/revoquer", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Taxonomie ─────────────────────────────────────────────────────────────

    public Task<IReadOnlyList<CategorieTaxonomieModel>?> GetCategoriesAsync(Guid configId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<CategorieTaxonomieModel>>($"api/taxonomie/config/{configId}", ct);

    public async Task<CategorieTaxonomieModel?> CreateCategorieAsync(
        string code, string nomJson, Guid configId, Guid? parentId = null,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/taxonomie", new
        {
            Code = code,
            NomJson = nomJson,
            ConfigId = configId,
            ParentId = parentId,
            DescriptionJson = "{}"
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<CategorieTaxonomieModel>(ct);
    }

    public async Task<bool> DesactiverCategorieAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/taxonomie/{id}/desactiver", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Entités ───────────────────────────────────────────────────────────────

    public Task<IReadOnlyList<EntiteModel>?> GetEntitesAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<EntiteModel>>("api/entites", ct);

    public async Task<EntiteModel?> CreateEntiteAsync(
        string email, string motDePasse, string nom, string description,
        string moyensContact, Guid responsableId, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/entites", new
        {
            Email = email,
            MotDePasse = motDePasse,
            Nom = nom,
            Description = description,
            MoyensContact = moyensContact,
            ResponsableId = responsableId
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<EntiteModel>(ct);
    }

    public async Task<bool> DesactiverEntiteAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/entites/{id}/desactiver", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Méthodes d'identification ─────────────────────────────────────────────

    public Task<IReadOnlyList<MethodeIdentificationModel>?> GetMethodesAsync(Guid personneId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<MethodeIdentificationModel>>($"api/methodes-identification/personne/{personneId}", ct);

    public async Task<bool> VerifierMethodeAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/methodes-identification/{id}/verifier", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Acteurs ───────────────────────────────────────────────────────────────

    public Task<PersonneModel?> GetActeurAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<PersonneModel>($"api/acteurs/{id}", ct);

    public async Task<PersonneModel?> UpdateActeurAsync(
        Guid id, UpdateActeurRequest req, CancellationToken ct = default)
    {
        var response = await _http.PatchAsJsonAsync($"api/acteurs/{id}", new
        {
            Id = id, Prenom = req.Prenom, Nom = req.Nom,
            Telephone = req.Telephone, UrlPhoto = req.UrlPhoto, LanguePreferee = req.LanguePreferee,
            MoyensContact = req.MoyensContact, AdresseRue = req.Rue, AdresseVille = req.Ville,
            AdresseCodePostal = req.CodePostal, AdressePays = req.Pays
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<PersonneModel>(ct);
    }

    // ── Entités (détail) ──────────────────────────────────────────────────────

    public Task<EntiteModel?> GetEntiteByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<EntiteModel>($"api/entites/{id}", ct);

    // ── Update propositions ───────────────────────────────────────────────────

    public async Task<OffreModel?> UpdateOffreAsync(
        Guid id, string titre, string description, bool livraisonIncluse = false,
        CancellationToken ct = default)
    {
        var response = await _http.PatchAsJsonAsync($"api/propositions/offres/{id}",
            new { Id = id, Titre = titre, Description = description, LivraisonIncluse = livraisonIncluse }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<OffreModel>(ct);
    }

    public async Task<DemandeModel?> UpdateDemandeAsync(
        Guid id, string titre, string description, string urgence = "Moyen",
        string? regionSeverite = null, CancellationToken ct = default)
    {
        var response = await _http.PatchAsJsonAsync($"api/propositions/demandes/{id}",
            new { Id = id, Titre = titre, Description = description, Urgence = urgence, RegionSeverite = regionSeverite }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DemandeModel>(ct);
    }

    public async Task<bool> RecyclerPropositionAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync($"api/propositions/{id}/recycler", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── Bascule visibilité discussion ─────────────────────────────────────────

    public async Task<bool> BasculerVisibiliteDiscussionAsync(
        Guid transactionId, string visibilite, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync(
            $"api/transactions/{transactionId}/discussion/visibilite?visibilite={visibilite}", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── DemandeQuota ──────────────────────────────────────────────────────────

    public Task<IReadOnlyList<DemandeQuotaModel>?> GetDemandesQuotaAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<DemandeQuotaModel>>("api/demandes-quota", ct);

    public Task<DemandeQuotaModel?> GetDemandeQuotaByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<DemandeQuotaModel>($"api/demandes-quota/{id}", ct);

    public async Task<DemandeQuotaModel?> CreateDemandeQuotaAsync(
        Guid creePar, CreateDemandeQuotaRequest req, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/demandes-quota", new
        {
            Titre = req.Titre, Description = req.Description, CreePar = creePar,
            CapaciteMax = req.CapaciteMax, UniteCapacite = req.UniteCapacite,
            AdresseDepot = req.AdresseDepot, DateLimit = req.DateLimit
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DemandeQuotaModel>(ct);
    }

    public async Task<IntentionDonModel?> SoumettreIntentionDonAsync(
        Guid demandeQuotaId, Guid acteurId, int quantite, string unite, string description,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync($"api/demandes-quota/{demandeQuotaId}/intentions", new
        {
            DemandeQuotaId = demandeQuotaId, ActeurId = acteurId,
            Quantite = quantite, Unite = unite, Description = description
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<IntentionDonModel>(ct);
    }

    public async Task<bool> AccepterIntentionDonAsync(
        Guid demandeQuotaId, Guid intentionId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync(
            $"api/demandes-quota/{demandeQuotaId}/intentions/{intentionId}/accepter", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RefuserIntentionDonAsync(
        Guid demandeQuotaId, Guid intentionId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync(
            $"api/demandes-quota/{demandeQuotaId}/intentions/{intentionId}/refuser", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ConfirmerIntentionDonAsync(
        Guid demandeQuotaId, Guid intentionId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsync(
            $"api/demandes-quota/{demandeQuotaId}/intentions/{intentionId}/confirmer", null, ct);
        return response.IsSuccessStatusCode;
    }

    // ── PropositionAvecValidation ─────────────────────────────────────────────

    public async Task<PropositionAvecValidationModel?> CreatePropositionAvecValidationAsync(
        string titre, string description, Guid creePar, string descriptionValidation,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/propositions/avec-validation", new
        {
            Titre = titre, Description = description, CreePar = creePar,
            DescriptionValidation = descriptionValidation
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<PropositionAvecValidationModel>(ct);
    }

    public async Task<bool> ValiderPropositionAsync(
        Guid id, Guid valideurEntiteId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsJsonAsync($"api/propositions/{id}/valider",
            new { Id = id, ValideurEntiteId = valideurEntiteId }, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RefuserValidationPropositionAsync(
        Guid id, Guid valideurEntiteId, CancellationToken ct = default)
    {
        var response = await _http.PatchAsJsonAsync($"api/propositions/{id}/refuser-validation",
            new { Id = id, ValideurEntiteId = valideurEntiteId }, ct);
        return response.IsSuccessStatusCode;
    }

    // ── DemandeSurCatalogue ───────────────────────────────────────────────────

    public Task<IReadOnlyList<DemandeSurCatalogueModel>?> GetDemandesSurCatalogueAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<DemandeSurCatalogueModel>>("api/demandes-sur-catalogue", ct);

    public Task<DemandeSurCatalogueModel?> GetDemandeSurCatalogueByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<DemandeSurCatalogueModel>($"api/demandes-sur-catalogue/{id}", ct);

    public async Task<DemandeSurCatalogueModel?> CreateDemandeSurCatalogueAsync(
        string titre, string description, Guid creePar, string urlCatalogue,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/demandes-sur-catalogue", new
        {
            Titre = titre, Description = description, CreePar = creePar, UrlCatalogue = urlCatalogue
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DemandeSurCatalogueModel>(ct);
    }

    public async Task<LigneCatalogueModel?> AjouterLigneAsync(
        Guid demandeId, string reference, string designation, int quantite, double prixUnitaire,
        string? urlProduit, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync($"api/demandes-sur-catalogue/{demandeId}/lignes", new
        {
            Reference = reference, Designation = designation,
            Quantite = quantite, PrixUnitaire = prixUnitaire, UrlProduit = urlProduit
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<LigneCatalogueModel>(ct);
    }

    // ── DemandeRepartitionGeo ─────────────────────────────────────────────────

    public Task<IReadOnlyList<DemandeRepartitionGeoModel>?> GetDemandesRepartitionGeoAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<DemandeRepartitionGeoModel>>("api/demandes-repartition-geo", ct);

    public Task<DemandeRepartitionGeoModel?> GetDemandeRepartitionGeoByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<DemandeRepartitionGeoModel>($"api/demandes-repartition-geo/{id}", ct);

    public async Task<DemandeRepartitionGeoModel?> CreateDemandeRepartitionGeoAsync(
        string titre, string description, Guid creePar,
        int nombreRessourcesRequises, string descriptionMission, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/demandes-repartition-geo", new
        {
            Titre = titre, Description = description, CreePar = creePar,
            NombreRessourcesRequises = nombreRessourcesRequises, DescriptionMission = descriptionMission
        }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DemandeRepartitionGeoModel>(ct);
    }

    // ── Médias ────────────────────────────────────────────────────────────────

    public Task<IReadOnlyList<MediaModel>?> GetMediasAsync(Guid propositionId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<MediaModel>>($"api/propositions/{propositionId}/medias", ct);

    public async Task<MediaModel?> AttacherMediaAsync(
        Guid propositionId, string url, string type, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync(
            $"api/propositions/{propositionId}/medias",
            new { Url = url, Type = type }, ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MediaModel>(ct);
    }

    // ── Discussion ────────────────────────────────────────────────────────────

    public Task<DiscussionData?> GetDiscussionAsync(Guid transactionId, CancellationToken ct = default)
        => _http.GetFromJsonAsync<DiscussionData>($"api/transactions/{transactionId}/discussion", ct);

    public async Task<MessageModel?> EnvoyerMessageAsync(
        Guid transactionId, Guid expediteurId, string contenu, string langue = "fr",
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync(
            $"api/transactions/{transactionId}/messages",
            new { TransactionId = transactionId, ExpediteurId = expediteurId, Contenu = contenu, Langue = langue },
            ct);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MessageModel>(ct);
    }
}
