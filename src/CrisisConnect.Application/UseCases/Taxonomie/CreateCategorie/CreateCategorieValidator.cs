using FluentValidation;

namespace CrisisConnect.Application.UseCases.Taxonomie.CreateCategorie;

public class CreateCategorieValidator : AbstractValidator<CreateCategorieCommand>
{
    public CreateCategorieValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.NomJson).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ConfigId).NotEmpty();
    }
}
