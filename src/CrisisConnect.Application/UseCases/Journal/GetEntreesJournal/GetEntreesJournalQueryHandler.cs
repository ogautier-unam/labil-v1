using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Journal.GetEntreesJournal;

public class GetEntreesJournalQueryHandler : IRequestHandler<GetEntreesJournalQuery, IReadOnlyList<EntreeJournalDto>>
{
    private readonly IEntreeJournalRepository _journalRepository;
    private readonly IMapper _mapper;

    public GetEntreesJournalQueryHandler(IEntreeJournalRepository journalRepository, IMapper mapper)
    {
        _journalRepository = journalRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EntreeJournalDto>> Handle(GetEntreesJournalQuery request, CancellationToken cancellationToken)
    {
        var entrees = await _journalRepository.GetByActeurAsync(request.ActeurId, cancellationToken);
        return _mapper.Map<IReadOnlyList<EntreeJournalDto>>(entrees);
    }
}
