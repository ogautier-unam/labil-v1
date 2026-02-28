using FluentValidation;

namespace CrisisConnect.Application.UseCases.Offres.CreateOffre;

public class CreateOffreValidator : AbstractValidator<CreateOffreCommand>
{
    public CreateOffreValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CreePar).NotEmpty();
    }
}
