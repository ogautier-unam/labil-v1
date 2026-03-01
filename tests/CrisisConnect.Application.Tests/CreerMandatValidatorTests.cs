using CrisisConnect.Application.UseCases.Mandats.CreerMandat;
using CrisisConnect.Domain.Enums;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class CreerMandatValidatorTests
{
    private readonly CreerMandatValidator _validator = new();

    private static CreerMandatCommand CommandeValide() =>
        new(Guid.NewGuid(), Guid.NewGuid(), PorteeMandat.ToutesOperations,
            "Délégation logistique zone nord", false, DateTime.Today);

    [Fact]
    public void Valide_DonnéesComplètes_PasseValidation()
    {
        _validator.TestValidate(CommandeValide()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_MandantIdVide_EchecSurMandantId()
    {
        var cmd = CommandeValide() with { MandantId = Guid.Empty };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.MandantId);
    }

    [Fact]
    public void Invalide_MandataireEgalMandant_EchecSurMandataireId()
    {
        var id = Guid.NewGuid();
        var cmd = new CreerMandatCommand(id, id, PorteeMandat.LectureSeule, "Description", false, DateTime.Today);
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.MandataireId);
    }

    [Fact]
    public void Invalide_DescriptionVide_EchecSurDescription()
    {
        var cmd = CommandeValide() with { Description = "" };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Invalide_DateFinAvantDateDebut_EchecSurDateFin()
    {
        var cmd = new CreerMandatCommand(Guid.NewGuid(), Guid.NewGuid(), PorteeMandat.ToutesOperations,
            "Description", false, DateTime.Today, DateTime.Today.AddDays(-1));
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.DateFin);
    }
}
