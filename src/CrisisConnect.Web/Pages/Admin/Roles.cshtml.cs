using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Admin;

[Authorize(Roles = "Coordinateur,Responsable")]
public class RolesModel : PageModel
{
    private readonly ApiClient _api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    public RolesModel(ApiClient api) => _api = api;

    [BindProperty(SupportsGet = true)]
    public Guid? ActeurId { get; set; }

    public IReadOnlyList<AttributionRoleModel> Roles { get; private set; } = [];

    [BindProperty] public Guid AttribuerActeurId { get; set; }
    [BindProperty] public string AttribuerTypeRole { get; set; } = string.Empty;
    [BindProperty] public DateTime AttribuerDateDebut { get; set; } = DateTime.Today;
    [BindProperty] public DateTime? AttribuerDateFin { get; set; }
    [BindProperty] public bool AttribuerReconductible { get; set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (ActeurId is { } id)
        {
            try
            {
                Roles = await _api.GetRolesActeurAsync(id, ct) ?? [];
            }
            catch (HttpRequestException)
            {
                TempData[KeyError] = ErrApi;
            }
        }
    }

    public async Task<IActionResult> OnPostAttribuerAsync(CancellationToken ct)
    {
        try
        {
            var result = await _api.AttribuerRoleAsync(
                AttribuerActeurId, AttribuerTypeRole, AttribuerDateDebut,
                AttribuerDateFin, AttribuerReconductible, ct);

            TempData[KeySuccess] = result is not null
                ? "Rôle attribué avec succès."
                : "Impossible d'attribuer le rôle.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }

        return RedirectToPage(new { ActeurId = AttribuerActeurId });
    }

    public async Task<IActionResult> OnPostRevoquerAsync(Guid roleId, Guid acteurId, CancellationToken ct)
    {
        if (!User.IsInRole("Responsable"))
            return Forbid();

        try
        {
            var ok = await _api.RevoquerRoleAsync(roleId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Rôle révoqué." : "Impossible de révoquer ce rôle.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }

        return RedirectToPage(new { ActeurId = acteurId });
    }

    private static bool IsActif(AttributionRoleModel r)
        => r.Statut != "Expire" && (r.DateFin is null || r.DateFin > DateTime.UtcNow);

    public static string StatutBadge(AttributionRoleModel r)
        => IsActif(r) ? "success" : "secondary";
}
