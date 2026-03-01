using FluentValidation;

namespace CrisisConnect.Application.UseCases.MethodesIdentification.VerifierMethode;

public class VerifierMethodeCommandValidator : AbstractValidator<VerifierMethodeCommand>
{
    public VerifierMethodeCommandValidator()
    {
        RuleFor(x => x.MethodeId).NotEmpty();
    }
}
