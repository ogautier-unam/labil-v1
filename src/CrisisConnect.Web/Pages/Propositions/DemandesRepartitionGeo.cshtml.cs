using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

[Authorize]
public class DemandesRepartitionGeoModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string LoginPage = "/Auth/Login";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public DemandesRepartitionGeoModel(ApiClient api) => _api = api;

    public IReadOnlyList<DemandeRepartitionGeoModel> Demandes { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public int NombreRessourcesRequises { get; set; } = 1;
    [BindProperty] public string DescriptionMission { get; set; } = string.Empty;

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Demandes = await _api.GetDemandesRepartitionGeoAsync(ct) ?? [];
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
            var result = await _api.CreateDemandeRepartitionGeoAsync(
                Titre, Description, userId, NombreRessourcesRequises, DescriptionMission, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Demande de répartition géo « {result.Titre} » créée."
                : "Impossible de créer la demande de répartition géo.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }
}
