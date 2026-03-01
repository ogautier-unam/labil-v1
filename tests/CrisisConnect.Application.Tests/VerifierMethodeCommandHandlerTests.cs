using CrisisConnect.Application.UseCases.MethodesIdentification.VerifierMethode;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class VerifierMethodeCommandHandlerTests
{
    private readonly IMethodeIdentificationRepository _methodeRepo = Substitute.For<IMethodeIdentificationRepository>();

    private VerifierMethodeCommandHandler CréerHandler() => new(_methodeRepo);

    [Fact]
    public async Task VerifierMethode_MethodeExiste_MarquéeVérifiéeEtPersistée()
    {
        var methode = new VerificationSMS(Guid.NewGuid(), "+32 4 56 78 90");
        Assert.False(methode.EstVerifiee);
        _methodeRepo.GetByIdAsync(methode.Id, Arg.Any<CancellationToken>()).Returns(methode);

        await CréerHandler().Handle(new VerifierMethodeCommand(methode.Id), CancellationToken.None);

        Assert.True(methode.EstVerifiee);
        await _methodeRepo.Received(1).UpdateAsync(methode, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task VerifierMethode_MethodeIntrouvable_LèveNotFoundException()
    {
        var id = Guid.NewGuid();
        _methodeRepo.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((MethodeIdentification?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new VerifierMethodeCommand(id), CancellationToken.None).AsTask());
    }
}
