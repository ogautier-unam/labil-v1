using FluentValidation;

namespace CrisisConnect.Application.UseCases.Paniers.CreatePanier;

public class CreatePanierValidator : AbstractValidator<CreatePanierCommand>
{
    public CreatePanierValidator()
    {
        RuleFor(x => x.ProprietaireId).NotEmpty();
    }
}
