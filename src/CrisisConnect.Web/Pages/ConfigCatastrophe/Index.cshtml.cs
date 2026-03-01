using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.ConfigCatastrophe;

public class ConfigCatastropheIndexModel : PageModel
{
    private readonly ApiClient _api;

    public ConfigCatastropheIndexModel(ApiClient api) => _api = api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";

    public ConfigCatastropheModel? Config { get; private set; }
    public string? ErrorMessage { get; private set; }

    // Création
    [BindProperty] public string Nom { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public string ZoneGeographique { get; set; } = string.Empty;
    [BindProperty] public string EtatReferent { get; set; } = string.Empty;
    [BindProperty] public int DelaiArchivageJours { get; set; } = 30;
    [BindProperty] public int DelaiRappelAvantArchivage { get; set; } = 7;

    // Modification
    [BindProperty] public Guid ConfigId { get; set; }
    [BindProperty] public bool EstActive { get; set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Config = await _api.GetConfigCatastropheAsync(ct);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostModifierAsync(CancellationToken ct)
    {
        if (!User.IsInRole("Responsable")) return Forbid();
        try
        {
            var config = await _api.UpdateConfigCatastropheAsync(
                ConfigId, DelaiArchivageJours, DelaiRappelAvantArchivage, EstActive, ct);
            TempData[config is not null ? KeySuccess : KeyError] =
                config is not null ? "Configuration mise à jour." : "Impossible de mettre à jour la configuration.";
        }
        catch (HttpRequestException) { TempData[KeyError] = "Impossible de contacter l'API."; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCréerAsync(CancellationToken ct)
    {
        if (!User.IsInRole("Responsable")) return Forbid();
        try
        {
            var config = await _api.CreateConfigCatastropheAsync(
                Nom, Description, ZoneGeographique, EtatReferent,
                DelaiArchivageJours, DelaiRappelAvantArchivage, ct);
            TempData[config is not null ? KeySuccess : KeyError] =
                config is not null ? $"Configuration « {config.Nom} » créée." : "Impossible de créer la configuration.";
        }
        catch (HttpRequestException) { TempData[KeyError] = "Impossible de contacter l'API."; }
        return RedirectToPage();
    }
}
