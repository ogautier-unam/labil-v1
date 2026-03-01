using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandeSurCatalogue.CreateDemandeSurCatalogue;

public class CreateDemandeSurCatalogueValidator : AbstractValidator<CreateDemandeSurCatalogueCommand>
{
    public CreateDemandeSurCatalogueValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CreePar).NotEmpty();
        RuleFor(x => x.UrlCatalogue).NotEmpty().MaximumLength(500);
    }
}
