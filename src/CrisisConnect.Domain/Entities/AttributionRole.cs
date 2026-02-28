using CrisisConnect.Domain.Enums;

namespace CrisisConnect.Domain.Entities;

public class AttributionRole
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ActeurId { get; private set; }
    public TypeRole TypeRole { get; private set; }
    public Guid? AccrediteeParId { get; private set; }
    public DateTime DateDebut { get; private set; }
    public DateTime? DateFin { get; private set; }
    public bool Reconductible { get; private set; }
    public DateTime? DateRappel { get; private set; }
    public bool RappelEnvoye { get; private set; }
    public StatutRole Statut { get; private set; } = StatutRole.Actif;

    protected AttributionRole() { }

    public AttributionRole(Guid acteurId, TypeRole typeRole, DateTime dateDebut,
        DateTime? dateFin = null, bool reconductible = false, Guid? accrediteeParId = null)
    {
        ActeurId = acteurId;
        TypeRole = typeRole;
        DateDebut = dateDebut;
        DateFin = dateFin;
        Reconductible = reconductible;
        AccrediteeParId = accrediteeParId;
        if (dateFin.HasValue)
            DateRappel = dateFin.Value.AddDays(-7);
    }

    public void Expirer()
    {
        Statut = StatutRole.Expire;
    }

    public void MarquerRappelEnvoye()
    {
        RappelEnvoye = true;
    }
}
