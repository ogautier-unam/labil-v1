using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.ReconfirmerProposition;

public class ReconfirmerPropositionCommandValidator : AbstractValidator<ReconfirmerPropositionCommand>
{
    public ReconfirmerPropositionCommandValidator()
    {
        RuleFor(x => x.PropositionId).NotEmpty();
    }
}
