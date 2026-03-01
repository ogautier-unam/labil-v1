using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.RecyclerProposition;

public class RecyclerPropositionCommandValidator : AbstractValidator<RecyclerPropositionCommand>
{
    public RecyclerPropositionCommandValidator()
    {
        RuleFor(x => x.PropositionId).NotEmpty();
    }
}
