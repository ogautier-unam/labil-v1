using CrisisConnect.Application.Mappings;
using CrisisConnect.Application.DTOs;
using CrisisConnect.Domain.Interfaces.Repositories;
using Mediator;

namespace CrisisConnect.Application.UseCases.Suggestions.GetSuggestionsByDemande;

public class GetSuggestionsByDemandeQueryHandler
    : IRequestHandler<GetSuggestionsByDemandeQuery, IReadOnlyList<SuggestionAppariementDto>>
{
    private readonly ISuggestionAppariementRepository _suggestionRepository;
    private readonly AppMapper _mapper;

    public GetSuggestionsByDemandeQueryHandler(
        ISuggestionAppariementRepository suggestionRepository,
        AppMapper mapper)
    {
        _suggestionRepository = suggestionRepository;
        _mapper = mapper;
    }

    public async ValueTask<IReadOnlyList<SuggestionAppariementDto>> Handle(
        GetSuggestionsByDemandeQuery request,
        CancellationToken cancellationToken)
    {
        var suggestions = await _suggestionRepository.GetByDemandeAsync(request.DemandeId, cancellationToken);
        return _mapper.ToDto(suggestions);
    }
}
