using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.MethodesIdentification;

[Authorize]
public class MethodesIdentificationIndexModel : PageModel
{
    private readonly ApiClient _api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    public MethodesIdentificationIndexModel(ApiClient api) => _api = api;

    [BindProperty(SupportsGet = true)]
    public Guid? PersonneId { get; set; }

    public IReadOnlyList<MethodeIdentificationModel> Methodes { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (PersonneId is { } id)
        {
            try
            {
                Methodes = await _api.GetMethodesAsync(id, ct) ?? [];
            }
            catch (HttpRequestException)
            {
                TempData[KeyError] = ErrApi;
            }
        }
    }

    public async Task<IActionResult> OnPostVerifierAsync(Guid methodeId, Guid personneId, CancellationToken ct)
    {
        if (!User.IsInRole("Coordinateur") && !User.IsInRole("Responsable"))
            return Forbid();

        try
        {
            var ok = await _api.VerifierMethodeAsync(methodeId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok
                ? "Méthode marquée comme vérifiée."
                : "Impossible de vérifier cette méthode.";
        }
        catch (HttpRequestException)
        {
            TempData[KeyError] = ErrApi;
        }
        return RedirectToPage(new { PersonneId = personneId });
    }
}
