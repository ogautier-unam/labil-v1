using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages;

public class IndexModel(ApiClient api) : PageModel
{
    public int NombreOffres { get; private set; }
    public int NombreOffresActives { get; private set; }
    public int NombreDemandes { get; private set; }
    public int NombreDemandesActives { get; private set; }
    public int NombreTransactions { get; private set; }
    public int NombreTransactionsEnCours { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            var offres      = await api.GetOffresAsync(ct);
            var demandes    = await api.GetDemandesAsync(ct);
            var transactions = await api.GetTransactionsAsync(ct);

            NombreOffres         = offres?.Count ?? 0;
            NombreOffresActives  = offres?.Count(o => o.Statut == "Active") ?? 0;
            NombreDemandes       = demandes?.Count ?? 0;
            NombreDemandesActives = demandes?.Count(d => d.Statut == "Active") ?? 0;
            NombreTransactions   = transactions?.Count ?? 0;
            NombreTransactionsEnCours = transactions?.Count(t => t.Statut == "EnCours") ?? 0;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }
}
