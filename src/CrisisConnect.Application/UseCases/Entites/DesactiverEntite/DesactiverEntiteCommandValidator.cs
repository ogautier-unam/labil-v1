using FluentValidation;

namespace CrisisConnect.Application.UseCases.Entites.DesactiverEntite;

public class DesactiverEntiteCommandValidator : AbstractValidator<DesactiverEntiteCommand>
{
    public DesactiverEntiteCommandValidator()
    {
        RuleFor(x => x.EntiteId).NotEmpty();
    }
}
