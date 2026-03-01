using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

[Authorize]
public class DemandesQuotaModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string LoginPage = "/Auth/Login";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public DemandesQuotaModel(ApiClient api) => _api = api;

    public IReadOnlyList<DemandeQuotaModel> Demandes { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    // Création demande
    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public int CapaciteMax { get; set; } = 100;
    [BindProperty] public string UniteCapacite { get; set; } = string.Empty;
    [BindProperty] public string? AdresseDepot { get; set; }
    [BindProperty] public DateTime? DateLimit { get; set; }

    // Soumettre intention
    [BindProperty] public Guid IntentionDemandeId { get; set; }
    [BindProperty] public int Quantite { get; set; } = 1;
    [BindProperty] public string Unite { get; set; } = string.Empty;
    [BindProperty] public string DescriptionIntention { get; set; } = string.Empty;

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Demandes = await _api.GetDemandesQuotaAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostCreerAsync(CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return RedirectToPage(LoginPage);

        var req = new CreateDemandeQuotaRequest(
            Titre, Description, CapaciteMax, UniteCapacite, AdresseDepot, DateLimit);

        try
        {
            var result = await _api.CreateDemandeQuotaAsync(userId, req, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Demande quota « {result.Titre} » créée."
                : "Impossible de créer la demande quota.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSoumettreAsync(Guid demandeId, CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return RedirectToPage(LoginPage);

        try
        {
            var result = await _api.SoumettreIntentionDonAsync(
                demandeId, userId, Quantite, Unite, DescriptionIntention, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? "Intention soumise avec succès."
                : "Impossible de soumettre l'intention.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAccepterAsync(Guid demandeId, Guid intentionId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        if (!User.IsInRole("Coordinateur") && !User.IsInRole("Responsable")) return Forbid();

        try
        {
            var ok = await _api.AccepterIntentionDonAsync(demandeId, intentionId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Intention acceptée." : "Impossible d'accepter l'intention.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRefuserAsync(Guid demandeId, Guid intentionId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        if (!User.IsInRole("Coordinateur") && !User.IsInRole("Responsable")) return Forbid();

        try
        {
            var ok = await _api.RefuserIntentionDonAsync(demandeId, intentionId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Intention refusée." : "Impossible de refuser l'intention.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostConfirmerAsync(Guid demandeId, Guid intentionId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);

        try
        {
            var ok = await _api.ConfirmerIntentionDonAsync(demandeId, intentionId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Intention confirmée." : "Impossible de confirmer l'intention.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }
}
