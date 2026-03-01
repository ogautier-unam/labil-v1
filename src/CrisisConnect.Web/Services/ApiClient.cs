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
