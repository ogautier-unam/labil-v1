using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Journal.GetEntreesJournal;

public class GetEntreesJournalQueryHandler : IRequestHandler<GetEntreesJournalQuery, IReadOnlyList<EntreeJournalDto>>
{
    private readonly IEntreeJournalRepository _journalRepository;
    private readonly AppMapper _mapper;

    public GetEntreesJournalQueryHandler(IEntreeJournalRepository journalRepository, AppMapper mapper)
    {
        _journalRepository = journalRepository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<EntreeJournalDto>> Handle(GetEntreesJournalQuery request, CancellationToken cancellationToken)
    {
        var entrees = await _journalRepository.GetByActeurAsync(request.ActeurId, cancellationToken);
        return _mapper.ToDto(entrees);
    }
}
