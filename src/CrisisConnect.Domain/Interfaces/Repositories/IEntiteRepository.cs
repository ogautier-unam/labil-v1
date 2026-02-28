using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IEntiteRepository
{
    Task<Entite?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entite>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Entite entite, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entite entite, CancellationToken cancellationToken = default);
}
