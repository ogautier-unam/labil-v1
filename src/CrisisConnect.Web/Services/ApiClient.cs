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

    // ── Transactions ──────────────────────────────────────────────────────────

    public Task<IReadOnlyList<TransactionModel>?> GetTransactionsAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<IReadOnlyList<TransactionModel>>("api/transactions", ct);

    public Task<TransactionModel?> GetTransactionByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<TransactionModel>($"api/transactions/{id}", ct);
}
