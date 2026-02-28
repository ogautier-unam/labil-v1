using FluentValidation;

namespace CrisisConnect.Application.UseCases.Auth.Register;

public class RegisterActeurValidator : AbstractValidator<RegisterActeurCommand>
{
    private static readonly string[] RolesValides = ["Citoyen", "Benevole", "Coordinateur", "Responsable"];

    public RegisterActeurValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.MotDePasse).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Role).NotEmpty().Must(r => RolesValides.Contains(r))
            .WithMessage("Rôle invalide. Valeurs acceptées : Citoyen, Benevole, Coordinateur, Responsable.");
        RuleFor(x => x.Prenom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
    }
}
