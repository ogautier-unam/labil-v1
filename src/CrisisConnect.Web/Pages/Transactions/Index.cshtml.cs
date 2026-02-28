using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Transactions;

public class TransactionsIndexModel : PageModel
{
    private readonly ApiClient _api;

    public TransactionsIndexModel(ApiClient api) => _api = api;

    public IReadOnlyList<TransactionModel> Transactions { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            Transactions = await _api.GetTransactionsAsync(ct) ?? [];
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }
}
