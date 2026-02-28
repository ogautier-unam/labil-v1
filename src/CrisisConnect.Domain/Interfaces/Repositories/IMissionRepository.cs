using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IMissionRepository
{
    Task<Mission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Mission>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Mission mission, CancellationToken cancellationToken = default);
    Task UpdateAsync(Mission mission, CancellationToken cancellationToken = default);
}
