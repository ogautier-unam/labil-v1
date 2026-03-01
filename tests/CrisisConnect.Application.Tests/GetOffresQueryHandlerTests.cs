using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Offres.GetOffres;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetOffresQueryHandlerTests
{
    private readonly IOffreRepository _offreRepo = Substitute.For<IOffreRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetOffresQueryHandler CréerHandler() => new(_offreRepo, _mapper);

    [Fact]
    public async Task GetOffres_DeuxOffres_RetourneDeuxDtos()
    {
        // Arrange
        var offres = new List<Offre>
        {
            new("Offre A", "Desc A", Guid.NewGuid()),
            new("Offre B", "Desc B", Guid.NewGuid(), livraisonIncluse: true)
        };
        _offreRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(offres.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetOffresQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Titre == "Offre A");
        Assert.Contains(result, d => d.Titre == "Offre B" && d.LivraisonIncluse);
    }

    [Fact]
    public async Task GetOffres_AucuneOffre_RetourneListeVide()
    {
        _offreRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Offre>());

        var result = await CréerHandler().Handle(new GetOffresQuery(), CancellationToken.None);

        Assert.Empty(result);
    }
}
