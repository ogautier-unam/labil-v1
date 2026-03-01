using CrisisConnect.Application.UseCases.Auth.Logout;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class LogoutCommandValidatorTests
{
    private readonly LogoutCommandValidator _validator = new();

    [Fact]
    public void Valide_PersonneIdRempli_PasseValidation()
    {
        _validator.TestValidate(new LogoutCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_PersonneIdVide_EchecSurPersonneId()
    {
        _validator.TestValidate(new LogoutCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.PersonneId);
    }
}
