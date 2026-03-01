using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

[Authorize]
public class OffreEditModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public OffreEditModel(ApiClient api) => _api = api;

    [BindProperty(SupportsGet = true)] public Guid Id { get; set; }
    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public bool LivraisonIncluse { get; set; }

    public OffreModel? Offre { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Offre = await _api.GetOffreByIdAsync(Id, ct);
            if (Offre is not null)
            {
                Titre = Offre.Titre;
                Description = Offre.Description;
                LivraisonIncluse = Offre.LivraisonIncluse;
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
            var result = await _api.UpdateOffreAsync(Id, Titre, Description, LivraisonIncluse, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Offre « {result.Titre} » mise à jour."
                : "Impossible de mettre à jour l'offre.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage("/Propositions/Offres");
    }
}
