using FluentValidation;

namespace CrisisConnect.Application.UseCases.Taxonomie.DesactiverCategorie;

public class DesactiverCategorieCommandValidator : AbstractValidator<DesactiverCategorieCommand>
{
    public DesactiverCategorieCommandValidator()
    {
        RuleFor(x => x.CategorieId).NotEmpty();
    }
}
