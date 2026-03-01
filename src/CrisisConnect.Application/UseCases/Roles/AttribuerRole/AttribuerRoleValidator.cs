using FluentValidation;

namespace CrisisConnect.Application.UseCases.Roles.AttribuerRole;

public class AttribuerRoleValidator : AbstractValidator<AttribuerRoleCommand>
{
    public AttribuerRoleValidator()
    {
        RuleFor(x => x.ActeurId).NotEmpty();
        RuleFor(x => x.DateFin)
            .GreaterThan(x => x.DateDebut)
            .When(x => x.DateFin.HasValue);
    }
}
