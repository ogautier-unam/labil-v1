using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.CreateProposition;

public class CreatePropositionValidator : AbstractValidator<CreatePropositionCommand>
{
    public CreatePropositionValidator()
    {
        RuleFor(x => x.Titre).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CreePar).NotEmpty();
    }
}
