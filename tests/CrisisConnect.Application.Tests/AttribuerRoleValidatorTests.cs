using CrisisConnect.Application.UseCases.Roles.AttribuerRole;
using CrisisConnect.Domain.Enums;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class AttribuerRoleValidatorTests
{
    private readonly AttribuerRoleValidator _validator = new();

    [Fact]
    public void Valide_SansDates_PasseValidation()
    {
        var cmd = new AttribuerRoleCommand(Guid.NewGuid(), TypeRole.Contributeur, DateTime.Today);
        _validator.TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Valide_AvecDateFinPostérieure_PasseValidation()
    {
        var cmd = new AttribuerRoleCommand(Guid.NewGuid(), TypeRole.AdminCatastrophe,
            DateTime.Today, DateTime.Today.AddMonths(6));
        _validator.TestValidate(cmd).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_ActeurIdVide_EchecSurActeurId()
    {
        var cmd = new AttribuerRoleCommand(Guid.Empty, TypeRole.Contributeur, DateTime.Today);
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.ActeurId);
    }

    [Fact]
    public void Invalide_DateFinAvantDateDebut_EchecSurDateFin()
    {
        var cmd = new AttribuerRoleCommand(Guid.NewGuid(), TypeRole.Contributeur,
            DateTime.Today, DateTime.Today.AddDays(-1));
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.DateFin);
    }
}
