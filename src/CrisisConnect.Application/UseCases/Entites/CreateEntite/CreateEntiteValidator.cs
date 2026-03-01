using FluentValidation;

namespace CrisisConnect.Application.UseCases.Entites.CreateEntite;

public class CreateEntiteValidator : AbstractValidator<CreateEntiteCommand>
{
    public CreateEntiteValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.MotDePasse).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.MoyensContact).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ResponsableId).NotEmpty();
    }
}
