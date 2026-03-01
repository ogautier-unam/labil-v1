using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.ArchiverProposition;

public class ArchiverPropositionCommandValidator : AbstractValidator<ArchiverPropositionCommand>
{
    public ArchiverPropositionCommandValidator()
    {
        RuleFor(x => x.PropositionId).NotEmpty();
    }
}
