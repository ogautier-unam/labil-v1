using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IPropositionRepository
{
    Task<Proposition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Proposition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Proposition proposition, CancellationToken cancellationToken = default);
    Task UpdateAsync(Proposition proposition, CancellationToken cancellationToken = default);
    Task DeleteAsync(Proposition proposition, CancellationToken cancellationToken = default);
}
