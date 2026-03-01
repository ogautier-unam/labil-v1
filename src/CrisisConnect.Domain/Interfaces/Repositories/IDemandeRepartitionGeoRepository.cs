using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IDemandeRepartitionGeoRepository
{
    Task<DemandeRepartitionGeo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DemandeRepartitionGeo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DemandeRepartitionGeo demande, CancellationToken cancellationToken = default);
    Task UpdateAsync(DemandeRepartitionGeo demande, CancellationToken cancellationToken = default);
}
