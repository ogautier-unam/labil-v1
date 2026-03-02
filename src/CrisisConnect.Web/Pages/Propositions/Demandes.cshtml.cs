using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

public class DemandesModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string LoginPage = "/Auth/Login";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public DemandesModel(ApiClient api) => _api = api;

    public IReadOnlyList<DemandeModel> Demandes { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public string Urgence { get; set; } = "Moyen";
    [BindProperty] public string? RegionSeverite { get; set; }
    [BindProperty] public bool EstRecurrente { get; set; }
    [BindProperty] public string? FrequenceRecurrence { get; set; }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Demandes = await _api.GetDemandesAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostPublierAsync(CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return RedirectToPage(LoginPage);

        try
        {
            var demande = await _api.CreateDemandeAsync(
                new CreateDemandeRequest(Titre, Description, userId, Urgence, RegionSeverite, EstRecurrente, FrequenceRecurrence), ct);
            TempData[demande is not null ? KeySuccess : KeyError] = demande is not null
                ? $"Demande « {demande.Titre} » publiée avec succès."
                : "Impossible de créer la demande.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostArchiverAsync(Guid demandeId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.ArchiverPropositionAsync(demandeId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Demande archivée." : "Impossible d'archiver cette demande.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCloreAsync(Guid demandeId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.ClorePropositionAsync(demandeId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Demande clôturée." : "Impossible de clôturer cette demande.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRelancerAsync(Guid demandeId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.RelancerPropositionAsync(demandeId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Demande remise en attente de relance." : "Impossible de relancer cette demande.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostReconfirmerAsync(Guid demandeId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.ReconfirmerPropositionAsync(demandeId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Demande reconfirmée (Active)." : "Impossible de reconfirmer cette demande.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }
}
