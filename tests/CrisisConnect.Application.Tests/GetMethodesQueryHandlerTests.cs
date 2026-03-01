using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.UseCases.MethodesIdentification.GetMethodes;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GetMethodesQueryHandlerTests
{
    private readonly IMethodeIdentificationRepository _methodeRepo = Substitute.For<IMethodeIdentificationRepository>();
    private readonly AppMapper _mapper = AutoMapperFixture.Créer();

    private GetMethodesQueryHandler CréerHandler() => new(_methodeRepo, _mapper);

    [Fact]
    public async Task GetMethodes_PersonneAvecMethodes_ListeRetournée()
    {
        var personneId = Guid.NewGuid();
        var methodes = new List<MethodeIdentification>
        {
            new LoginPassword(personneId, "alice", "hash"),
            new VerificationSMS(personneId, "+32 4 xx xx xx xx")
        };
        _methodeRepo.GetByPersonneAsync(personneId, Arg.Any<CancellationToken>())
            .Returns((IReadOnlyList<MethodeIdentification>)methodes);

        var result = await CréerHandler().Handle(new GetMethodesQuery(personneId), CancellationToken.None);

        Assert.Equal(2, result.Count);
        await _methodeRepo.Received(1).GetByPersonneAsync(personneId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetMethodes_PersonneSansMethodes_ListeVide()
    {
        var personneId = Guid.NewGuid();
        _methodeRepo.GetByPersonneAsync(personneId, Arg.Any<CancellationToken>())
            .Returns((IReadOnlyList<MethodeIdentification>)new List<MethodeIdentification>());

        var result = await CréerHandler().Handle(new GetMethodesQuery(personneId), CancellationToken.None);

        Assert.Empty(result);
    }
}
