using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.ValiderProposition;

public class ValiderPropositionCommandValidator : AbstractValidator<ValiderPropositionCommand>
{
    public ValiderPropositionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ValideurEntiteId).NotEmpty();
    }
}
