using FluentValidation;

namespace CrisisConnect.Application.UseCases.Demandes.CreateDemande;

public class CreateDemandeValidator : AbstractValidator<CreateDemandeCommand>
{
    public CreateDemandeValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CreePar).NotEmpty();
    }
}
