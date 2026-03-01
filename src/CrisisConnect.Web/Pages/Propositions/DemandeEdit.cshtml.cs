using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

[Authorize]
public class DemandeEditModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public DemandeEditModel(ApiClient api) => _api = api;

    [BindProperty(SupportsGet = true)] public Guid Id { get; set; }
    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public string Urgence { get; set; } = "Moyen";
    [BindProperty] public string? RegionSeverite { get; set; }

    public DemandeModel? Demande { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Demande = await _api.GetDemandeByIdAsync(Id, ct);
            if (Demande is not null)
            {
                Titre = Demande.Titre;
                Description = Demande.Description;
                Urgence = Demande.Urgence;
                RegionSeverite = Demande.RegionSeverite;
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        try
        {
            var result = await _api.UpdateDemandeAsync(Id, Titre, Description, Urgence, RegionSeverite, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Demande « {result.Titre} » mise à jour."
                : "Impossible de mettre à jour la demande.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage("/Propositions/Demandes");
    }
}
