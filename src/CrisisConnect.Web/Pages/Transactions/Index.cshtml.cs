using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Transactions;

public class TransactionsIndexModel : PageModel
{
    private readonly ApiClient _api;

    public TransactionsIndexModel(ApiClient api) => _api = api;

    private const string KeySuccess = "Success";
    private const string KeyError = "Error";

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

    public async Task<IActionResult> OnPostConfirmerAsync(Guid transactionId, CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated != true) return RedirectToPage("/Auth/Login");
        try
        {
            var ok = await _api.ConfirmerTransactionAsync(transactionId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Transaction confirmée." : "Impossible de confirmer.";
        }
        catch (HttpRequestException) { TempData[KeyError] = "Impossible de contacter l'API."; }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAnnulerAsync(Guid transactionId, CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated != true) return RedirectToPage("/Auth/Login");
        try
        {
            var ok = await _api.AnnulerTransactionAsync(transactionId, ct);
            TempData[ok ? KeySuccess : KeyError] = ok ? "Transaction annulée." : "Impossible d'annuler.";
        }
        catch (HttpRequestException) { TempData[KeyError] = "Impossible de contacter l'API."; }
        return RedirectToPage();
    }
}
