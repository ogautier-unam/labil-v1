using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Suggestions;

[Authorize]
public class SuggestionsIndexModel : PageModel
{
    private readonly ApiClient _api;

    public SuggestionsIndexModel(ApiClient api) => _api = api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";

    public IReadOnlyList<SuggestionAppariementModel> Suggestions { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [BindProperty]
    public Guid DemandeIdGeneration { get; set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Suggestions = await _api.GetSuggestionsPendingAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
        catch (Exception)
        {
            // Rôle insuffisant (403) → liste vide sans erreur bloquante
            Suggestions = [];
        }
    }

    public async Task<IActionResult> OnPostGenererAsync(CancellationToken ct)
    {
        if (DemandeIdGeneration == Guid.Empty)
        {
            TempData[KeyError] = "Identifiant de demande invalide.";
            return RedirectToPage();
        }
        try
        {
            var nouvelles = await _api.GenererSuggestionsAsync(DemandeIdGeneration, ct);
            var count = nouvelles?.Count ?? 0;
            TempData[KeySuccess] = count > 0
                ? $"{count} nouvelle(s) suggestion(s) générée(s)."
                : "Aucune nouvelle suggestion (toutes déjà générées ou score insuffisant).";
        }
        catch (HttpRequestException) { TempData[KeyError] = "Impossible de contacter l'API."; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAcknowledgeAsync(Guid suggestionId, CancellationToken ct)
    {
        try
        {
            var ok = await _api.AcknowledgeSuggestionAsync(suggestionId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok
                ? "Suggestion acquittée."
                : "Impossible d'acquitter la suggestion.";
        }
        catch (HttpRequestException) { TempData[KeyError] = "Impossible de contacter l'API."; }
        return RedirectToPage();
    }
}
