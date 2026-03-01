using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandesQuota.AccepterIntentionDon;

public class AccepterIntentionDonCommandValidator : AbstractValidator<AccepterIntentionDonCommand>
{
    public AccepterIntentionDonCommandValidator()
    {
        RuleFor(x => x.DemandeQuotaId).NotEmpty();
        RuleFor(x => x.IntentionId).NotEmpty();
    }
}
