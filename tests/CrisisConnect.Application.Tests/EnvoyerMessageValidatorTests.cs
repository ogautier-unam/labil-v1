using CrisisConnect.Application.UseCases.Transactions.EnvoyerMessage;
using FluentValidation.TestHelper;

namespace CrisisConnect.Application.Tests;

public class EnvoyerMessageValidatorTests
{
    private readonly EnvoyerMessageValidator _validator = new();

    private static EnvoyerMessageCommand CommandeValide() =>
        new(Guid.NewGuid(), Guid.NewGuid(), "Bonjour, comment puis-je aider ?");

    [Fact]
    public void Valide_DonnéesComplètes_PasseValidation()
    {
        _validator.TestValidate(CommandeValide()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Invalide_TransactionIdVide_EchecSurTransactionId()
    {
        var cmd = CommandeValide() with { TransactionId = Guid.Empty };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.TransactionId);
    }

    [Fact]
    public void Invalide_ExpediteurIdVide_EchecSurExpediteurId()
    {
        var cmd = CommandeValide() with { ExpediteurId = Guid.Empty };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.ExpediteurId);
    }

    [Fact]
    public void Invalide_ContenuVide_EchecSurContenu()
    {
        var cmd = CommandeValide() with { Contenu = "" };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Contenu);
    }

    [Fact]
    public void Invalide_ContenuTropLong_EchecSurContenu()
    {
        var cmd = CommandeValide() with { Contenu = new string('x', 2001) };
        _validator.TestValidate(cmd).ShouldHaveValidationErrorFor(x => x.Contenu);
    }
}
