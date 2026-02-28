using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages;

public class IndexModel(ApiClient api) : PageModel
{
    public int NombreOffres { get; private set; }
    public int NombreDemandes { get; private set; }
    public int NombreTransactions { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        var offres = await api.GetOffresAsync(ct);
        var demandes = await api.GetDemandesAsync(ct);
        var transactions = await api.GetTransactionsAsync(ct);

        NombreOffres = offres?.Count ?? 0;
        NombreDemandes = demandes?.Count ?? 0;
        NombreTransactions = transactions?.Count ?? 0;
    }
}
