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
}
