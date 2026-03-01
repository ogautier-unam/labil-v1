using FluentValidation;

namespace CrisisConnect.Application.UseCases.Roles.RevoquerRole;

public class RevoquerRoleCommandValidator : AbstractValidator<RevoquerRoleCommand>
{
    public RevoquerRoleCommandValidator()
    {
        RuleFor(x => x.AttributionId).NotEmpty();
    }
}
