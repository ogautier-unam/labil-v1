using FluentValidation;

namespace CrisisConnect.Application.UseCases.Paniers.AjouterOffreAuPanier;

public class AjouterOffreAuPanierValidator : AbstractValidator<AjouterOffreAuPanierCommand>
{
    public AjouterOffreAuPanierValidator()
    {
        RuleFor(x => x.PanierId).NotEmpty();
        RuleFor(x => x.OffreId).NotEmpty();
    }
}
