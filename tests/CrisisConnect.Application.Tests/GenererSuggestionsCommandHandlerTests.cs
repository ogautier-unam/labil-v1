using AutoMapper;
using CrisisConnect.Application.UseCases.Suggestions.GenererSuggestions;
using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;
using CrisisConnect.Domain.Interfaces.Repositories;
using NSubstitute;

namespace CrisisConnect.Application.Tests;

public class GenererSuggestionsCommandHandlerTests
{
    private readonly IDemandeRepository _demandeRepo = Substitute.For<IDemandeRepository>();
    private readonly IOffreRepository _offreRepo = Substitute.For<IOffreRepository>();
    private readonly ISuggestionAppariementRepository _suggestionRepo = Substitute.For<ISuggestionAppariementRepository>();
    private readonly IMapper _mapper = AutoMapperFixture.Créer();

    private GenererSuggestionsCommandHandler CréerHandler() =>
        new(_demandeRepo, _offreRepo, _suggestionRepo, _mapper);

    [Fact]
    public async Task Generer_DemandeIntrouvable_LèveNotFoundException()
    {
        var demandeId = Guid.NewGuid();
        _demandeRepo.GetByIdAsync(demandeId, Arg.Any<CancellationToken>()).Returns((Demande?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            CréerHandler().Handle(new GenererSuggestionsCommand(demandeId), CancellationToken.None));
    }

    [Fact]
    public async Task Generer_DemandeNonActive_RetourneListeVide()
    {
        var demande = new Demande("Aide médicale", "Besoin de masques", Guid.NewGuid(),
            urgence: NiveauUrgence.Moyen);
        demande.Archiver(); // → Archivée, donc non active
        _demandeRepo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);

        var result = await CréerHandler().Handle(new GenererSuggestionsCommand(demande.Id), CancellationToken.None);

        Assert.Empty(result);
        await _offreRepo.DidNotReceive().GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Generer_OffreAvecMotsCommunsEtUrgenceCritique_SuggestionCréée()
    {
        var demande = new Demande("aide médicale masques", "besoin de masques chirurgicaux", Guid.NewGuid(),
            urgence: NiveauUrgence.Critique);
        var offre = new Offre("aide médicale disponible masques", "masques chirurgicaux disponibles", Guid.NewGuid(), livraisonIncluse: true);

        _demandeRepo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);
        _offreRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns((IReadOnlyList<Offre>)new List<Offre> { offre });
        _suggestionRepo.GetByDemandeAsync(demande.Id, Arg.Any<CancellationToken>())
            .Returns((IReadOnlyList<SuggestionAppariement>)new List<SuggestionAppariement>());

        var result = await CréerHandler().Handle(new GenererSuggestionsCommand(demande.Id), CancellationToken.None);

        Assert.NotEmpty(result);
        await _suggestionRepo.Received(1).AddAsync(Arg.Any<SuggestionAppariement>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Generer_OffreDéjàTraitée_SuggestionSkippée()
    {
        var demande = new Demande("aide médicale masques", "besoin de masques", Guid.NewGuid(),
            urgence: NiveauUrgence.Critique);
        var offre = new Offre("aide médicale disponible masques", "masques disponibles", Guid.NewGuid());

        // suggestion déjà existante pour cette offre
        var existante = new SuggestionAppariement(offre.Id, demande.Id, 0.85, "déjà générée");

        _demandeRepo.GetByIdAsync(demande.Id, Arg.Any<CancellationToken>()).Returns(demande);
        _offreRepo.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns((IReadOnlyList<Offre>)new List<Offre> { offre });
        _suggestionRepo.GetByDemandeAsync(demande.Id, Arg.Any<CancellationToken>())
            .Returns((IReadOnlyList<SuggestionAppariement>)new List<SuggestionAppariement> { existante });

        var result = await CréerHandler().Handle(new GenererSuggestionsCommand(demande.Id), CancellationToken.None);

        Assert.Empty(result);
        await _suggestionRepo.DidNotReceive().AddAsync(Arg.Any<SuggestionAppariement>(), Arg.Any<CancellationToken>());
    }
}
