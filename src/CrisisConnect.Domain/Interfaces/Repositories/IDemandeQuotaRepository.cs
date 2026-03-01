using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IDemandeQuotaRepository
{
    Task<DemandeQuota?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DemandeQuota>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DemandeQuota demande, CancellationToken cancellationToken = default);
    Task UpdateAsync(DemandeQuota demande, CancellationToken cancellationToken = default);
}
