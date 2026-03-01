using FluentValidation;

namespace CrisisConnect.Application.UseCases.Paniers.AnnulerPanier;

public class AnnulerPanierCommandValidator : AbstractValidator<AnnulerPanierCommand>
{
    public AnnulerPanierCommandValidator()
    {
        RuleFor(x => x.PanierId).NotEmpty();
    }
}
