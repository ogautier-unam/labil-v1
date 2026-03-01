using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

public class OffresModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string LoginPage = "/Auth/Login";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public OffresModel(ApiClient api) => _api = api;

    public IReadOnlyList<OffreModel> Offres { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [BindProperty] public string Titre { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    [BindProperty] public bool LivraisonIncluse { get; set; }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
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
        if (GetUserId(User) is not { } userId) return RedirectToPage(LoginPage);

        try
        {
            var offre = await _api.CreateOffreAsync(Titre, Description, userId, LivraisonIncluse, ct);
            TempData[offre is not null ? KeySuccess : KeyError] = offre is not null
                ? $"Offre « {offre.Titre} » publiée avec succès."
                : "Impossible de créer l'offre.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostArchiverAsync(Guid offreId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.ArchiverPropositionAsync(offreId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Offre archivée." : "Impossible d'archiver cette offre.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCloreAsync(Guid offreId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.ClorePropositionAsync(offreId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Offre clôturée." : "Impossible de clôturer cette offre.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRelancerAsync(Guid offreId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.RelancerPropositionAsync(offreId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Offre mise en attente de relance." : "Impossible de relancer cette offre.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostReconfirmerAsync(Guid offreId, CancellationToken ct)
    {
        if (GetUserId(User) is null) return RedirectToPage(LoginPage);
        try
        {
            var ok = await _api.ReconfirmerPropositionAsync(offreId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Offre reconfirmée (Active)." : "Impossible de reconfirmer cette offre.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAjouterAuPanierAsync(Guid offreId, CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return RedirectToPage(LoginPage);

        try
        {
            var panier = await _api.GetPanierAsync(userId, ct);
            panier ??= await _api.CreatePanierAsync(userId, ct);

            if (panier is null)
            {
                TempData[KeyError] = "Impossible d'obtenir votre panier.";
                return RedirectToPage();
            }

            await _api.AjouterOffreAuPanierAsync(panier.Id, offreId, ct);
            TempData[KeySuccess] = "Offre ajoutée au panier.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }
}
