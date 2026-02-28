using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class AttributionRoleTests
{
    [Fact]
    public void AttributionRole_SansDateFin_StatutActifEtSansRappel()
    {
        var role = new AttributionRole(Guid.NewGuid(), TypeRole.Contributeur, DateTime.UtcNow);

        Assert.Equal(StatutRole.Actif, role.Statut);
        Assert.Null(role.DateFin);
        Assert.Null(role.DateRappel);
        Assert.False(role.RappelEnvoye);
    }

    [Fact]
    public void AttributionRole_AvecDateFin_DateRappelCalculée()
    {
        var dateFin = DateTime.UtcNow.AddDays(30);

        var role = new AttributionRole(Guid.NewGuid(), TypeRole.AdminCatastrophe, DateTime.UtcNow, dateFin: dateFin);

        Assert.NotNull(role.DateRappel);
        // DateRappel = DateFin - 7 jours
        Assert.Equal(dateFin.AddDays(-7).Date, role.DateRappel!.Value.Date);
    }

    [Fact]
    public void Expirer_RoleActif_StatutExpiré()
    {
        var role = new AttributionRole(Guid.NewGuid(), TypeRole.Utilisateur, DateTime.UtcNow);

        role.Expirer();

        Assert.Equal(StatutRole.Expire, role.Statut);
    }

    [Fact]
    public void MarquerRappelEnvoye_RappelNonEnvoyé_RappelEnvoyéVrai()
    {
        var role = new AttributionRole(Guid.NewGuid(), TypeRole.Contributeur, DateTime.UtcNow,
            dateFin: DateTime.UtcNow.AddDays(14));

        role.MarquerRappelEnvoye();

        Assert.True(role.RappelEnvoye);
    }
}
