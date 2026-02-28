using AutoMapper;
using CrisisConnect.Application.UseCases.Propositions.GetPropositionById;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetPropositionByIdQueryHandlerTests
{
    private readonly IPropositionRepository _propRepo = Substitute.For<IPropositionRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GetPropositionByIdQueryHandler CréerHandler() => new(_propRepo, _mapper);

    [Fact]
    public async Task GetPropositionById_OffreExistante_RetourneDto()
    {
        // Arrange
        var offre = new Offre("Offre titre", "Description", Guid.NewGuid());
        _propRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>())
            .Returns(offre);

        // Act
        var result = await CréerHandler().Handle(new GetPropositionByIdQuery(offre.Id), CancellationToken.None);

        // Assert
        Assert.Equal(offre.Id, result.Id);
        Assert.Equal("Offre titre", result.Titre);
    }

    [Fact]
    public async Task GetPropositionById_IdInexistant_LèveNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _propRepo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Proposition?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new GetPropositionByIdQuery(id), CancellationToken.None));
    }
}
