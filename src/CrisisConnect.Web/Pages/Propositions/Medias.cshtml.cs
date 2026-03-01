using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

[Authorize]
public class MediasModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public MediasModel(ApiClient api) => _api = api;

    public IReadOnlyList<MediaModel> Medias { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [BindProperty(SupportsGet = true)] public Guid PropositionId { get; set; }
    [BindProperty] public string Url { get; set; } = string.Empty;
    [BindProperty] public string Type { get; set; } = "Photo";

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (PropositionId == Guid.Empty) return;
        try
        {
            Medias = await _api.GetMediasAsync(PropositionId, ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API.";
        }
    }

    public async Task<IActionResult> OnPostAttacherAsync(CancellationToken ct)
    {
        try
        {
            var result = await _api.AttacherMediaAsync(PropositionId, Url, Type, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? $"Média {result.Type} attaché."
                : "Impossible d'attacher le média.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage(new { PropositionId });
    }
}
