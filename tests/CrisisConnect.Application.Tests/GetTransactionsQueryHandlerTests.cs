using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Transactions.GetTransactions;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetTransactionsQueryHandlerTests
{
    private readonly ITransactionRepository _txRepo = Substitute.For<ITransactionRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetTransactionsQueryHandler CréerHandler() => new(_txRepo, _mapper);

    [Fact]
    public async Task GetTransactions_DeuxTransactions_RetourneDeuxDtos()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new(Guid.NewGuid(), Guid.NewGuid()),
            new(Guid.NewGuid(), Guid.NewGuid())
        };
        _txRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(transactions.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetTransactionsQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTransactions_AucuneTransaction_RetourneListeVide()
    {
        _txRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Transaction>());

        var result = await CréerHandler().Handle(new GetTransactionsQuery(), CancellationToken.None);

        Assert.Empty(result);
    }
}
