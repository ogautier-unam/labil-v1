using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Entites;

public class EntitesIndexModel : PageModel
{
    private readonly ApiClient _api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    public EntitesIndexModel(ApiClient api) => _api = api;

    public IReadOnlyList<EntiteModel> Entites { get; private set; } = [];

    [BindProperty] public string CreerEmail { get; set; } = string.Empty;
    [BindProperty] public string CreerMotDePasse { get; set; } = string.Empty;
    [BindProperty] public string CreerNom { get; set; } = string.Empty;
    [BindProperty] public string CreerDescription { get; set; } = string.Empty;
    [BindProperty] public string CreerMoyensContact { get; set; } = string.Empty;
    [BindProperty] public Guid CreerResponsableId { get; set; }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Entites = await _api.GetEntitesAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }
    }

    public async Task<IActionResult> OnPostCreerAsync(CancellationToken ct)
    {
        if (!User.IsInRole("Responsable"))
            return Forbid();

        if (GetUserId(User) is not { } userId)
            return RedirectToPage("/Auth/Login");

        // Si ResponsableId non renseigné, utiliser l'utilisateur courant
        var responsableId = CreerResponsableId == Guid.Empty ? userId : CreerResponsableId;

        try
        {
            var result = await _api.CreateEntiteAsync(
                CreerEmail, CreerMotDePasse, CreerNom, CreerDescription,
                CreerMoyensContact, responsableId, ct);
            TempData[KeySuccess] = result is not null
                ? $"Entité « {CreerNom} » créée."
                : "Impossible de créer l'entité (email déjà utilisé ?).";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDesactiverAsync(Guid entiteId, CancellationToken ct)
    {
        if (!User.IsInRole("Responsable"))
            return Forbid();

        try
        {
            var ok = await _api.DesactiverEntiteAsync(entiteId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Entité désactivée." : "Impossible de désactiver.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }
        return RedirectToPage();
    }
}
