using CrisisConnect.Application.UseCases.DemandesQuota.AccepterIntentionDon;
using CrisisConnect.Application.UseCases.DemandesQuota.ConfirmerIntentionDon;
using CrisisConnect.Application.UseCases.DemandesQuota.CreateDemandeQuota;
using CrisisConnect.Application.UseCases.DemandesQuota.GetDemandeQuotaById;
using CrisisConnect.Application.UseCases.DemandesQuota.GetDemandesQuota;
using CrisisConnect.Application.UseCases.DemandesQuota.RefuserIntentionDon;
using CrisisConnect.Application.UseCases.DemandesQuota.SoumettreIntentionDon;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class DemandeQuotaCommandHandlerTests
{
    private readonly IDemandeQuotaRepository _repo = Substitute.For<IDemandeQuotaRepository>();

    private static DemandeQuota CréerDemande(int capaciteMax = 100) =>
        new("Titre", "Description", Guid.NewGuid(), capaciteMax, "m3");

    // ── CreateDemandeQuota ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateDemandeQuota_Valide_CrééeEtRetournée()
    {
        var cmd = new CreateDemandeQuotaCommand("Toits pour sinistrés", "Besoin de bâches",
            Guid.NewGuid(), 50, "unités");

        var result = await new CreateDemandeQuotaCommandHandler(_repo).Handle(cmd, CancellationToken.None);

        Assert.Equal("Toits pour sinistrés", result.Titre);
        Assert.Equal(50, result.CapaciteMax);
        Assert.Equal("unités", result.UniteCapacite);
        Assert.Empty(result.Intentions);
        await _repo.Received(1).AddAsync(Arg.Any<DemandeQuota>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateDemandeQuota_AvecAdresseEtDateLimit_ChampsPersistés()
    {
        var dateLimit = DateTime.UtcNow.AddDays(7);
        var cmd = new CreateDemandeQuotaCommand("Aide alimentaire", "Desc",
            Guid.NewGuid(), 200, "kg", "12 rue de la Paix", dateLimit);

        var result = await new CreateDemandeQuotaCommandHandler(_repo).Handle(cmd, CancellationToken.None);

        Assert.Equal("12 rue de la Paix", result.AdresseDepot);
        Assert.Equal(dateLimit, result.DateLimit);
    }

    // ── SoumettreIntentionDon ─────────────────────────────────────────────────

    [Fact]
    public async Task SoumettreIntentionDon_DemandeExistante_IntentionRetournée()
    {
        var demande = CréerDemande();
        _repo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var cmd = new SoumettreIntentionDonCommand(demande.Id, Guid.NewGuid(), 10, "m3", "Don de bâches");
        var result = await new SoumettreIntentionDonCommandHandler(_repo).Handle(cmd, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(10, result.Quantite);
        Assert.Equal(StatutIntention.EnAttente, result.Statut);
        await _repo.Received(1).UpdateAsync(demande, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SoumettreIntentionDon_DemandeInexistante_LèveNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((DemandeQuota?)null);

        var cmd = new SoumettreIntentionDonCommand(Guid.NewGuid(), Guid.NewGuid(), 5, "kg", "Desc");

        await Assert.ThrowsAsync<NotFoundException>(
            () => new SoumettreIntentionDonCommandHandler(_repo).Handle(cmd, CancellationToken.None).AsTask());
    }

    [Fact]
    public async Task SoumettreIntentionDon_CapacitéDépassée_LèveDomainException()
    {
        var demande = CréerDemande(capaciteMax: 5);
        _repo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var cmd = new SoumettreIntentionDonCommand(demande.Id, Guid.NewGuid(), 10, "m3", "Trop grand");

        await Assert.ThrowsAsync<DomainException>(
            () => new SoumettreIntentionDonCommandHandler(_repo).Handle(cmd, CancellationToken.None).AsTask());
        await _repo.DidNotReceive().UpdateAsync(Arg.Any<DemandeQuota>(), Arg.Any<CancellationToken>());
    }

    // ── AccepterIntentionDon ──────────────────────────────────────────────────

    [Fact]
    public async Task AccepterIntentionDon_IntentionEnAttente_AcceptéeEtCapacitéMiseAJour()
    {
        var demande = CréerDemande();
        var intention = demande.AjouterIntention(Guid.NewGuid(), 20, "m3", "Don volontaire");
        _repo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var cmd = new AccepterIntentionDonCommand(demande.Id, intention.Id);
        var result = await new AccepterIntentionDonCommandHandler(_repo).Handle(cmd, CancellationToken.None);

        Assert.Equal(20, result.CapaciteUtilisee);
        Assert.Equal(StatutIntention.Acceptee,
            result.Intentions.Single(i => i.Id == intention.Id).Statut);
        await _repo.Received(1).UpdateAsync(demande, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AccepterIntentionDon_DemandeInexistante_LèveNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((DemandeQuota?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => new AccepterIntentionDonCommandHandler(_repo)
                .Handle(new AccepterIntentionDonCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None)
                .AsTask());
    }

    // ── RefuserIntentionDon ───────────────────────────────────────────────────

    [Fact]
    public async Task RefuserIntentionDon_IntentionEnAttente_RefuséeEtCapacitéInchangée()
    {
        var demande = CréerDemande();
        var intention = demande.AjouterIntention(Guid.NewGuid(), 15, "m3", "Don");
        _repo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var cmd = new RefuserIntentionDonCommand(demande.Id, intention.Id);
        var result = await new RefuserIntentionDonCommandHandler(_repo).Handle(cmd, CancellationToken.None);

        Assert.Equal(0, result.CapaciteUtilisee);
        Assert.Equal(StatutIntention.Refusee,
            result.Intentions.Single(i => i.Id == intention.Id).Statut);
        await _repo.Received(1).UpdateAsync(demande, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RefuserIntentionDon_DemandeInexistante_LèveNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((DemandeQuota?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => new RefuserIntentionDonCommandHandler(_repo)
                .Handle(new RefuserIntentionDonCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None)
                .AsTask());
    }

    // ── ConfirmerIntentionDon ─────────────────────────────────────────────────

    [Fact]
    public async Task ConfirmerIntentionDon_IntentionAcceptée_Confirmée()
    {
        var demande = CréerDemande();
        var intention = demande.AjouterIntention(Guid.NewGuid(), 30, "m3", "Don");
        demande.ValiderIntention(intention.Id); // passe de EnAttente → Acceptee
        _repo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var cmd = new ConfirmerIntentionDonCommand(demande.Id, intention.Id);
        var result = await new ConfirmerIntentionDonCommandHandler(_repo).Handle(cmd, CancellationToken.None);

        Assert.Equal(StatutIntention.Confirmee,
            result.Intentions.Single(i => i.Id == intention.Id).Statut);
        await _repo.Received(1).UpdateAsync(demande, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ConfirmerIntentionDon_DemandeInexistante_LèveNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((DemandeQuota?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => new ConfirmerIntentionDonCommandHandler(_repo)
                .Handle(new ConfirmerIntentionDonCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None)
                .AsTask());
    }

    [Fact]
    public async Task ConfirmerIntentionDon_IntentionInexistante_LèveNotFoundException()
    {
        var demande = CréerDemande();
        _repo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        await Assert.ThrowsAsync<NotFoundException>(
            () => new ConfirmerIntentionDonCommandHandler(_repo)
                .Handle(new ConfirmerIntentionDonCommand(demande.Id, Guid.NewGuid()), CancellationToken.None)
                .AsTask());
    }

    // ── GetDemandesQuota ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetDemandesQuota_DeuxDemandes_ListeComplèteRetournée()
    {
        var d1 = CréerDemande();
        var d2 = CréerDemande(200);
        _repo.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<DemandeQuota> { d1, d2 });

        var result = await new GetDemandesQuotaQueryHandler(_repo)
            .Handle(new GetDemandesQuotaQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetDemandesQuota_ListeVide_ListeVideRetournée()
    {
        _repo.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<DemandeQuota>());

        var result = await new GetDemandesQuotaQueryHandler(_repo)
            .Handle(new GetDemandesQuotaQuery(), CancellationToken.None);

        Assert.Empty(result);
    }

    // ── GetDemandeQuotaById ───────────────────────────────────────────────────

    [Fact]
    public async Task GetDemandeQuotaById_Existante_DtoRetourné()
    {
        var demande = CréerDemande();
        _repo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var result = await new GetDemandeQuotaByIdQueryHandler(_repo)
            .Handle(new GetDemandeQuotaByIdQuery(demande.Id), CancellationToken.None);

        Assert.Equal(demande.Id, result.Id);
        Assert.Equal("Titre", result.Titre);
    }

    [Fact]
    public async Task GetDemandeQuotaById_Inexistante_LèveNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((DemandeQuota?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => new GetDemandeQuotaByIdQueryHandler(_repo)
                .Handle(new GetDemandeQuotaByIdQuery(Guid.NewGuid()), CancellationToken.None)
                .AsTask());
    }
}
