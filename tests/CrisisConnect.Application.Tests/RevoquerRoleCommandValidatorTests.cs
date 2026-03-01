using CrisisConnect.Application.UseCases.Roles.RevoquerRole;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class RevoquerRoleCommandValidatorTests
{
    private readonly RevoquerRoleCommandValidator _validator = new();

    [Fact]
    public void Valide_AttributionIdRempli_PasseValidation()
    {
        _validator.TestValidate(new RevoquerRoleCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_AttributionIdVide_EchecSurAttributionId()
    {
        _validator.TestValidate(new RevoquerRoleCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.AttributionId);
    }
}
