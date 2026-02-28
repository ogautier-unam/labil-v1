using AutoMapper;
using CrisisConnect.Application.UseCases.Offres.GetOffreById;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetOffreByIdQueryHandlerTests
{
    private readonly IOffreRepository _offreRepo = Substitute.For<IOffreRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GetOffreByIdQueryHandler CréerHandler() => new(_offreRepo, _mapper);

    [Fact]
    public async Task GetOffreById_OffreExistante_RetourneDto()
    {
        // Arrange
        var offre = new Offre("Matériel médical", "Don de masques", Guid.NewGuid());
        _offreRepo.GetByIdAsync(offre.Id, Arg.Any<CancellationToken>())
            .Returns(offre);

        // Act
        var result = await CréerHandler().Handle(new GetOffreByIdQuery(offre.Id), CancellationToken.None);

        // Assert
        Assert.Equal(offre.Id, result.Id);
        Assert.Equal("Matériel médical", result.Titre);
    }

    [Fact]
    public async Task GetOffreById_IdInexistant_LèveNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _offreRepo.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Offre?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new GetOffreByIdQuery(id), CancellationToken.None));
    }
}
