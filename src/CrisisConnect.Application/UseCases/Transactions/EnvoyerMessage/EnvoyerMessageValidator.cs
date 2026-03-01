using FluentValidation;

namespace CrisisConnect.Application.UseCases.Transactions.EnvoyerMessage;

public class EnvoyerMessageValidator : AbstractValidator<EnvoyerMessageCommand>
{
    public EnvoyerMessageValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("L'identifiant de la transaction est requis.");

        RuleFor(x => x.ExpediteurId)
            .NotEmpty().WithMessage("L'identifiant de l'expéditeur est requis.");

        RuleFor(x => x.Contenu)
            .NotEmpty().WithMessage("Le contenu du message est requis.")
            .MaximumLength(2000).WithMessage("Le message ne peut pas dépasser 2000 caractères.");
    }
}
