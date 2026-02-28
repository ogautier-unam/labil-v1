using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IMethodeIdentificationRepository
{
    Task<MethodeIdentification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MethodeIdentification>> GetByPersonneAsync(Guid personneId, CancellationToken cancellationToken = default);
    Task AddAsync(MethodeIdentification methode, CancellationToken cancellationToken = default);
    Task UpdateAsync(MethodeIdentification methode, CancellationToken cancellationToken = default);
}
