using CrisisConnect.Application.UseCases.Mandats.RevoquerMandat;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class RevoquerMandatCommandValidatorTests
{
    private readonly RevoquerMandatCommandValidator _validator = new();

    [Fact]
    public void Valide_MandatIdRempli_PasseValidation()
    {
        _validator.TestValidate(new RevoquerMandatCommand(Guid.NewGuid())).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_MandatIdVide_EchecSurMandatId()
    {
        _validator.TestValidate(new RevoquerMandatCommand(Guid.Empty)).ShouldHaveValidationErrorFor(x => x.MandatId);
    }
}
