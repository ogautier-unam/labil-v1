using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandesQuota.ConfirmerIntentionDon;

public class ConfirmerIntentionDonCommandValidator : AbstractValidator<ConfirmerIntentionDonCommand>
{
    public ConfirmerIntentionDonCommandValidator()
    {
        RuleFor(x => x.DemandeQuotaId).NotEmpty();
        RuleFor(x => x.IntentionId).NotEmpty();
    }
}
