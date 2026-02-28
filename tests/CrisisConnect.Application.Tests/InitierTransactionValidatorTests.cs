using CrisisConnect.Application.UseCases.Transactions.InitierTransaction;

namespace CrisisConnect.Application.Tests;

public class InitierTransactionValidatorTests
{
    private readonly InitierTransactionValidator _validator = new();

    [Fact]
    public async Task Valide_DonnéesComplètes_PasseValidation()
    {
        var cmd = new InitierTransactionCommand(Guid.NewGuid(), Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Invalide_PropositionIdVide_EchecSurPropositionId()
    {
        var cmd = new InitierTransactionCommand(Guid.Empty, Guid.NewGuid());
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PropositionId");
    }

    [Fact]
    public async Task Invalide_InitiateurIdVide_EchecSurInitiateurId()
    {
        var cmd = new InitierTransactionCommand(Guid.NewGuid(), Guid.Empty);
        var result = await _validator.ValidateAsync(cmd);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InitiateurId");
    }
}
