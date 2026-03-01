using System.Security.Claims;
using CrisisConnect.Web.Models;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Profil;

[Authorize]
public class IndexModel : PageModel
{
    private const string KeySuccess = "Success";
    private const string KeyError = "Error";
    private const string ErrApi = "Impossible de contacter l'API.";

    private readonly ApiClient _api;

    public IndexModel(ApiClient api) => _api = api;

    public PersonneModel? Profil { get; private set; }
    public string? ErrorMessage { get; private set; }

    [BindProperty] public string Prenom { get; set; } = string.Empty;
    [BindProperty] public string Nom { get; set; } = string.Empty;
    [BindProperty] public string? Telephone { get; set; }
    [BindProperty] public string? UrlPhoto { get; set; }
    [BindProperty] public string? LanguePreferee { get; set; }
    [BindProperty] public string? MoyensContact { get; set; }
    [BindProperty] public string? Rue { get; set; }
    [BindProperty] public string? Ville { get; set; }
    [BindProperty] public string? CodePostal { get; set; }
    [BindProperty] public string? Pays { get; set; }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return val is not null ? Guid.Parse(val) : null;
    }

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return;
        try
        {
            Profil = await _api.GetActeurAsync(userId, ct);
            if (Profil is not null)
            {
                Prenom = Profil.Prenom;
                Nom = Profil.Nom;
                Telephone = Profil.Telephone;
                UrlPhoto = Profil.UrlPhoto;
                LanguePreferee = Profil.LanguePreferee;
                MoyensContact = Profil.MoyensContact;
                Rue = Profil.Rue;
                Ville = Profil.Ville;
                CodePostal = Profil.CodePostal;
                Pays = Profil.Pays;
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de contacter l'API. Vérifiez que le service est démarré.";
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (GetUserId(User) is not { } userId) return RedirectToPage("/Auth/Login");

        try
        {
            var req = new UpdateActeurRequest(
                Prenom, Nom, Telephone, UrlPhoto, LanguePreferee,
                MoyensContact, Rue, Ville, CodePostal, Pays);
            var result = await _api.UpdateActeurAsync(userId, req, ct);
            TempData[result is not null ? KeySuccess : KeyError] = result is not null
                ? "Profil mis à jour avec succès."
                : "Impossible de mettre à jour le profil.";
        }
        catch (HttpRequestException) { TempData[KeyError] = ErrApi; }

        return RedirectToPage();
    }
}
