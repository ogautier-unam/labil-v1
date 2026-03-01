using FluentValidation;

namespace CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;

public class RevoquerMandatCommandValidator : AbstractValidator<RevoquerMandatCommand>
{
    public RevoquerMandatCommandValidator()
    {
        RuleFor(x => x.MandatId).NotEmpty();
    }
}
