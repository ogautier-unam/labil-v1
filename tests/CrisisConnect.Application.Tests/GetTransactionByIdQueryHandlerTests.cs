using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Transactions.GetTransactionById;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetTransactionByIdQueryHandlerTests
{
    private readonly ITransactionRepository _txRepo = Substitute.For<ITransactionRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetTransactionByIdQueryHandler CréerHandler() => new(_txRepo, _mapper);

    [Fact]
    public async Task GetTransactionById_TransactionExistante_RetourneDto()
    {
        // Arrange
        var tx = new Transaction(Guid.NewGuid(), Guid.NewGuid());
        _txRepo.GetByIdAsync(tx.Id, Arg.Any<CancellationToken>())
            .Returns(tx);

        // Act
        var result = await CréerHandler().Handle(new GetTransactionByIdQuery(tx.Id), CancellationToken.None);

        // Assert
        Assert.Equal(tx.Id, result.Id);
    }

    [Fact]
    public async Task GetTransactionById_IdInexistant_LèveNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _txRepo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Transaction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new GetTransactionByIdQuery(id), CancellationToken.None).AsTask());
    }
}
