using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.CreatePropositionAvecValidation;

public class CreatePropositionAvecValidationCommandValidator
    : AbstractValidator<CreatePropositionAvecValidationCommand>
{
    public CreatePropositionAvecValidationCommandValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CreePar).NotEmpty();
        RuleFor(x => x.DescriptionValidation).NotEmpty().MaximumLength(1000);
    }
}
