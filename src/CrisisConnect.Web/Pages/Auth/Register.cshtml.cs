using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CrisisConnect.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly ApiClient _api;

    public RegisterModel(ApiClient api) => _api = api;

    [BindProperty]
    [Required(ErrorMessage = "Le prénom est requis.")]
    [MaxLength(100)]
    public string Prenom { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Le nom est requis.")]
    [MaxLength(100)]
    public string Nom { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "L'e-mail est requis.")]
    [EmailAddress(ErrorMessage = "Format d'e-mail invalide.")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [MinLength(8, ErrorMessage = "Le mot de passe doit comporter au moins 8 caractères.")]
    public string MotDePasse { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Le rôle est requis.")]
    public string Role { get; set; } = string.Empty;

    [BindProperty]
    public string? Telephone { get; set; }

    public string? ErrorMessage { get; private set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        var auth = await _api.RegisterAsync(Email, MotDePasse, Role, Prenom, Nom, Telephone, ct);
        if (auth is null)
        {
            ErrorMessage = "L'inscription a échoué. L'adresse e-mail est peut-être déjà utilisée.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, auth.UserId.ToString()),
            new(ClaimTypes.Email, auth.Email),
            new(ClaimTypes.Name, auth.Email),
            new(ClaimTypes.Role, auth.Role),
            new("access_token", auth.AccessToken),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToPage("/Index");
    }
}
