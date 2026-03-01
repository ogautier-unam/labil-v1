using FluentValidation;

namespace CrisisConnect.Application.UseCases.Demandes.UpdateDemande;

public class UpdateDemandeCommandValidator : AbstractValidator<UpdateDemandeCommand>
{
    public UpdateDemandeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).When(x => x.Latitude.HasValue);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).When(x => x.Longitude.HasValue);
    }
}
