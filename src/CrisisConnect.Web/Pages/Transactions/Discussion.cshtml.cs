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
            TempData["Error"] = "Le message ne peut pas être vide.";
            return RedirectToPage(new { transactionId = TransactionId });
        }

        var userId = Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

        try
        {
            var msg = await _api.EnvoyerMessageAsync(TransactionId, userId, Contenu, ct: ct);
            TempData["Success"] = msg is not null ? "Message envoyé." : "Impossible d'envoyer le message.";
        }
        catch (HttpRequestException) { TempData["Error"] = "Impossible de contacter l'API."; }

        return RedirectToPage(new { transactionId = TransactionId });
    }

    private Guid UserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
    }
}
