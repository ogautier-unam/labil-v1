using FluentValidation;

namespace CrisisConnect.Application.UseCases.DemandesQuota.RefuserIntentionDon;

public class RefuserIntentionDonCommandValidator : AbstractValidator<RefuserIntentionDonCommand>
{
    public RefuserIntentionDonCommandValidator()
    {
        RuleFor(x => x.DemandeQuotaId).NotEmpty();
        RuleFor(x => x.IntentionId).NotEmpty();
    }
}
