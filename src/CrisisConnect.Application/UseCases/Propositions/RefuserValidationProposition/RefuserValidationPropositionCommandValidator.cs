using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.RefuserValidationProposition;

public class RefuserValidationPropositionCommandValidator
    : AbstractValidator<RefuserValidationPropositionCommand>
{
    public RefuserValidationPropositionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ValideurEntiteId).NotEmpty();
    }
}
