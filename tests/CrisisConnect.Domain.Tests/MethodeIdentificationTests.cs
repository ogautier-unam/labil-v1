using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class MethodeIdentificationTests
{
    [Fact]
    public void LoginPassword_NouvelleInstance_EstVérifiéeEtNiveauFaible()
    {
        var lp = new LoginPassword(Guid.NewGuid(), "alice@example.com", "hash");

        Assert.True(lp.EstVerifiee);
        Assert.Equal(NiveauFiabilite.Faible, lp.NiveauFiabilite);
    }

    [Fact]
    public void LoginPassword_EnregistrerConnexion_DerniereConnexionRenseignée()
    {
        var lp = new LoginPassword(Guid.NewGuid(), "alice@example.com", "hash");

        lp.EnregistrerConnexion();

        Assert.NotNull(lp.DerniereConnexion);
    }

    [Fact]
    public void Delegation_NouvelleInstance_EstImmédiatementVérifiée()
    {
        var del = new Delegation(Guid.NewGuid(), Guid.NewGuid());

        Assert.True(del.EstVerifiee);
        Assert.Equal(NiveauFiabilite.ExplicitementFaible, del.NiveauFiabilite);
    }

    [Fact]
    public void Parrainage_AjouterParrains_NonVérifié_TantQueQuorumNonAtteint()
    {
        var parrainage = new Parrainage(Guid.NewGuid(), nombreParrainsRequis: 3);

        parrainage.AjouterParrain(Guid.NewGuid());
        parrainage.AjouterParrain(Guid.NewGuid());

        Assert.Equal(2, parrainage.NombreParrainsActuels);
        Assert.False(parrainage.EstVerifiee);
    }

    [Fact]
    public void Parrainage_QuorumAtteint_MarquéVérifié()
    {
        var parrainage = new Parrainage(Guid.NewGuid(), nombreParrainsRequis: 2);

        parrainage.AjouterParrain(Guid.NewGuid());
        parrainage.AjouterParrain(Guid.NewGuid());

        Assert.True(parrainage.EstVerifiee);
        Assert.Equal(2, parrainage.NombreParrainsActuels);
    }

    [Fact]
    public void Parrainage_MêmeParrainDeuxFois_CompteUneSeuleFois()
    {
        var parrainage = new Parrainage(Guid.NewGuid(), nombreParrainsRequis: 3);
        var parrain = Guid.NewGuid();

        parrainage.AjouterParrain(parrain);
        parrainage.AjouterParrain(parrain);

        Assert.Equal(1, parrainage.NombreParrainsActuels);
    }

    [Fact]
    public void VerificationSMS_MarquerVerifiee_EstVérifiée()
    {
        var sms = new VerificationSMS(Guid.NewGuid(), "0612345678");

        Assert.False(sms.EstVerifiee);
        sms.MarquerVerifiee();
        Assert.True(sms.EstVerifiee);
    }

    [Fact]
    public void CarteIdentiteElectronique_NiveauTresHaute()
    {
        var cie = new CarteIdentiteElectronique(Guid.NewGuid(), "BE123456789", "Belgique");

        Assert.Equal(NiveauFiabilite.TresHaute, cie.NiveauFiabilite);
        Assert.Equal("BE123456789", cie.NumeroCarte);
    }
}
