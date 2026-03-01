using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandeRepartitionGeo.CreateDemandeRepartitionGeo;

public class CreateDemandeRepartitionGeoValidator : AbstractValidator<CreateDemandeRepartitionGeoCommand>
{
    public CreateDemandeRepartitionGeoValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CreePar).NotEmpty();
        RuleFor(x => x.NombreRessourcesRequises).GreaterThan(0);
        RuleFor(x => x.DescriptionMission).NotEmpty().MaximumLength(2000);
    }
}
