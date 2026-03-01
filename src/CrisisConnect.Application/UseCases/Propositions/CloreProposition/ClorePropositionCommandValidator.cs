using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.CloreProposition;

public class ClorePropositionCommandValidator : AbstractValidator<ClorePropositionCommand>
{
    public ClorePropositionCommandValidator()
    {
        RuleFor(x => x.PropositionId).NotEmpty();
    }
}
