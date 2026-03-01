using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Journal;

[Authorize]
public class JournalIndexModel : PageModel
{
    private readonly ApiClient _api;

    public JournalIndexModel(ApiClient api) => _api = api;

    public IReadOnlyList<EntreeJournalModel> Entrees { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var userId = GetUserId(User);
        if (userId == Guid.Empty) return;

        try
        {
            Entrees = await _api.GetEntreesJournalAsync(userId, ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    private static Guid GetUserId(ClaimsPrincipal user)
    {
        var raw = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
    }
}
