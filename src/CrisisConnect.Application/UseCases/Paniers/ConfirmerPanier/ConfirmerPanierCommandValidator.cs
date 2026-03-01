using FluentValidation;

namespace CrisisConnect.Application.UseCases.Paniers.ConfirmerPanier;

public class ConfirmerPanierCommandValidator : AbstractValidator<ConfirmerPanierCommand>
{
    public ConfirmerPanierCommandValidator()
    {
        RuleFor(x => x.PanierId).NotEmpty();
    }
}
