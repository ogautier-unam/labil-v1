using CrisisConnect.Domain.Entities;

namespace CrisisConnect.Domain.Interfaces.Repositories;

public interface ISuggestionAppariementRepository
{
    Task<SuggestionAppariement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SuggestionAppariement>> GetByOffreAsync(Guid offreId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SuggestionAppariement>> GetByDemandeAsync(Guid demandeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SuggestionAppariement>> GetNonAcknowledgedAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SuggestionAppariement suggestion, CancellationToken cancellationToken = default);
    Task UpdateAsync(SuggestionAppariement suggestion, CancellationToken cancellationToken = default);
}
