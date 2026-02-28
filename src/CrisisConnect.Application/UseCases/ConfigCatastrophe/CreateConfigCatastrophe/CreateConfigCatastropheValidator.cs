using FluentValidation;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.CreateConfigCatastrophe;

public class CreateConfigCatastropheValidator : AbstractValidator<CreateConfigCatastropheCommand>
{
    public CreateConfigCatastropheValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.ZoneGeographique).NotEmpty().MaximumLength(300);
        RuleFor(x => x.EtatReferent).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DelaiArchivageJours).GreaterThan(0);
        RuleFor(x => x.DelaiRappelAvantArchivage).GreaterThan(0)
            .LessThan(x => x.DelaiArchivageJours);
    }
}
