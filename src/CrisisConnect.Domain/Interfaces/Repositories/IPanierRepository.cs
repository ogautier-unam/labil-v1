using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IPanierRepository
{
    Task<Panier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Panier>> GetByProprietaireAsync(Guid proprietaireId, CancellationToken cancellationToken = default);
    Task AddAsync(Panier panier, CancellationToken cancellationToken = default);
    Task UpdateAsync(Panier panier, CancellationToken cancellationToken = default);
}
