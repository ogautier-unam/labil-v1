using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Propositions;

public class PropositionsIndexModel : PageModel
{
    private readonly ApiClient _api;

    public PropositionsIndexModel(ApiClient api) => _api = api;

    public IReadOnlyList<PropositionModel> Propositions { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Propositions = await _api.GetPropositionsAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }
}
