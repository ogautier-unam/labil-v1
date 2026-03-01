using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandesQuota.CreateDemandeQuota;

public class CreateDemandeQuotaCommandValidator : AbstractValidator<CreateDemandeQuotaCommand>
{
    public CreateDemandeQuotaCommandValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CreePar).NotEmpty();
        RuleFor(x => x.CapaciteMax).GreaterThan(0);
        RuleFor(x => x.UniteCapacite).NotEmpty().MaximumLength(50);
    }
}
