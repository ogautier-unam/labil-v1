using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Paniers;

[Authorize]
public class PaniersIndexModel : PageModel
{
    private readonly ApiClient _api;

    public PaniersIndexModel(ApiClient api) => _api = api;

    public PanierModel? Panier { get; private set; }
    public string? SuccessMessage { get; private set; }
    public string? ErrorMessage { get; private set; }

    private Guid ProprietaireId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Panier = await _api.GetPanierAsync(ProprietaireId, ct);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API.";
        }
    }

    public async Task<IActionResult> OnPostCréerAsync(CancellationToken ct)
    {
        try
        {
            Panier = await _api.CreatePanierAsync(ProprietaireId, ct);
            if (Panier is null)
                ErrorMessage = "Impossible de créer le panier (un panier ouvert existe peut-être déjà).";
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API.";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostConfirmerAsync(Guid panierId, CancellationToken ct)
    {
        try
        {
            var ok = await _api.ConfirmerPanierAsync(panierId, ct);
            SuccessMessage = ok ? "Panier confirmé avec succès." : "Impossible de confirmer le panier.";
            Panier = await _api.GetPanierAsync(ProprietaireId, ct);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API.";
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAnnulerAsync(Guid panierId, CancellationToken ct)
    {
        try
        {
            var ok = await _api.AnnulerPanierAsync(panierId, ct);
            SuccessMessage = ok ? "Panier annulé." : "Impossible d'annuler le panier.";
            Panier = await _api.GetPanierAsync(ProprietaireId, ct);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API.";
        }
        return Page();
    }
}
