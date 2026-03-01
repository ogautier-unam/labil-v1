using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandesQuota.SoumettreIntentionDon;

public class SoumettreIntentionDonCommandValidator : AbstractValidator<SoumettreIntentionDonCommand>
{
    public SoumettreIntentionDonCommandValidator()
    {
        RuleFor(x => x.DemandeQuotaId).NotEmpty();
        RuleFor(x => x.ActeurId).NotEmpty();
        RuleFor(x => x.Quantite).GreaterThan(0);
        RuleFor(x => x.Unite).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
