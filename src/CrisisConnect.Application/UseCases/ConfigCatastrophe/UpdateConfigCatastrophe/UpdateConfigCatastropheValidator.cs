using FluentValidation;

namespace CrisisConnect.Application.UseCases.ConfigCatastrophe.UpdateConfigCatastrophe;

public class UpdateConfigCatastropheValidator : AbstractValidator<UpdateConfigCatastropheCommand>
{
    public UpdateConfigCatastropheValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DelaiArchivageJours).GreaterThan(0);
        RuleFor(x => x.DelaiRappelAvantArchivage).GreaterThan(0)
            .LessThan(x => x.DelaiArchivageJours);
    }
}
