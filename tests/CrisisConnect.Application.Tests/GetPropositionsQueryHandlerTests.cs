using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.Propositions.GetPropositions;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetPropositionsQueryHandlerTests
{
    private readonly IPropositionRepository _propRepo = Substitute.For<IPropositionRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetPropositionsQueryHandler CréerHandler() => new(_propRepo, _mapper);

    [Fact]
    public async Task GetPropositions_DeuxPropositions_RetourneDeuxDtos()
    {
        // Arrange
        var propositions = new List<Proposition>
        {
            new Offre("Offre A", "Desc A", Guid.NewGuid()),
            new Demande("Demande B", "Desc B", Guid.NewGuid())
        };
        _propRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(propositions.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetPropositionsQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetPropositions_AucuneProposition_RetourneListeVide()
    {
        _propRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Proposition>());

        var result = await CréerHandler().Handle(new GetPropositionsQuery(), CancellationToken.None);

        Assert.Empty(result);
    }
}
