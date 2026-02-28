using CrisisConnect.Domain.Entities;
using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Tests;

public class MandatTests
{
    [Fact]
    public void NouvelleInstance_MandantMandatairePortee_SetCorrectement()
    {
        var mandantId = Guid.NewGuid();
        var mandataireId = Guid.NewGuid();
        var debut = DateTime.UtcNow;

        var m = new Mandat(mandantId, mandataireId, PorteeMandat.ToutesOperations, "Délégation totale", true, debut);

        Assert.Equal(mandantId, m.MandantId);
        Assert.Equal(mandataireId, m.MandataireId);
        Assert.Equal(PorteeMandat.ToutesOperations, m.Portee);
        Assert.True(m.EstPublic);
    }

    [Fact]
    public void NouvelleInstance_DateFin_Optionnelle()
    {
        var m = new Mandat(Guid.NewGuid(), Guid.NewGuid(), PorteeMandat.LectureSeule, "Lecture", false, DateTime.UtcNow);

        Assert.Null(m.DateFin);
    }

    [Fact]
    public void NouvelleInstance_AvecDateFin_DateFinSetée()
    {
        var fin = DateTime.UtcNow.AddDays(30);
        var m = new Mandat(Guid.NewGuid(), Guid.NewGuid(), PorteeMandat.CategorieSpecifique, "Catégorie", false, DateTime.UtcNow, fin);

        Assert.Equal(fin, m.DateFin);
    }
}
