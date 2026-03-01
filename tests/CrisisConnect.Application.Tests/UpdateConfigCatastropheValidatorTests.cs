using CrisisConnect.Application.UseCases.ConfigCatastrophe.UpdateConfigCatastrophe;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class UpdateConfigCatastropheValidatorTests
{
    private readonly UpdateConfigCatastropheValidator _validator = new();

    [Fact]
    public void Valide_DonnéesComplètes_PasseValidation()
    {
        var cmd = new UpdateConfigCatastropheCommand(Guid.NewGuid(), 30, 7, true);
        _validator.TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_IdVide_EchecSurId()
    {
        var cmd = new UpdateConfigCatastropheCommand(Guid.Empty, 30, 7, true);
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Invalide_DelaiArchivageZero_EchecSurDelai()
    {
        var cmd = new UpdateConfigCatastropheCommand(Guid.NewGuid(), 0, 7, true);
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.DelaiArchivageJours);
    }

    [Fact]
    public void Invalide_DelaiRappelSuperieurDelaiArchivage_EchecSurRappel()
    {
        // DelaiRappelAvantArchivage (20) > DelaiArchivageJours (10) → invalide
        var cmd = new UpdateConfigCatastropheCommand(Guid.NewGuid(), 10, 20, true);
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.DelaiRappelAvantArchivage);
    }
}
