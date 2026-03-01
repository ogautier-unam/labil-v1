using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

[Authorize]
public class DemandesSurCatalogueModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string LoginPage = "/Auth/Login";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public DemandesSurCatalogueModel(ApiClient api) => _api = api;

    public IReadOnlyList<DemandeSurCatalogueModel> Demandes { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    // Création demande sur catalogue
    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public string UrlCatalogue { get; set; } = string.Empty;

    // Ajout de ligne
    [BindProperty] public string Reference { get; set; } = string.Empty;
    [BindProperty] public string Designation { get; set; } = string.Empty;
    [BindProperty] public int Quantite { get; set; } = 1;
    [BindProperty] public double PrixUnitaire { get; set; }
    [BindProperty] public string? UrlProduit { get; set; }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Demandes = await _api.GetDemandesSurCatalogueAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostCreerAsync(CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return RedirectToPage(LoginPage);

        try
        {
            var result = await _api.CreateDemandeSurCatalogueAsync(Titre, Description, userId, UrlCatalogue, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Demande sur catalogue « {result.Titre} » créée."
                : "Impossible de créer la demande sur catalogue.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAjouterLigneAsync(Guid demandeId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);

        try
        {
            var result = await _api.AjouterLigneAsync(demandeId, Reference, Designation, Quantite, PrixUnitaire, UrlProduit, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Ligne « {result.Designation} » ajoutée."
                : "Impossible d'ajouter la ligne.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }
}
