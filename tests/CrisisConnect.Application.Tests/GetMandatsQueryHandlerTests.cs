using AutoMapper;
using CrisisConnect.Application.UseCases.Mandats.GetMandats;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetMandatsQueryHandlerTests
{
    private readonly IMandatRepository _mandatRepo = Substitute.For<IMandatRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GetMandatsQueryHandler CréerHandler() => new(_mandatRepo, _mapper);

    [Fact]
    public async Task GetMandats_ActeurAvecMandats_RetourneListe()
    {
        // Arrange
        var mandantId = Guid.NewGuid();
        var mandats = new List<Mandat>
        {
            new(mandantId, Guid.NewGuid(), PorteeMandat.LectureSeule, "Description A", false, DateTime.Today),
            new(mandantId, Guid.NewGuid(), PorteeMandat.ToutesOperations, "Description B", true, DateTime.Today)
        };
        _mandatRepo.GetByMandantAsync(mandantId, Arg.Any<CancellationToken>())
            .Returns(mandats.AsReadOnly());

        // Act
        var result = await CréerHandler().Handle(new GetMandatsQuery(mandantId), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.Description == "Description A");
        Assert.Contains(result, m => m.EstPublic);
    }

    [Fact]
    public async Task GetMandats_AucunMandat_RetourneListeVide()
    {
        // Arrange
        var mandantId = Guid.NewGuid();
        _mandatRepo.GetByMandantAsync(mandantId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Mandat>());

        // Act
        var result = await CréerHandler().Handle(new GetMandatsQuery(mandantId), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
