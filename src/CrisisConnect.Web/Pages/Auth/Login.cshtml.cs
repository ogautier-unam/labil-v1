using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrisisConnect.Web.Pages.Auth;

public class LoginModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "L'e-mail est requis.")]
    [EmailAddress(ErrorMessage = "Format d'e-mail invalide.")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    public string MotDePasse { get; set; } = string.Empty;

    public string? ErrorMessage { get; private set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        // TODO : appel POST /api/auth/login via ApiClient, stockage du token JWT
        ErrorMessage = "Authentification non encore implémentée.";
        return Page();
    }
}
