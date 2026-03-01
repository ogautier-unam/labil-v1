using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.AjouterLigneCatalogue;

public class AjouterLigneCatalogueValidator : AbstractValidator<AjouterLigneCatalogueCommand>
{
    public AjouterLigneCatalogueValidator()
    {
        RuleFor(x => x.DemandeSurCatalogueId).NotEmpty();
        RuleFor(x => x.Reference).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Quantite).GreaterThan(0);
        RuleFor(x => x.PrixUnitaire).GreaterThanOrEqualTo(0);
    }
}
