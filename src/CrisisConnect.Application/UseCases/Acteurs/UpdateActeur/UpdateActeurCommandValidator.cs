using FluentValidation;

namespace CrisisConnect.Application.UseCases.Acteurs.UpdateActeur;

public class UpdateActeurCommandValidator : AbstractValidator<UpdateActeurCommand>
{
    public UpdateActeurCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Prenom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Telephone).MaximumLength(20).When(x => x.Telephone is not null);
        RuleFor(x => x.LanguePreferee).MaximumLength(10).When(x => x.LanguePreferee is not null);
    }
}
