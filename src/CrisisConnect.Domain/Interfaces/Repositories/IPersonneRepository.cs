using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IPersonneRepository
{
    Task<Personne?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Personne?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Personne personne, CancellationToken cancellationToken = default);
    Task UpdateAsync(Personne personne, CancellationToken cancellationToken = default);
}
