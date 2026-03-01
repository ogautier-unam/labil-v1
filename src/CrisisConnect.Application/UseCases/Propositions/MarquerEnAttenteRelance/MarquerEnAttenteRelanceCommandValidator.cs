using FluentValidation;

namespace CrisisConnect.Application.UseCases.Propositions.MarquerEnAttenteRelance;

public class MarquerEnAttenteRelanceCommandValidator : AbstractValidator<MarquerEnAttenteRelanceCommand>
{
    public MarquerEnAttenteRelanceCommandValidator()
    {
        RuleFor(x => x.PropositionId).NotEmpty();
    }
}
