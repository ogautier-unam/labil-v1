using AutoMapper;
using CrisisConnect.Application.UseCases.Demandes.GetDemandeById;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetDemandeByIdQueryHandlerTests
{
    private readonly IDemandeRepository _demandeRepo = Substitute.For<IDemandeRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GetDemandeByIdQueryHandler CréerHandler() => new(_demandeRepo, _mapper);

    [Fact]
    public async Task GetDemandeById_DemandeExistante_RetourneDto()
    {
        // Arrange
        var demande = new Demande("Abri d'urgence", "Besoin de tentes", Guid.NewGuid(),
            urgence: NiveauUrgence.Critique);
        _demandeRepo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>())
            .Returns(demande);

        // Act
        var result = await CréerHandler().Handle(new GetDemandeByIdQuery(demande.Id), CancellationToken.None);

        // Assert
        Assert.Equal(demande.Id, result.Id);
        Assert.Equal(NiveauUrgence.Critique, result.Urgence);
    }

    [Fact]
    public async Task GetDemandeById_IdInexistant_LèveNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _demandeRepo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Demande?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new GetDemandeByIdQuery(id), CancellationToken.None));
    }
}
