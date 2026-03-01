using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

public class Mandat
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid MandantId { get; private set; }
    public Guid MandataireId { get; private set; }
    public PorteeMandat Portee { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public bool EstPublic { get; private set; }
    public DateTime DateDebut { get; private set; }
    public DateTime? DateFin { get; private set; }

    protected Mandat() { }

    public Mandat(Guid mandantId, Guid mandataireId, PorteeMandat portee,
        string description, bool estPublic, DateTime dateDebut, DateTime? dateFin = null)
    {
        MandantId = mandantId;
        MandataireId = mandataireId;
        Portee = portee;
        Description = description;
        EstPublic = estPublic;
        DateDebut = dateDebut;
        DateFin = dateFin;
    }

    public bool EstActif => DateFin is null || DateFin > DateTime.UtcNow;

    public void Revoquer()
    {
        DateFin = DateTime.UtcNow;
    }
}
