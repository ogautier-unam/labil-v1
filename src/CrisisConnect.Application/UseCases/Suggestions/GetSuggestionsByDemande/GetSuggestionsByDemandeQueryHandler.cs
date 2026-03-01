using AutoMapper;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using MediatR;

namespace CrisisConnect.Application.UseCases.Suggestions.GetSuggestionsByDemande;

public class GetSuggestionsByDemandeQueryHandler
    : IRequestHandler<GetSuggestionsByDemandeQuery, IReadOnlyList<SuggestionAppariementDto>>
{
    private readonly ISuggestionAppariementRepository _suggestionRepository;
    private readonly IMapper _mapper;

    public GetSuggestionsByDemandeQueryHandler(
        ISuggestionAppariementRepository suggestionRepository,
        IMapper mapper)
    {
        _suggestionRepository = suggestionRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SuggestionAppariementDto>> Handle(
        GetSuggestionsByDemandeQuery request,
        CancellationToken cancellationToken)
    {
        var suggestions = await _suggestionRepository.GetByDemandeAsync(request.DemandeId, cancellationToken);
        return _mapper.Map<IReadOnlyList<SuggestionAppariementDto>>(suggestions);
    }
}
