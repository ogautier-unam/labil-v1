using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

public class DemandesModel : PageModel
{
    private readonly ApiClient _api;

    public DemandesModel(ApiClient api) => _api = api;

    public IReadOnlyList<DemandeModel> Demandes { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public string Urgence { get; set; } = "Moyen";
    [BindProperty] public string? RegionSeverite { get; set; }

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
        var val = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (val is null) return RedirectToPage("/Auth/Login");
        var userId = Guid.Parse(val);

        try
        {
            var demande = await _api.CreateDemandeAsync(Titre, Description, userId, Urgence, RegionSeverite, ct);
            TempData["Success"] = demande is not null
                ? $"Demande « {demande.Titre} » publiée avec succès."
                : "Impossible de créer la demande.";
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = "Impossible de contacter l'API.";
        }

        return RedirectToPage();
    }
}
