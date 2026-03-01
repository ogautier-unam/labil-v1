using AutoMapper;
using CrisisConnect.Application.UseCases.Entites.CreateEntite;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Interfaces.Repositories;
using CrisisConnect.Domain.Interfaces.Services;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class CreateEntiteCommandHandlerTests
{
    private readonly IEntiteRepository _entiteRepo = Substitute.For<IEntiteRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private CreateEntiteCommandHandler CréerHandler() => new(_entiteRepo, _passwordHasher, _mapper);

    [Fact]
    public async Task CreateEntite_MotDePasseHashé_EntitéCréeEtRetournéeDto()
    {
        _passwordHasher.Hacher("motdepasse").Returns("hash_mocké");
        var responsableId = Guid.NewGuid();
        var command = new CreateEntiteCommand("org@example.com", "motdepasse",
            "Croix-Rouge", "Organisation humanitaire", "+32 2 345 67 89", responsableId);

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Croix-Rouge", result.Nom);
        Assert.Equal(responsableId, result.ResponsableId);
        Assert.True(result.EstActive);
        _passwordHasher.Received(1).Hacher("motdepasse");
        await _entiteRepo.Received(1).AddAsync(Arg.Any<Entite>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateEntite_EmailEnregistré_EmailRetournéDansDto()
    {
        _passwordHasher.Hacher(Arg.Any<string>()).Returns("hash");
        var command = new CreateEntiteCommand("contact@ong.be", "motdepasse",
            "ONG Test", "Description", "Tél: 0499...", Guid.NewGuid());

        var result = await CréerHandler().Handle(command, CancellationToken.None);

        Assert.Equal("contact@ong.be", result.Email);
    }
}
