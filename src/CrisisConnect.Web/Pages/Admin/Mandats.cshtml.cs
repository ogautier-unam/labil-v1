using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Admin;

[Authorize]
public class MandatsModel : PageModel
{
    private readonly ApiClient _api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    public MandatsModel(ApiClient api) => _api = api;

    [BindProperty(SupportsGet = true)]
    public Guid? MandantId { get; set; }

    public IReadOnlyList<MandatModel> Mandats { get; private set; } = [];

    [BindProperty] public Guid CreerMandantId { get; set; }
    [BindProperty] public Guid CreerMandataireId { get; set; }
    [BindProperty] public string CreerPortee { get; set; } = string.Empty;
    [BindProperty] public string CreerDescription { get; set; } = string.Empty;
    [BindProperty] public bool CreerEstPublic { get; set; }
    [BindProperty] public DateTime CreerDateDebut { get; set; } = DateTime.Today;
    [BindProperty] public DateTime? CreerDateFin { get; set; }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (MandantId is { } id)
        {
            try
            {
                Mandats = await _api.GetMandatsAsync(id, ct) ?? [];
            }
            catch (HttpRequestException)
            {
                TempData[KeyError] = ErrApi;
            }
        }
    }

    public async Task<IActionResult> OnPostCreerAsync(CancellationToken ct)
    {
        if (GetUserId(User) is null)
            return RedirectToPage("/Auth/Login");

        try
        {
            var result = await _api.CreerMandatAsync(
                new(CreerMandantId, CreerMandataireId, CreerPortee,
                    CreerDescription, CreerEstPublic, CreerDateDebut, CreerDateFin), ct);

            TempData[KeySuccess] = result is not null
                ? "Mandat créé avec succès."
                : "Impossible de créer le mandat.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }

        return RedirectToPage(new { MandantId = CreerMandantId });
    }

    public async Task<IActionResult> OnPostRevoquerAsync(Guid mandatId, Guid mandantId, CancellationToken ct)
    {
        if (!User.IsInRole("Responsable"))
            return Forbid();

        try
        {
            var ok = await _api.RevoquerMandatAsync(mandatId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Mandat révoqué." : "Impossible de révoquer ce mandat.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }

        return RedirectToPage(new { MandantId = mandantId });
    }
}
