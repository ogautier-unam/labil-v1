using System.Security.Claims;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

[Authorize]
public class AvecValidationModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string LoginPage = "/Auth/Login";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public AvecValidationModel(ApiClient api) => _api = api;

    // Création
    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public string DescriptionValidation { get; set; } = string.Empty;

    // Validation / Refus
    [BindProperty] public Guid ActionPropositionId { get; set; }
    [BindProperty] public Guid ActionEntiteId { get; set; }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public void OnGet()
    {
        // La page affiche uniquement des formulaires statiques — aucune donnée à charger.
    }

    public async Task<IActionResult> OnPostCreerAsync(CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return RedirectToPage(LoginPage);

        try
        {
            var result = await _api.CreatePropositionAvecValidationAsync(
                Titre, Description, userId, DescriptionValidation, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Proposition « {result.Titre} » soumise à validation."
                : "Impossible de créer la proposition.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostValiderAsync(CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        if (!User.IsInRole("Coordinateur") && !User.IsInRole("Responsable")) return Forbid();

        try
        {
            var ok = await _api.ValiderPropositionAsync(ActionPropositionId, ActionEntiteId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok
                ? "Proposition validée — maintenant Active."
                : "Impossible de valider la proposition.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRefuserValidationAsync(CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        if (!User.IsInRole("Coordinateur") && !User.IsInRole("Responsable")) return Forbid();

        try
        {
            var ok = await _api.RefuserValidationPropositionAsync(ActionPropositionId, ActionEntiteId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok
                ? "Validation refusée."
                : "Impossible de refuser la validation.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }
}
