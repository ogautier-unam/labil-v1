using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Entites;

public class DetailModel : PageModel
{
    private readonly ApiClient _api;

    public DetailModel(ApiClient api) => _api = api;

    public EntiteModel? Entite { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(Guid id, CancellationToken ct)
    {
        try
        {
            Entite = await _api.GetEntiteByIdAsync(id, ct);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }
}
