using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IMandatRepository
{
    Task<Mandat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Mandat>> GetByMandantAsync(Guid mandantId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Mandat>> GetByMandataireAsync(Guid mandataireId, CancellationToken cancellationToken = default);
    Task AddAsync(Mandat mandat, CancellationToken cancellationToken = default);
    Task UpdateAsync(Mandat mandat, CancellationToken cancellationToken = default);
}
