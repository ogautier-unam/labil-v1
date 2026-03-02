using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Taxonomie;

public class TaxonomieIndexModel : PageModel
{
    private readonly ApiClient _api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    public TaxonomieIndexModel(ApiClient api) => _api = api;

    [BindProperty(SupportsGet = true)]
    public Guid? ConfigId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Langue { get; set; } = "fr";

    public IReadOnlyList<CategorieTaxonomieModel> Categories { get; private set; } = [];

    [BindProperty] public string CreerCode { get; set; } = string.Empty;
    [BindProperty] public string CreerNom { get; set; } = string.Empty;
    [BindProperty] public Guid CreerConfigId { get; set; }
    [BindProperty] public Guid? CreerParentId { get; set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (ConfigId is { } id)
        {
            try
            {
                Categories = await _api.GetCategoriesAsync(id, Langue, ct) ?? [];
            }
            catch (HttpRequestException)
            {
                TempData[KeyError] = ErrApi;
            }
        }
    }

    public async Task<IActionResult> OnPostCreerAsync(CancellationToken ct)
    {
        if (!User.IsInRole("Coordinateur") && !User.IsInRole("Responsable"))
            return Forbid();

        try
        {
            var nomJson = $"{{\"fr\":\"{CreerNom}\"}}";
            var result = await _api.CreateCategorieAsync(CreerCode, nomJson, CreerConfigId, CreerParentId, ct);
            TempData[KeySuccess] = result is not null
                ? $"Catégorie « {CreerCode} » créée."
                : "Impossible de créer la catégorie.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }
        return RedirectToPage(new { ConfigId = CreerConfigId });
    }

    public async Task<IActionResult> OnPostDesactiverAsync(Guid categorieId, Guid configId, CancellationToken ct)
    {
        if (!User.IsInRole("Coordinateur") && !User.IsInRole("Responsable"))
            return Forbid();

        try
        {
            var ok = await _api.DesactiverCategorieAsync(categorieId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Catégorie désactivée." : "Impossible de désactiver.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }
        return RedirectToPage(new { ConfigId = configId });
    }
}
