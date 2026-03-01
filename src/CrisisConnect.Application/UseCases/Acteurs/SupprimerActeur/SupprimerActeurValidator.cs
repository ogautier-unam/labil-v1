using FluentValidation;

namespace CrisisConnect.Application.UseCases.Acteurs.SupprimerActeur;

public class SupprimerActeurValidator : AbstractValidator<SupprimerActeurCommand>
{
    public SupprimerActeurValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
