using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Tests;

public class ConfigCatastropheTests
{
    private static ConfigCatastrophe CréerConfig() =>
        new("Inondation", "Crise inondation", "PACA", "France", 30, 7);

    [Fact]
    public void ConfigCatastrophe_NouvelleInstance_EstActiveEtPropriétésCorrectement()
    {
        var config = CréerConfig();

        Assert.True(config.EstActive);
        Assert.Equal("Inondation", config.Nom);
        Assert.Equal("PACA", config.ZoneGeographique);
        Assert.Equal(30, config.DelaiArchivageJours);
        Assert.Equal(7, config.DelaiRappelAvantArchivage);
    }

    [Fact]
    public void Desactiver_ConfigActive_EstActiveFaux()
    {
        var config = CréerConfig();

        config.Desactiver();

        Assert.False(config.EstActive);
    }

    [Fact]
    public void Activer_ConfigInactive_EstActiveVrai()
    {
        var config = CréerConfig();
        config.Desactiver();

        config.Activer();

        Assert.True(config.EstActive);
    }

    [Fact]
    public void MettreAJourParametres_NouveauxDélais_Persistés()
    {
        var config = CréerConfig();

        config.MettreAJourParametres(60, 14);

        Assert.Equal(60, config.DelaiArchivageJours);
        Assert.Equal(14, config.DelaiRappelAvantArchivage);
    }
}
