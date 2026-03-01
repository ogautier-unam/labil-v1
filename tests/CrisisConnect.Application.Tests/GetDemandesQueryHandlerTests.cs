using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Demandes.GetDemandes;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetDemandesQueryHandlerTests
{
    private readonly IDemandeRepository _demandeRepo = Substitute.For<IDemandeRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetDemandesQueryHandler CréerHandler() => new(_demandeRepo, _mapper);

    [Fact]
    public async Task GetDemandes_DeuxDemandes_RetourneDeuxDtos()
    {
        // Arrange
        var demandes = new List<Demande>
        {
            new("Eau potable", "Besoin eau", Guid.NewGuid(), urgence: NiveauUrgence.Critique),
            new("Couvertures", "Besoin couvertures", Guid.NewGuid(), urgence: NiveauUrgence.Eleve)
        };
        _demandeRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(demandes.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetDemandesQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Urgence == NiveauUrgence.Critique);
        Assert.Contains(result, d => d.Urgence == NiveauUrgence.Eleve);
    }

    [Fact]
    public async Task GetDemandes_AucuneDemande_RetourneListeVide()
    {
        _demandeRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Demande>());

        var result = await CréerHandler().Handle(new GetDemandesQuery(), CancellationToken.None);

        Assert.Empty(result);
    }
}
