using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;
using CrisisConnect.Domain.Exceptions;

namespace CrisisConnect.Domain.Tests;

public class PropositionAvecValidationTests
{
    private static PropositionAvecValidation Créer() =>
        new("Aide psychologique", "Soutien post-crise", Guid.NewGuid(), "Validation tiers de confiance requise");

    [Fact]
    public void NouvelleInstance_StatutInitial_EstArchivée_EnAttente()
    {
        var pav = Créer();

        // Non visible tant que non validée
        Assert.Equal(StatutProposition.Archivee, pav.Statut);
        Assert.Equal(StatutValidation.EnAttente, pav.StatutValidation);
        Assert.Null(pav.ValideurEntiteId);
    }

    [Fact]
    public void Valider_EnAttente_MarqueStatutActifEtValideurSet()
    {
        var pav = Créer();
        var valideurId = Guid.NewGuid();

        pav.Valider(valideurId);

        Assert.Equal(StatutValidation.Validee, pav.StatutValidation);
        Assert.Equal(StatutProposition.Active, pav.Statut);
        Assert.Equal(valideurId, pav.ValideurEntiteId);
    }

    [Fact]
    public void Valider_DéjàValidée_LèveDomainException()
    {
        var pav = Créer();
        pav.Valider(Guid.NewGuid());

        Assert.Throws<DomainException>(() => pav.Valider(Guid.NewGuid()));
    }

    [Fact]
    public void RefuserValidation_MarqueRefuséeEtValideurSet()
    {
        var pav = Créer();
        var valideurId = Guid.NewGuid();

        pav.RefuserValidation(valideurId);

        Assert.Equal(StatutValidation.Refusee, pav.StatutValidation);
        Assert.Equal(valideurId, pav.ValideurEntiteId);
    }
}
