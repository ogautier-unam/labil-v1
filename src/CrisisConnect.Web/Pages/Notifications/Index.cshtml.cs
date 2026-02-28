using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Notifications;

[Authorize]
public class NotificationsIndexModel : PageModel
{
    private readonly ApiClient _api;

    public NotificationsIndexModel(ApiClient api) => _api = api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";

    public IReadOnlyList<NotificationModel> Notifications { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var userId = UserId();
        if (userId == Guid.Empty) return;

        try
        {
            Notifications = await _api.GetNotificationsAsync(userId, ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostMarquerLueAsync(Guid notificationId, CancellationToken ct)
    {
        try
        {
            var ok = await _api.MarkNotificationAsReadAsync(notificationId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Notification marquée comme lue." : "Impossible de marquer la notification.";
        }
        catch (HttpRequestException) { TempData[KeyError] = "Impossible de contacter l'API."; }
        return RedirectToPage();
    }

    private Guid UserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var id) ? id : Guid.Empty;
    }
}
