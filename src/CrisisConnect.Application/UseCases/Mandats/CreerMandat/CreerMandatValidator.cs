using FluentValidation;

namespace CrisisConnect.Application.UseCases.Mandats.CreerMandat;

public class CreerMandatValidator : AbstractValidator<CreerMandatCommand>
{
    public CreerMandatValidator()
    {
        RuleFor(x => x.MandantId).NotEmpty();
        RuleFor(x => x.MandataireId).NotEmpty()
            .NotEqual(x => x.MandantId).WithMessage("Le mandataire ne peut pas être le mandant.");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.DateFin)
            .GreaterThan(x => x.DateDebut)
            .When(x => x.DateFin.HasValue);
    }
}
