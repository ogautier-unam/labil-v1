using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Transactions;

[Authorize]
public class DiscussionModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public DiscussionModel(ApiClient api) => _api = api;

    [BindProperty(SupportsGet = true)]
    public Guid TransactionId { get; set; }

    [BindProperty]
    public string Contenu { get; set; } = string.Empty;

    public DiscussionData? Discussion { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (TransactionId == Guid.Empty) return;
        try
        {
            Discussion = await _api.GetDiscussionAsync(TransactionId, ct);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(Contenu))
        {
            TempData[KeyError] = "Le message ne peut pas être vide.";
            return RedirectToPage(new { transactionId = TransactionId });
        }

        var userId = GetUserId(User);

        try
        {
            var msg = await _api.EnvoyerMessageAsync(TransactionId, userId, Contenu, ct: ct);
            TempData[msg is not null ? KeySuccess : KeyError] = msg is not null
                ? "Message envoyé."
                : "Impossible d'envoyer le message.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage(new { transactionId = TransactionId });
    }

    public async Task<IActionResult> OnPostBasculerVisibiliteAsync(string visibilite, CancellationToken ct)
    {
        try
        {
            var ok = await _api.BasculerVisibiliteDiscussionAsync(TransactionId, visibilite, ct);
            TempData[ok ? KeySuccess : KeyError] = ok
                ? $"Discussion passée en mode {visibilite}."
                : "Impossible de modifier la visibilité.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage(new { transactionId = TransactionId });
    }

    private static Guid GetUserId(ClaimsPrincipal user)
    {
        var raw = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
    }
}
