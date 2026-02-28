using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IDemandeRepository
{
    Task<Demande?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Demande>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Demande demande, CancellationToken cancellationToken = default);
    Task UpdateAsync(Demande demande, CancellationToken cancellationToken = default);
}
