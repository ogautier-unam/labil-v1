using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

public class OffresModel : PageModel
{
    private readonly ApiClient _api;

    public OffresModel(ApiClient api) => _api = api;

    public IReadOnlyList<OffreModel> Offres { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public bool LivraisonIncluse { get; set; }

    private Guid? CurrentUserId()
    {
        var val = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Offres = await _api.GetOffresAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostPublierAsync(CancellationToken ct)
    {
        var userId = CurrentUserId();
        if (userId is null) return RedirectToPage("/Auth/Login");

        try
        {
            var offre = await _api.CreateOffreAsync(Titre, Description, userId.Value, LivraisonIncluse, ct);
            TempData["Success"] = offre is not null
                ? $"Offre « {offre.Titre} » publiée avec succès."
                : "Impossible de créer l'offre.";
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = "Impossible de contacter l'API.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAjouterAuPanierAsync(Guid offreId, CancellationToken ct)
    {
        var userId = CurrentUserId();
        if (userId is null) return RedirectToPage("/Auth/Login");

        try
        {
            // Récupérer ou créer le panier
            var panier = await _api.GetPanierAsync(userId.Value, ct);
            panier ??= await _api.CreatePanierAsync(userId.Value, ct);

            if (panier is null)
            {
                TempData["Error"] = "Impossible d'obtenir votre panier.";
                return RedirectToPage();
            }

            await _api.AjouterOffreAuPanierAsync(panier.Id, offreId, ct);
            TempData["Success"] = "Offre ajoutée au panier.";
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = "Impossible de contacter l'API.";
        }

        return RedirectToPage();
    }
}
