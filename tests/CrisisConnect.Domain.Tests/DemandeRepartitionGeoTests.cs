using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class DemandeRepartitionGeoTests
{
    [Fact]
    public void NouvelleInstance_NombreRessourcesEtMission_Setés()
    {
        var drg = new DemandeRepartitionGeo(
            "Évacuation boues",
            "Nettoyage secteur Rochebranle",
            Guid.NewGuid(),
            nombreRessourcesRequises: 50,
            descriptionMission: "Bénévoles nettoyage et évacuation");

        Assert.Equal(50, drg.NombreRessourcesRequises);
        Assert.Equal("Bénévoles nettoyage et évacuation", drg.DescriptionMission);
        Assert.Equal(StatutProposition.Active, drg.Statut);
    }

    [Fact]
    public void NouvelleInstance_HériteDemandeStatutActif()
    {
        var drg = new DemandeRepartitionGeo(
            "Test", "Description", Guid.NewGuid(), 10, "Mission test");

        Assert.Equal(StatutProposition.Active, drg.Statut);
        Assert.Equal(NiveauUrgence.Moyen, drg.Urgence);
    }
}
