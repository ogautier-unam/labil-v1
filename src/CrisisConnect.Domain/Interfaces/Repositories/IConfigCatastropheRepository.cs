using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface IConfigCatastropheRepository
{
    Task<ConfigCatastrophe?> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<ConfigCatastrophe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ConfigCatastrophe config, CancellationToken cancellationToken = default);
    Task UpdateAsync(ConfigCatastrophe config, CancellationToken cancellationToken = default);
}
